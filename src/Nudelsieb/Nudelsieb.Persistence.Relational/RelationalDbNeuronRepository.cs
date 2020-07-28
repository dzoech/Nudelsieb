using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nudelsieb.Domain;
using Nudelsieb.Persistence.Relational.Entities;

namespace Nudelsieb.Persistence.Relational
{
    public class RelationalDbNeuronRepository : INeuronRepository
    {
        private readonly BraindumpDbContext context;

        public RelationalDbNeuronRepository(BraindumpDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Domain.Neuron neuron)
        {
            context.Neurons.Add(new Entities.Neuron
            {
                Id = neuron.Id,
                Information = neuron.Information,
                Groups = neuron.Groups
                    .Select(g => new Entities.Group { Name = g })
                    .ToList()
            });

            await context.SaveChangesAsync();
        }

        public async Task<List<Domain.Neuron>> GetAllAsync()
        {
            var neurons = await context.Neurons
                .Select(n => new Domain.Neuron(n.Information)
                {
                    Id = n.Id,
                    Groups = n.Groups.Select(g => g.Name).ToList()
                })
                .ToListAsync();

            return neurons;
        }
    }
}
