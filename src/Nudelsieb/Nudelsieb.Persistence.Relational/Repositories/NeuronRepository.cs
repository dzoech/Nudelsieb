using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nudelsieb.Domain.Abstractions;
using Nudelsieb.Persistence.Relational.Entities;

namespace Nudelsieb.Persistence.Relational.Repositories
{
    public class NeuronRepository : Domain.Aggregates.INeuronRepository
    {
        private readonly BraindumpDbContext context;
        private readonly ILogger logger;

        public NeuronRepository(BraindumpDbContext context, ILogger<NeuronRepository> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IUnitOfWork UnitOfWork => context;
        public async Task<Domain.Aggregates.Neuron> GetByIdAsync(Guid id)
        {
            var dbNeuron = await context.Neurons
               .AsNoTracking()
               .Include(n => n.Groups)
               .ToSql(logger)
               .FirstOrDefaultAsync(n => n.Id == id);

            return MapNeuron(dbNeuron);
        }

        // TODO #DDD implement methods
        public async Task<List<Domain.Aggregates.Neuron>> GetAllAsync()
        {
            var neurons = await context.Neurons
                .AsNoTracking()
                .Include(n => n.Groups)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => MapNeuron(n))
                .ToListAsync();

            return neurons;
        }

        public async Task<List<Domain.Aggregates.Neuron>> GetByGroupAsync(string groupName)
        {
            var neurons = await context.Groups
                .AsNoTracking()
                .Where(g => g.Name == groupName)
                .OrderByDescending(g => g.Neuron.CreatedAt)
                .Select(g => MapNeuron(g.Neuron))
                .ToSql(logger)
                .ToListAsync();

            return neurons;
        }

        public async Task<Domain.Aggregates.Neuron> AddAsync(Domain.Aggregates.Neuron neuron)
        {
            var neuronEntity = new Neuron
            {
                Id = neuron.Id,
                Information = neuron.Information,
                Groups = neuron.Groups
                    .Select(g => new Group { Name = g })
                    .ToList(),
                CreatedAt = neuron.CreatedAt
            };

            context.Neurons.Add(neuronEntity);

            await context.SaveChangesAsync(); // TODO #DDD commit only via the UoW pattern

            return MapNeuron(neuronEntity);
        }

        public async Task<Domain.Aggregates.Neuron> UpdateAsync(Domain.Aggregates.Neuron neuron)
        {
            var dbNeuron = MapNeuron(neuron);
            context.Neurons.Update(dbNeuron);
            await context.SaveChangesAsync(); // TODO #DDD commit only via the UoW pattern
            return MapNeuron(dbNeuron);
        }

        private static Domain.Aggregates.Neuron MapNeuron(Neuron dbNeuron)
        {
            var n = new Domain.Aggregates.Neuron(dbNeuron.Information)
            {
                Id = dbNeuron.Id,
                Groups = dbNeuron.Groups.Select(g => g.Name).ToList(),
                CreatedAt = dbNeuron.CreatedAt
            };

            return n;
        }

        private static Neuron MapNeuron(Domain.Aggregates.Neuron neuron)
        {
            var dbNeuron = new Neuron
            {
                Id = neuron.Id,
                Information = neuron.Information,
                Groups = neuron.Groups
                    .Select(g => new Group
                    {
                        Name = g,
                        NeuronId = neuron.Id
                    })
                    .ToList()
            };

            return dbNeuron;
        }
    }
}
