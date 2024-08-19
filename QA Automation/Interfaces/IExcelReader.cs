using System.Collections.Generic;

namespace QA_Automation.Interfaces
{
    // Interface to abstract Excel reading functionality
    public interface IExcelReader
    {
        List<UserData> ReadUserData(string filePath);
    }
}
