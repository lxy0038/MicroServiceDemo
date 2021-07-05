using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Insfratructure.Extensions
{
    public class ConfigurationSectionExtension : IConfigurationSection, IConfiguration
    {
        private readonly string sectionKey;

        private IConfiguration configuration;

        public string Key
        {
            get;
        }

        public string Path
        {
            get;
        }

        public string Value
        {
            get;
            set;
        }

        public string this[string key]
        {
            get
            {
                string enviromentValue = GetEnviromentValue(key);
                if (string.IsNullOrEmpty(enviromentValue))
                {
                    return configuration.GetSection(sectionKey)[key];
                }
                return enviromentValue;
            }
            set
            {
                configuration.GetSection(sectionKey)[key] = value;
            }
        }

        public ConfigurationSectionExtension(string sectionKey, IConfiguration configuration)
        {
            this.sectionKey = sectionKey;
            this.configuration = configuration;
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }

        protected virtual string GetEnviromentValue(string key)
        {
            return System.Environment.GetEnvironmentVariable(key);
        }
    }
}

