namespace QA_Automation
{
    public class UserData
    {
        public string UserName { get; }
        public string Email { get; }

        public UserData(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }
    }
}