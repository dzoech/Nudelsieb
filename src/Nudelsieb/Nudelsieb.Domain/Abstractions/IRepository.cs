﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Domain.Abstractions
{
    /// <summary>
    /// A repository in the sense of Domain-driven Desing. It retrieves a complete neuron aggregate
    /// from the persistence.
    /// </summary>
    /// <typeparam name="T">The type of the Aggregate Root.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Gets he Unit of Work this repository belongs to.
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }
}
