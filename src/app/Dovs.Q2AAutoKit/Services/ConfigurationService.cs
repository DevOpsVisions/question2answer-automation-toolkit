using Dovs.Q2AAutoKit.Interfaces;
using System.Configuration;

namespace Dovs.Q2AAutoKit.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
    
}
