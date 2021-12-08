using System.Collections.Generic;

namespace Nudelsieb.Domain.Aggregates.NeuronAggregate
{
    public class Group : ValueObject
    {
        public Group(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Group name must not be empty or white spaces only.");

            Name = name.Trim();
        }

        public string Name { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
