using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nudelsieb.Domain.Abstractions;
using Nudelsieb.Domain.Aggregates;

namespace Nudelsieb.Persistence.Relational.Repositories
{
    public class NeuronRepository : INeuronRepository
    {
        private readonly BraindumpDbContext context;
        private readonly ILogger logger;

        public NeuronRepository(BraindumpDbContext context, ILogger<NeuronRepository> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IUnitOfWork UnitOfWork => context;
        public async Task<Neuron> GetByIdAsync(Guid id)
        {
            var dbNeuron = await context.Neurons
               .AsNoTracking()
               .Include(n => n.Groups)
               .Include(n => n.Reminders)
               .ToSql(logger)
               .FirstOrDefaultAsync(n => n.Id == id);

            return new Neuron("TODO");
        }
        public Task<List<Neuron>> GetAllAsync() => throw new NotImplementedException();
        public Task<List<Neuron>> GetByGroupAsync(string group) => throw new NotImplementedException();
        public Neuron Add(Neuron neuron) => context.Add(neuron).Entity;
        public Neuron Update(Neuron neuron) => throw new NotImplementedException();
    }
}
