using System.Threading.Tasks;
using Domain.Interfaces.Commands;

namespace Domain.Interfaces.Handlers
{
    public interface IHandler<T> where T : ICommandResult
    {
        Task<ICommandResult> Handler(T command);
    }
}