using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Client
{
    public class Services
    {
        private static IServiceProvider _serviceProvider;

        public static void Init(IServiceProvider serviceProvider)
        {
            if (_serviceProvider == null)
            {
                _serviceProvider = serviceProvider;
            }
        }

        public static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}