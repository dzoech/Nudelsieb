using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nudelsieb.Domain.Abstractions;
using Nudelsieb.Persistence.Relational.Entities;

namespace Nudelsieb.Persistence.Relational
{
    /// <remarks>
    /// https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext#using-dbcontext-with-dependency-injection
    /// </remarks>
    public class BraindumpDbContext : DbContext, IUnitOfWork
    {
        public BraindumpDbContext(DbContextOptions<BraindumpDbContext> options)
            : base(options)
        {
        }

        public DbSet<Neuron> Neurons { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        public async Task<bool> SaveAsync(CancellationToken cancellationToken = default)
        {
            await this.SaveChangesAsync(cancellationToken);

            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Neuron>().HasKey(n => n.Id);
            modelBuilder.Entity<Neuron>().Property(n => n.Information).IsRequired();
            modelBuilder.Entity<Neuron>().Property(n => n.CreatedAt).IsRequired();

            modelBuilder.Entity<Group>().HasKey(g => new { g.Name, g.NeuronId });
            modelBuilder.Entity<Group>().HasOne<Neuron>(g => g.Neuron).WithMany(n => n.Groups).HasForeignKey(g => g.NeuronId);

            modelBuilder.Entity<Reminder>().HasKey(r => r.Id);
            modelBuilder.Entity<Reminder>().HasOne<Neuron>(r => r.Subject).WithMany(n => n.Reminders);
            modelBuilder.Entity<Reminder>().Property(r => r.At).IsRequired();
        }
    }
}
