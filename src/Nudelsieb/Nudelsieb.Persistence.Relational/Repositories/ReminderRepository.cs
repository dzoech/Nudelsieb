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
    internal class ReminderRepository : Domain.Aggregates.IReminderRepository
    {
        private readonly BraindumpDbContext context;
        private readonly ILogger<ReminderRepository> logger;

        public ReminderRepository(BraindumpDbContext context, ILogger<ReminderRepository> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IUnitOfWork UnitOfWork => context;

        public async Task<List<Domain.Aggregates.Reminder>> GetRemindersAsync(DateTimeOffset until)
        {
            return await context.Reminders
                .AsNoTracking()
                .Include(r => r.Subject).ThenInclude(n => n.Groups)
                .Where(r => r.At <= until)
                .Select(r => Domain.Aggregates.Reminder.Reconstitute(
                    r.Id,
                    r.SubjectId,
                    r.At,
                    MapReminderState(r.State)))
                .ToSql(logger)
                .ToListAsync();
        }

        public async Task<List<Domain.Aggregates.Reminder>> GetRemindersAsync(DateTimeOffset until, Domain.Aggregates.ReminderState state)
        {
            return await context.Reminders
                .AsNoTracking()
                .Include(r => r.Subject).ThenInclude(n => n.Groups)
                .Where(r => r.At <= until && r.State == MapReminderState(state))
                .Select(r => Domain.Aggregates.Reminder.Reconstitute(
                    r.Id,
                    r.SubjectId,
                    r.At,
                    MapReminderState(r.State)))
                .ToSql(logger)
                .ToListAsync();
        }

        public async Task AddAsync(Domain.Aggregates.Reminder reminder)
        {
            var dbReminder = MapReminder(reminder);
            context.Reminders.AddRange(dbReminder);
            await context.SaveChangesAsync();
        }

        private static Reminder MapReminder(Domain.Aggregates.Reminder reminder)
        {
            return new Reminder
            {
                Id = reminder.Id,
                SubjectId = reminder.NeuronReference,
                At = reminder.At,
                State = MapReminderState(reminder.State)
            };
        }

        private static Domain.Aggregates.ReminderState MapReminderState(ReminderState dbState)
        {
            return dbState switch
            {
                ReminderState.Waiting => Domain.Aggregates.ReminderState.Waiting,
                ReminderState.Active => Domain.Aggregates.ReminderState.Active,
                ReminderState.Disabled => Domain.Aggregates.ReminderState.Disabled,
                _ => throw new ArgumentException(
                        $"Cannot map {nameof(ReminderState)} {dbState} to {nameof(Domain.Aggregates.ReminderState)}.",
                        nameof(dbState)),
            };
        }

        private static ReminderState MapReminderState(Domain.Aggregates.ReminderState state)
        {
            return state switch
            {
                Domain.Aggregates.ReminderState.Waiting => ReminderState.Waiting,
                Domain.Aggregates.ReminderState.Active => ReminderState.Active,
                Domain.Aggregates.ReminderState.Disabled => ReminderState.Disabled,
                _ => throw new ArgumentException(
                        $"Cannot map {nameof(Domain.Aggregates.ReminderState)} {state} to {nameof(ReminderState)}.",
                        nameof(state)),
            };
        }
    }
}
