using ExcelDataReader;
using QA_Automation.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace QA_Automation.Services
{
    // Class responsible for reading user data from an Excel file
    public class ExcelReader : IExcelReader
    {
        /// <summary>
        /// Reads user data (username and email) from the specified Excel file.
        /// Throws an exception if the necessary columns are not found or no data is present.
        /// </summary>
        /// <param name="filePath">Path to the Excel file containing user data.</param>
        /// <returns>List of user data (username and email).</returns>
        public List<UserData> ReadUserData(string filePath)
        {
            List<UserData> userDataList = new List<UserData>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // Read column names from the app.config file
            string userNameColumn = ConfigurationManager.AppSettings["UserNameColumn"];
            string emailColumn = ConfigurationManager.AppSettings["EmailColumn"];

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var header = new Dictionary<string, int>();
                bool isHeaderProcessed = false;

                while (reader.Read())
                {
                    // Process the header row to map column names to their indices
                    if (!isHeaderProcessed)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            header[reader.GetValue(i).ToString()] = i;
                        }

                        // Ensure the required columns are present in the Excel file
                        if (!header.ContainsKey(userNameColumn) || !header.ContainsKey(emailColumn))
                        {
                            throw new Exception($"Required columns '{userNameColumn}' or '{emailColumn}' not found.");
                        }

                        isHeaderProcessed = true;
                        continue; // Move to the next row after processing the header
                    }

                    // Read and validate the username and email for each user
                    string userName = reader.GetValue(header[userNameColumn])?.ToString();
                    string email = reader.GetValue(header[emailColumn])?.ToString();

                    if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(email))
                    {
                        userDataList.Add(new UserData(userName, email)); // Add valid user data to the list
                    }
                }
            }

            // Ensure there is user data present
            if (userDataList.Count == 0)
            {
                throw new Exception("No user data found in Excel.");
            }

            return userDataList;
        }
    }
}
