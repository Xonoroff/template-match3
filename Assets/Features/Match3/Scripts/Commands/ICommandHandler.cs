using Cysharp.Threading.Tasks;

namespace Features.Match3.Scripts.Services
{
    public interface ICommandHandler<T> where T : ICommand
    {
        UniTask<HandlerResult> Handle(T command);
    }
}