using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Managers;
using Features.Match3.Scripts.Presenters;
using Features.Match3.Scripts.Services;
using Features.Match3.Scripts.Views;
using UnityEngine;

namespace Features.Match3.Scripts
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private BoardView _view;
        [SerializeField] private int _width = 8;
        [SerializeField] private int _height = 8;
        [SerializeField] private int _seed = 42;

        private Match3Manager _manager;
        private BoardPresenter _presenter;

        private void Start()
        {
            var types = new List<TileTypeID>
            {
                new TileTypeID(1), // Red
                new TileTypeID(2), // Green
                new TileTypeID(3), // Blue
                new TileTypeID(4), // Yellow
                new TileTypeID(5)  // Purple
            };
            
            IGridService gridSys = new StandardGridService();
            IMatchService matchSys = new StandardMatchService();
            IGravityService gravitySys = new StandardGravityService();
            IMatch3Evaluator evaluator = new StandardMatch3Evaluator(gridSys, matchSys, gravitySys);
            MockMatch3API match3API = new MockMatch3API(evaluator, gridSys);
            ActivateTileHandler handler = new ActivateTileHandler(evaluator);
            
            _manager = new Match3Manager(handler, match3API);
            _presenter = new BoardPresenter(_manager, _view);

            var mockConfig = new LevelConfigEntity()
            {
                AvailableTileTypes = types, Width = _width, Height = _height,
            };
            match3API.MockConfig(mockConfig, _seed);

            InitializeGame().Forget();
        }

        private async UniTaskVoid InitializeGame()
        {
            await _presenter.StartLevelAsync(-1);
        }
    }
}
