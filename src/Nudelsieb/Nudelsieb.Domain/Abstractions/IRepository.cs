using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Domain.Abstractions
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Gets he Unit of Work this repository belongs to.
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }
}
