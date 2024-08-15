using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using ExcelDataReader;

namespace QA_Automation
{
    class Program
    {
        static void Main()
        {
            string basePath = GetBasePath();
            string defaultFilePath = Path.Combine(basePath, "default.xlsx");
            Console.WriteLine("Current default path is: " + defaultFilePath);

            string filePath = GetFilePath(defaultFilePath);
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                Console.WriteLine("File not found. Please provide a valid file path.");
                return;
            }

            Console.WriteLine("Enter the password to use for registration:");
            string password = ReadPassword();

            using (IWebDriver driver = new ChromeDriver())
            {
                var userDataList = ReadUserDataFromExcel(filePath);
                bool allUsersRegistered = RegisterUsers(driver, userDataList, password);

                Console.WriteLine(allUsersRegistered ? "All users have been registered successfully." : "Registration process encountered errors.");
                driver.Quit();
            }
        }

        static string GetBasePath()
        {
            string currentDir = Directory.GetCurrentDirectory();
            for (int i = 0; i < 4; i++)
            {
                currentDir = Directory.GetParent(currentDir).FullName;
            }
            return currentDir;
        }

        static string GetFilePath(string defaultFilePath)
        {
            if (File.Exists(defaultFilePath))
            {
                Console.WriteLine("File Found: Press \n1 to use the default path \n2 to enter your own path:");
            }
            else
            {
                Console.WriteLine($"File not found: Make sure to copy it first to {defaultFilePath}. \nPress \n1 to use the default path \n2 to enter your own path:");
            }
        
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                return defaultFilePath;
            }
            else if (choice == "2")
            {
                Console.WriteLine("Please enter your own path:");
                string customPath = Console.ReadLine();
                return customPath;
            }
            else
            {
                Console.WriteLine("Invalid choice. Returning null.");
                return null;
            }
        }

        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[0..^1];
                    Console.Write("\b \b");
                }
                else if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        static List<UserData> ReadUserDataFromExcel(string filePath)
        {
            List<UserData> userDataList = new List<UserData>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // Read column names from App.config
            string userNameColumn = ConfigurationManager.AppSettings["UserNameColumn"];
            string emailColumn = ConfigurationManager.AppSettings["EmailColumn"];

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // Initialize a flag to check if the header is processed
                bool isHeaderProcessed = false;
                var header = new Dictionary<string, int>();

                while (reader.Read())
                {
                    // Process the header row
                    if (!isHeaderProcessed)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            header[reader.GetValue(i).ToString()] = i;
                        }

                        // Check if required columns exist
                        if (!header.ContainsKey(userNameColumn) || !header.ContainsKey(emailColumn))
                        {
                            throw new Exception($"Required columns '{userNameColumn}' or '{emailColumn}' not found in Excel.");
                        }

                        isHeaderProcessed = true;
                        continue; // Skip to the next row after processing the header
                    }

                    // Process the data rows
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

        
        static bool RegisterUsers(IWebDriver driver, List<UserData> userDataList, string password)
        {
            string registrationUrl = ConfigurationManager.AppSettings.Get("RegistrationUrl");
            foreach (var userData in userDataList)
            {
                Console.WriteLine($"Registering user: {userData.UserName}, {userData.Email}");
                driver.Navigate().GoToUrl(registrationUrl);
                FillRegistrationForm(driver, userData, password);
                ClickRegisterButton(driver);
                System.Threading.Thread.Sleep(1000);
                Logout(driver);
        
                if (!IsRegistrationFormPresent(driver))
                {
                    Console.WriteLine("User registered successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to register user.");
                    return false;
                }
            }
            return true;
        }

        static void FillRegistrationForm(IWebDriver driver, UserData userData, string password)
        {
            driver.FindElement(By.Id("handle")).SendKeys(userData.UserName);
            driver.FindElement(By.Id("password")).SendKeys(password);
            driver.FindElement(By.Id("email")).SendKeys(userData.Email);
        }

        static void ClickRegisterButton(IWebDriver driver)
        {
            try
            {
                var registerButton = driver.FindElement(By.CssSelector(".qa-form-tall-button-register[value='Register']"));
                registerButton.Click();
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Registration process completed.");
            }
        }

        static void Logout(IWebDriver driver)
        {
            string logoutUrl = ConfigurationManager.AppSettings.Get("LogoutUrl");
            driver.Navigate().GoToUrl(logoutUrl);
        }

        static bool IsRegistrationFormPresent(IWebDriver driver)
        {
            try
            {
                driver.FindElement(By.Id("handle"));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}