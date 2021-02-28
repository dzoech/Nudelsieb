using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Nudelsieb.Mobile.Droid
{
    internal class FcmDataNotification
    {
        public Guid NeuronId { get; }

        public string NeuronInformation { get; }

        public IReadOnlyCollection<string> Groups { get; }

        public FcmDataNotification(IDictionary<string, string> data)
        {
            NeuronId = Guid.Parse(data["neuronId"]);
            NeuronInformation = data["neuronInformation"];

            var groups = JsonSerializer.Deserialize<string[]>(data["groups"]);
            Groups = new ReadOnlyCollection<string>(groups);
        }
    }
}