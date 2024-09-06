using Dovs.FileSystemInteractor.Interfaces;
using Dovs.FileSystemInteractor.Services;
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
                RegisterUsers();
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

    static void RegisterUsers()
    {
        const int LEVELSTRAVERSE = 2;

        IFilePathService filePathService = new FilePathService();
        IConfigurationService configurationService = new ConfigurationService();
        IFileInteractionService fileInteractionService = new FileInteractionService();

        string filePath = fileInteractionService.SelectFilePath(filePathService, LEVELSTRAVERSE);

        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("File not found. Please provide a valid file path.");
            Console.ResetColor();
            return;
        }

        IPasswordService passwordService = new PasswordService();
        string password = passwordService.PromptForPassword("Enter the password to use for registration:");

        IExcelReaderService excelReader = new ExcelReaderService(configurationService);
        List<UserData> userDataList = excelReader.ReadUserData(filePath);

        IWebDriverService webDriverService = new WebDriverService();
        IUserManagementService registrationService = new UserManagementService(webDriverService, configurationService);

        bool allUsersRegistered = registrationService.RegisterUsers(userDataList, password);

        if (allUsersRegistered)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("All users have been registered successfully.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Registration process encountered errors.");
        }
        Console.ResetColor();
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
