using Dovs.Q2AAutoKit;
using Dovs.Q2AAutoKit.Common;
using Dovs.Q2AAutoKit.Interfaces;
using Dovs.Q2AAutoKit.Services;
using System;
using System.Collections.Generic;
using System.IO;

const int LEVELSTRAVERSE = 2;

IFilePathService filePathService = new FilePathService();
IConfigurationService configurationService = new ConfigurationService(); // Assuming you have a ConfigurationService implementation

string basePath = filePathService.GetBasePath(LEVELSTRAVERSE);
string defaultFilePath = Path.Combine(basePath, "default.xlsx");

Console.WriteLine("Current default path is: " + defaultFilePath);

string filePath = filePathService.GetFilePath(defaultFilePath);
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
