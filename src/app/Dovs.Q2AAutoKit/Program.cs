using Dovs.Q2AAutoKit;
using Dovs.Q2AAutoKit.Common;
using Dovs.Q2AAutoKit.Interfaces;
using Dovs.Q2AAutoKit.Services;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        DisplayMenu();
    }

    static void DisplayMenu()
    {
        Console.WriteLine("Welcome to Q2A Automation Toolkit!");
        Console.WriteLine("Please select an option:");
        Console.WriteLine("1. Register Users");
        Console.WriteLine("2. Remove Users");
        Console.WriteLine("3. Update Users");
        Console.WriteLine("4. Exit");

        int option = GetOptionFromUser();

        switch (option)
        {
            case 1:
                RegisterUsersFromExcel();
                DisplayMenu();
                break;
            case 2:
                RemoveUsers();
                DisplayMenu();
                break;
            case 3:
                UpdateUsers();
                DisplayMenu();
                break;
            case 4:
                Console.WriteLine("Exiting the App...");
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                DisplayMenu();
                break;
        }
    }

    static int GetOptionFromUser()
    {
        Console.Write("Enter the option number: ");
        string input = Console.ReadLine();
        int option;
        if (int.TryParse(input, out option))
        {
            return option;
        }
        else
        {
            Console.WriteLine("Invalid option. Please try again.");
            return GetOptionFromUser();
        }
    }

    static void RegisterUsersFromExcel()
    {
        const int LEVELSTRAVERSE = 2;

        IFilePathService filePathService = new FilePathService();
        IConfigurationService configurationService = new ConfigurationService(); 

        string basePath = filePathService.GetBasePath(LEVELSTRAVERSE);
        string[] excelFiles = filePathService.GetExcelFiles(basePath);

        if (excelFiles.Length == 0)
        {
            Console.WriteLine("No Excel files found in the directory.");
        }
        else
        {
            Console.WriteLine("Please select an Excel file:");
            for (int i = 0; i < excelFiles.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(excelFiles[i])}");
            }
        }

        Console.WriteLine($"{excelFiles.Length + 1}. Enter your own file path");

        int fileOption = GetOptionFromUser();
        if (fileOption == excelFiles.Length + 1)
        {
            Console.Write("Enter the file path: ");
            string customFilePath = Console.ReadLine();
            ProcessExcelFile(customFilePath, configurationService);
        }
        else if (fileOption < 1 || fileOption > excelFiles.Length)
        {
            Console.WriteLine("Invalid option. Please try again.");
            RegisterUsersFromExcel();
        }
        else
        {
            string selectedFilePath = excelFiles[fileOption - 1];
            ProcessExcelFile(selectedFilePath, configurationService);
        }
    }

    static void ProcessExcelFile(string filePath, IConfigurationService configurationService)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine("File not found. Please provide a valid file path.");
            return;
        }

        IPasswordService passwordService = new PasswordService();
        string password = passwordService.PromptForPassword("Enter the password to use for registration:");

        IExcelReaderService excelReader = new ExcelReaderService(configurationService);
        List<UserData> userDataList = excelReader.ReadUserData(filePath);

        IWebDriverService webDriverService = new WebDriverService();
        IUserManagementService registrationService = new UserManagementService(webDriverService, configurationService);

        bool allUsersRegistered = registrationService.RegisterUsers(userDataList, password);
        Console.WriteLine(allUsersRegistered ? "All users have been registered successfully." : "Registration process encountered errors.");
    }

    static void RemoveUsers()
    {
        // TODO: Implement the logic to remove users
        Console.WriteLine("Remove Users method is not implemented yet.");
    }

    static void UpdateUsers()
    {
        // TODO: Implement the logic to update users
        Console.WriteLine("Update Users method is not implemented yet.");
    }
}
