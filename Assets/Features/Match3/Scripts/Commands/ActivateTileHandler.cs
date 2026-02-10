using System;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;

namespace Features.Match3.Scripts.Services
{
    public class ActivateTileHandler : ICommandHandler<ActivateTileCommand>
    {
        private readonly IMatch3Evaluator _evaluator;

        public ActivateTileHandler(IMatch3Evaluator evaluator)
        {
            _evaluator = evaluator;
        }
        
        public UniTask<HandlerResult> Handle(ActivateTileCommand command)
        {
            
            var evaluationResult =  _evaluator.ResolveTap(command.CurrentState, command.Coordinate.X, command.Coordinate.Y, command.Config);
            var result = new HandlerResult()
            {
                ResolvedSequence = evaluationResult,
            };

            return UniTask.FromResult(result);
        }
    }
}
