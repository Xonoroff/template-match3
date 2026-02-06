using System.Collections.Generic;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Managers;
using Features.Match3.Scripts.Presenters;
using Features.Match3.Scripts.Services;
using Features.Match3.Scripts.Views;
using UnityEngine;

namespace Features.Match3.Scripts
{
    public class Match3Context : MonoBehaviour
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

            ICommandLogService logService = new CommandLogService();
            ActivateTileHandler handler = new ActivateTileHandler(evaluator);

            // 3. Initial State
            // Create a random grid using the grid system refill logic?
            // Or just a fresh grid then refill.
            var grid = new GridEntity(_width, _height);
            var (filledGrid, _) = gridSys.Refill(grid, _seed, types);

            // 4. Manager
            _manager = new Match3Manager(logService, handler, config, filledGrid);

            // 5. Presenter
            _presenter = new BoardPresenter(_manager, _view);
            _presenter.Initialize(); // Draws initial state

            // 6. Start
            _manager.StartGame();
        }
    }
}
