using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nudelsieb.Domain;

namespace Nudelsieb.Persistence
{
    public class DummyNeuronRepository : INeuronRepository
    {
        private readonly List<Neuron> neuronInMemoryStore = new List<Neuron>
        {
            new Neuron("An unbelievable idea that I would otherwise forget instantly...")
            {
                Id = Guid.Parse("C5A0CF07-0DF4-4AD9-B299-363F0BE44509"),
                Groups = new List<string> { "projects", "programming" }
            },
            new Neuron("Remind me to turn off the oven.")
            {
                Id = Guid.Parse("C4F56878-1971-4620-9084-4204EE779D84")
            },
            new Neuron("Keep in mind that the API is only a dummy yet.")
            {
                Id = Guid.Parse("285C8181-BEAE-4F2C-AC41-A4E0C0870780"),
                Groups = new List<string> { "programming" }
            },
            new Neuron("Why is my kitchen burning down?")
            {
                Id = Guid.Parse("8C67AD3B-8AF6-4710-8ACD-C9CA282B1788")
            },
        };

        public Task AddAsync(Neuron neuron)
        {
            return Task.Run(() => this.neuronInMemoryStore.Add(neuron));
        }

        public Task<List<Neuron>> GetAllAsync()
        {
            return Task.FromResult(this.neuronInMemoryStore);
        }

        public Task<List<Neuron>> GetByGroupAsync(string group)
        {
            return Task.FromResult(
                neuronInMemoryStore
                    .Where(n => n.Groups
                    .Contains(group))
                    .ToList());
        }
        public Task<List<Reminder>> GetRemindersAsync(DateTimeOffset until)
        {
            throw new NotImplementedException();
        }

        public Task<List<Reminder>> GetRemindersAsync(DateTimeOffset until, ReminderState state)
        {
            throw new NotImplementedException();
        }
    }
}
