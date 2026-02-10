using Cysharp.Threading.Tasks;

namespace Features.Match3.Scripts.Commands
{
    public interface ICommandHandler<T> where T : ICommand
    {
        UniTask<HandlerResult> Handle(T command);
    }
}