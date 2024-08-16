using System.Collections.Generic;

namespace QA_Automation.Interfaces
{
    // Interface to abstract the user registration process
    public interface IUserRegistrationService
    {
        bool RegisterUsers(List<UserData> users, string password);
    }
}
