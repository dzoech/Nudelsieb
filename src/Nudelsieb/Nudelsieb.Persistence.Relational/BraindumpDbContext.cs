using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Nudelsieb.Persistence.Relational.Entities;

namespace Nudelsieb.Persistence.Relational
{
    // https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext#using-dbcontext-with-dependency-injection
    public class BraindumpDbContext : DbContext
    {
        public BraindumpDbContext(DbContextOptions<BraindumpDbContext> options)
            : base(options)
        { }

        public DbSet<Neuron> Neurons { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Neuron>().HasKey(n => n.Id);
            modelBuilder.Entity<Neuron>().Property(n => n.Information).IsRequired();

            modelBuilder.Entity<Group>().HasKey(g => new { g.Name, g.NeuronId });
            modelBuilder.Entity<Group>().HasOne<Neuron>(g => g.Neuron).WithMany(n => n.Groups).HasForeignKey(g => g.NeuronId);

            modelBuilder.Entity<Reminder>().HasKey(r => r.Id);
            modelBuilder.Entity<Reminder>().HasOne<Neuron>(r => r.Subject).WithMany(n => n.Reminders);
            modelBuilder.Entity<Reminder>().Property(r => r.At).IsRequired();
        }
    }
}
