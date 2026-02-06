using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;

namespace Features.Match3.Scripts.Services
{
    public interface ICommandHandler<T> where T : ICommand
    {
        UniTask<HandlerResult> Handle(T command);
    }
}