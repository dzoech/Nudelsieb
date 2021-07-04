using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nudelsieb.Notifications.Notifyer
{
    public class AndroidReminderBuilder
    {
        private readonly List<string> _groups = new List<string>();
        private string _neuronInformation = string.Empty;
        private Guid _neuronId = default;

        public AndroidReminderBuilder WithNeuron(Guid id, string neuronInformation)
        {
            _neuronId = id;
            _neuronInformation = neuronInformation;
            return this;
        }

        public AndroidReminderBuilder WithGroups(params string[] groups)
        {
            _groups.AddRange(groups);
            return this;
        }

        public string Build()
        {
            if (_neuronId == default)
                throw new ReminderBuilderException("Neuron ID must be set");

            var reminder = new
            {
                Data = new
                {
                    NeuronId = _neuronId,
                    NeuronInformation = _neuronInformation,
                    Groups = _groups
                }
            };

            return JsonSerializer.Serialize(reminder, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
