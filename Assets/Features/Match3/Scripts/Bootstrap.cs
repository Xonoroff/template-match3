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
            // 1. Config
            var types = new List<TileTypeID>
            {
                new TileTypeID(1), // Red
                new TileTypeID(2), // Green
                new TileTypeID(3), // Blue
                new TileTypeID(4), // Yellow
                new TileTypeID(5)  // Purple
            };
            
            var config = new LevelConfigEntity
            {
                Width = _width,
                Height = _height,
                Seed = _seed,
                AvailableTileTypes = types
            };

            // 2. Services / Systems
            IGridService gridSys = new StandardGridService();
            IMatchService matchSys = new StandardMatchService();
            IGravityService gravitySys = new StandardGravityService();
            IMatch3Evaluator evaluator = new StandardMatch3Evaluator(gridSys, matchSys, gravitySys);

            ActivateTileHandler handler = new ActivateTileHandler(evaluator);

            var grid = new GridEntity(_width, _height);
            var (filledGrid, _) = gridSys.Refill(grid, _seed, types);

            _manager = new Match3Manager(handler, config, filledGrid);
            _presenter = new BoardPresenter(_manager, _view);
            
            _presenter.InitializeAsync().Forget();
            
        }
    }
}
