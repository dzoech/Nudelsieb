using System.Threading;
using System.Threading.Tasks;

namespace Nudelsieb.Domain.Abstractions
{
    /// <summary>
    /// The Unit of Work pattern overarching multiple repositories.
    /// </summary>
    public interface IUnitOfWork // ? IDisposable
    {
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <returns>A flag indication wether the transaction was committed successfully.</returns>
        Task<bool> SaveAsync(CancellationToken cancellationToken = default);
    }
}
