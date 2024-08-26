using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovs.Q2AAutoKit.Interfaces
{
    public interface IConfigurationService
    {
        string GetConfigValue(string key);
    }
}
