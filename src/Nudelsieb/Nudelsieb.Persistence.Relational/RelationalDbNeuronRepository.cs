using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nudelsieb.Application.Persistence;


namespace Nudelsieb.Persistence.Relational
{
    public class RelationalDbNeuronRepository : INeuronRepository
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
            var neuronEntity = new Entities.Neuron
            {
                Id = neuron.Id,
                Information = neuron.Information,
                Groups = neuron.Groups
                    .Select(g => new Entities.Group { Name = g })
                    .ToList(),
                CreatedAt = neuron.CreatedAt
            };


            context.Neurons.Add(neuronEntity);

            await context.SaveChangesAsync();

            var reminders = neuron.Reminders
                .Select(r => new Entities.Reminder
                {
                    Id = r.Id,
                    At = r.At,
                    State = MapReminderState(r.State),
                    Subject = neuronEntity
                });

            context.Reminders.AddRange(reminders);

            await context.SaveChangesAsync();
        }

        public async Task AddRemindersAsync(List<Domain.Reminder> reminders)
        {
            var dbReminders = MapRemindersWithSubjectIdOnly(reminders);
            context.Reminders.AddRange(dbReminders);
            var e = context.ChangeTracker.Entries();
            await context.SaveChangesAsync();
        }

        public async Task<Domain.Neuron> GetByIdAsync(Guid id)
        {
            var dbNeuron = await context.Neurons
                .AsNoTracking()
                .Include(n => n.Groups)
                .Include(n => n.Reminders)
                .ToSql(logger)
                .FirstOrDefaultAsync(n => n.Id == id);

            return MapNeuron(dbNeuron);
        }

        public async Task<List<Domain.Neuron>> GetAllAsync()
        {
            var neurons = await context.Neurons
                .AsNoTracking()
                .Include(n => n.Groups)
                .Include(n => n.Reminders)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => MapNeuron(n))
                .ToListAsync();

            return neurons;
        }

        public async Task<List<Domain.Neuron>> GetByGroupAsync(string groupName)
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

        public async Task<List<Domain.Reminder>> GetRemindersAsync(DateTimeOffset until)
        {
            return await context.Reminders
                .AsNoTracking()
                .Include(r => r.Subject).ThenInclude(n => n.Groups)
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
                .AsNoTracking()
                .Include(r => r.Subject).ThenInclude(n => n.Groups)
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

        private static List<Domain.Reminder> MapReminders(IEnumerable<Entities.Reminder> dbReminders, Domain.Neuron subject)
        {
            var reminders = dbReminders
                .Select(r => new Domain.Reminder(subject)
                {
                    Id = r.Id,
                    At = r.At,
                    State = MapReminderState(r.State)
                })
                .ToList();

            return reminders;
        }

        private static List<Entities.Reminder> MapRemindersWithSubjectIdOnly(IEnumerable<Domain.Reminder> reminders)
        {
            var dbReminders = reminders
                .Select(r => new Entities.Reminder
                {
                    Id = r.Id,
                    SubjectId = r.Subject.Id,
                    At = r.At,
                    State = MapReminderState(r.State)
                })
                .ToList();

            return dbReminders;
        }

        private static Entities.Neuron MapNeuronWithIdOnly(Domain.Neuron neuron)
        {
            return new Entities.Neuron
            {
                Id = neuron.Id
            };
        }

        private static List<Entities.Reminder> MapReminders(IEnumerable<Domain.Reminder> reminders, Entities.Neuron subject)
        {
            var dbReminders = reminders
                .Select(r => new Entities.Reminder
                {
                    Id = r.Id,
                    Subject = subject,
                    At = r.At,
                    State = MapReminderState(r.State)
                })
                .ToList();

            return dbReminders;
        }

        private static Domain.Neuron MapNeuron(Entities.Neuron dbNeuron)
        {
            var n = new Domain.Neuron(dbNeuron.Information)
            {
                Id = dbNeuron.Id,
                Groups = dbNeuron.Groups.Select(g => g.Name).ToList(),
                CreatedAt = dbNeuron.CreatedAt
            };

            n.Reminders = MapReminders(dbNeuron.Reminders, n);

            return n;
        }

        private static Entities.Neuron MapNeuron(Domain.Neuron neuron)
        {
            var dbNeuron = new Entities.Neuron
            {
                Id = neuron.Id,
                Information = neuron.Information,
                Groups = neuron.Groups.Select(g => new Entities.Group
                {
                    Name = g,
                    NeuronId = neuron.Id
                }).ToList()
            };

            dbNeuron.Reminders = MapReminders(neuron.Reminders, dbNeuron);

            return dbNeuron;
        }

        private static Domain.ReminderState MapReminderState(Entities.ReminderState dbState)
        {
            return dbState switch
            {
                Entities.ReminderState.Waiting => Domain.ReminderState.Waiting,
                Entities.ReminderState.Active => Domain.ReminderState.Active,
                Entities.ReminderState.Disabled => Domain.ReminderState.Disabled,
                _ => throw new ArgumentException(
                        $"Cannot map {nameof(Entities.ReminderState)} {dbState} to {nameof(Domain.ReminderState)}.",
                        nameof(dbState)),
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
