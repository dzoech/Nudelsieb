using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient.Server;

namespace Nudelsieb.Persistence.Relational.Entities
{
    public class Neuron
    {
        public Guid Id { get; set; }

        public string Information { get; set; }

        public ICollection<Group> Groups { get; set; }

        public DateTime? Reminder { get; set; }

    }
}
