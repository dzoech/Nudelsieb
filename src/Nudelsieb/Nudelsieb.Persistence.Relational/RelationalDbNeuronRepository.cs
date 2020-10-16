using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Nudelsieb.Persistence.Relational
{
    public class RelationalDbNeuronRepository : Domain.INeuronRepository
    {
        private readonly ILogger<RelationalDbNeuronRepository> logger;
        private readonly BraindumpDbContext context;

        public RelationalDbNeuronRepository(ILogger<RelationalDbNeuronRepository> logger, BraindumpDbContext context)
        {
            this.logger = logger;
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
                .Select(n => MapNeuron(n))
                .ToListAsync();

            return neurons;
        }

        private static List<Domain.Reminder> MapReminders(Entities.Neuron n, Domain.Neuron subject)
        {
            var reminders = n.Reminders.Select(r => new Domain.Reminder(subject)
            {
                Id = r.Id,
                At = r.At,
                State = MapReminderState(r.State)
            })
            .ToList();

            return reminders;
        }

        public async Task<List<Domain.Neuron>> GetByGroupAsync(string groupName)
        {
            var neurons = await context.Groups
                .Where(g => g.Name == groupName)
                .Select(g => MapNeuron(g.Neuron))
                .ToSql(logger)
                .ToListAsync();

            return neurons;
        }

        public async Task<List<Domain.Reminder>> GetRemindersAsync(DateTimeOffset until)
        {
            return await context.Reminders
                .Where(r => r.At <= until)
                .Select(r => new Domain.Reminder(MapNeuron(r.Subject))
                {
                    Id = r.Id,
                    At = r.At,
                    State = MapReminderState(r.State)
                })
                .ToSql(logger)
                .ToListAsync();
        }

        public async Task<List<Domain.Reminder>> GetRemindersAsync(DateTimeOffset until, Domain.ReminderState state)
        {
            return await context.Reminders
                .Where(r => r.At <= until && r.State == MapReminderState(state))
                .Select(r => new Domain.Reminder(MapNeuron(r.Subject))
                {
                    Id = r.Id,
                    At = r.At,
                    State = MapReminderState(r.State)
                })
                .ToSql(logger)
                .ToListAsync();
        }

        private static Domain.Neuron MapNeuron(Entities.Neuron neuron)
        {
            var n = new Domain.Neuron(neuron.Information)
            {
                Id = neuron.Id,
                Groups = neuron.Groups.Select(g => g.Name).ToList()
            };

            n.Reminders = MapReminders(neuron, n);

            return n;
        }

        private static Domain.ReminderState MapReminderState(Entities.ReminderState state)
        {
            return state switch
            {
                Entities.ReminderState.Waiting => Domain.ReminderState.Waiting,
                Entities.ReminderState.Active => Domain.ReminderState.Active,
                Entities.ReminderState.Disabled => Domain.ReminderState.Disabled,
                _ => throw new ArgumentException(
                        $"Cannot map {nameof(Entities.ReminderState)} {state} to {nameof(Domain.ReminderState)}.",
                        nameof(state)),
            };
        }

        private static Entities.ReminderState MapReminderState(Domain.ReminderState state)
        {
            return state switch
            {
                Domain.ReminderState.Waiting => Entities.ReminderState.Waiting,
                Domain.ReminderState.Active => Entities.ReminderState.Active,
                Domain.ReminderState.Disabled => Entities.ReminderState.Disabled,
                _ => throw new ArgumentException(
                        $"Cannot map {nameof(Domain.ReminderState)} {state} to {nameof(Entities.ReminderState)}.",
                        nameof(state)),
            };
        }
    }
}
