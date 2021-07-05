using Common.Insfratructure.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Insfratructure.Utilities
{
    public static class ConfigurationManager
    {
        public static IConfiguration Configuration
        {
            get;
            private set;
        }

        public static IConfigurationSection AppSettings => new ConfigurationSectionExtension("AppSettings", Configuration);

        public static IConfigurationSection ConnectionStrings => new ConfigurationSectionExtension("ConnectionStrings", Configuration);

        public static void Register(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}

