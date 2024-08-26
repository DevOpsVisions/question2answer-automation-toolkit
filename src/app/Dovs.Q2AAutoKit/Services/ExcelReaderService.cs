using ExcelDataReader;
using Dovs.Q2AAutoKit.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace Dovs.Q2AAutoKit.Services
{
    public class ExcelReaderService : IExcelReaderService
    {
        private readonly IConfigurationService _configurationService;

        public ExcelReaderService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public List<UserData> ReadUserData(string filePath)
        {
            List<UserData> userDataList = new List<UserData>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // Read column names from the configuration service
            string userNameColumn = _configurationService.GetConfigValue("UserNameColumn");
            string emailColumn = _configurationService.GetConfigValue("EmailColumn");

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var header = new Dictionary<string, int>();
                bool isHeaderProcessed = false;

                while (reader.Read())
                {
                    if (!isHeaderProcessed)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            header[reader.GetValue(i).ToString()] = i;
                        }

                        if (!header.ContainsKey(userNameColumn) || !header.ContainsKey(emailColumn))
                        {
                            throw new Exception($"Required columns '{userNameColumn}' or '{emailColumn}' not found.");
                        }

                        isHeaderProcessed = true;
                        continue;
                    }

                    string userName = reader.GetValue(header[userNameColumn])?.ToString();
                    string email = reader.GetValue(header[emailColumn])?.ToString();

                    if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(email))
                    {
                        userDataList.Add(new UserData(userName, email));
                    }
                }
            }

            if (userDataList.Count == 0)
            {
                throw new Exception("No user data found in Excel.");
            }

            return userDataList;
        }
    }
}

