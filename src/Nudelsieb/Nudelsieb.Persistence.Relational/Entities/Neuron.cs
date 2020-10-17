using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient.Server;

namespace Nudelsieb.Persistence.Relational.Entities
{
    public class Neuron
    {
        public Guid Id { get; set; } = Guid.Empty;

        public string Information { get; set; } = string.Empty;

        public ICollection<Group> Groups { get; set; } = Array.Empty<Group>();

        public ICollection<Reminder> Reminders { get; set; } = Array.Empty<Reminder>();
    }
}
