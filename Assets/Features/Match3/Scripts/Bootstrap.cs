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
        [SerializeField] private Match3ContentLoader _contentLoader; // Assign in Inspector
        [SerializeField] private int _width = 4;
        [SerializeField] private int _height = 4;
        [SerializeField] private int _seed = 42;

        private Match3Manager _manager;
        private BoardPresenter _presenter;

        private void Start()
        {
            var types = new List<TileTypeIDEntity>
            {
                new TileTypeIDEntity(1),
                new TileTypeIDEntity(2),
                new TileTypeIDEntity(3),
                new TileTypeIDEntity(4),
            };

            IGridService gridSys = new StandardGridService();
            IMatchService matchSys = new StandardMatchService();
            IGravityService gravitySys = new StandardGravityService();
            IMatch3Evaluator evaluator = new StandardMatch3Evaluator(gridSys, matchSys, gravitySys);
            MockMatch3API match3API = new MockMatch3API(evaluator, gridSys);
            ActivateTileHandler handler = new ActivateTileHandler(evaluator);

            _manager = new Match3Manager(handler, match3API);
            _presenter = new BoardPresenter(_manager, _view, _contentLoader, Debug.unityLogger);

            var mockConfig = new LevelConfigEntity()
            {
                AvailableColors = types.ToArray(),
                Width = _width,
                Height = _height,
                AtlasAddress = "tiles_default",
                SpritePrefix = "tile_"
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
