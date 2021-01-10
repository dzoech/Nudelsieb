using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;

namespace Nudelsieb.Mobile.Configuration
{
    public class SimpleOptions<T> : IOptions<T>
        where T : class
    {
        private readonly T instance;

        public SimpleOptions(T instance)
        {
            this.instance = instance;
        }

        public T Value => instance;
    }
}
