// See https://aka.ms/new-console-template for more information
using Dovs.Q2AAutoKit;
using Dovs.Q2AAutoKit.Common;
using Dovs.Q2AAutoKit.Interfaces;
using Dovs.Q2AAutoKit.Services;

// Define the number of directory levels to traverse from the current directory.
// This constant can be adjusted based on how many levels up the base path should be located.
const int LEVELSTRAVERSE = 4;

// Retrieve the base path by traversing up the specified number of levels from the current directory.
// The GetBasePath method will handle the logic for traversing upwards and returning the correct directory.
string basePath = GetBasePath(LEVELSTRAVERSE);

// Combine the base path with the filename "default.xlsx" to create the full file path.
// Path.Combine ensures that the appropriate directory separator is used for the current operating system.
string defaultFilePath = Path.Combine(basePath, "default.xlsx");

// Output the current default file path to the console so the user is aware of where the file is expected to be located.
Console.WriteLine("Current default path is: " + defaultFilePath);

// Get the user-provided or default file path for the Excel file
string filePath = GetFilePath(defaultFilePath);
if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
{
    Console.WriteLine("File not found. Please provide a valid file path.");
    return; // Exit the program if no valid file path is provided
}

// Prompt the user to enter a password for registration
Console.WriteLine("Enter the password to use for registration:");
IPasswordService passwordService = new PasswordService();
string password = passwordService.ReadPassword();

// Read user data from the Excel file
IExcelReader excelReader = new ExcelReader();
List<UserData> userDataList = excelReader.ReadUserData(filePath);

// Inject dependencies and initiate the registration process
IWebDriverService webDriverService = new WebDriverService();
IUserRegistrationService registrationService = new UserRegistrationService(webDriverService);

// Perform user registration and output the result
bool allUsersRegistered = registrationService.RegisterUsers(userDataList, password);
Console.WriteLine(allUsersRegistered ? "All users have been registered successfully." : "Registration process encountered errors.");


static string GetBasePath(int levelsToTraverse)
{
    // Validate the number of levels to ensure it's a positive number
    if (levelsToTraverse < 1)
    {
        throw new ArgumentOutOfRangeException(nameof(levelsToTraverse), "Number of levels to traverse must be greater than zero.");
    }

    // Start from the current directory
    string currentDir = Directory.GetCurrentDirectory();

    try
    {
        // Traverse upwards the specified number of levels
        for (int i = 0; i < levelsToTraverse; i++)
        {
            DirectoryInfo parentDir = Directory.GetParent(currentDir);

            // If we reach the root directory, break early
            if (parentDir == null)
            {
                Console.WriteLine("Reached the root directory.");
                break;
            }

            currentDir = parentDir.FullName;
        }
    }
    catch (Exception ex)
    {
        // Handle potential errors, such as permission issues or other IO-related exceptions
        Console.WriteLine($"An error occurred while traversing directories: {ex.Message}");
    }

    return currentDir;
}

/// <summary>
/// Retrieves the file path based on the user's input. The user is prompted to choose 
/// between using the default file path or entering a custom path. The function will 
/// continue to prompt the user until valid input is provided.
/// </summary>
/// <param name="defaultFilePath">The default file path to be used if the user selects the default option.</param>
/// <returns>
/// The file path chosen by the user. This will either be the default file path or a custom 
/// file path provided by the user.
/// </returns>
static string GetFilePath(string defaultFilePath)
{
    // Display file path options based on whether the default file exists or not
    PrintFilePathOptions(defaultFilePath);

    // Continuously prompt the user until they provide a valid choice
    EUserChoice choice = GetValidUserChoice();

    // Handle the user's choice and return the corresponding file path (default or custom)
    return HandleUserChoice(choice, defaultFilePath);
}


/// <summary>
/// Prints the appropriate file path options based on whether the default file exists.
/// </summary>
/// <param name="defaultFilePath">The default file path.</param>
static void PrintFilePathOptions(string defaultFilePath)
{
    if (File.Exists(defaultFilePath))
    {
        Console.WriteLine("File Found: Press \n1 to use the default path \n2 to enter your own path:");
    }
    else
    {
        Console.WriteLine($"File not found: Ensure it's copied to {defaultFilePath}. Press \n1 to use the default path \n2 to enter your own path:");
    }
}

/// <summary>
/// Continuously prompts the user until a valid choice (1 or 2) is entered.
/// </summary>
/// <returns>The user's valid choice as an EUserChoice enum.</returns>
static EUserChoice GetValidUserChoice()
{
    while (true) // Loop until valid input is received
    {
        string input = Console.ReadLine();

        if (Enum.TryParse(input, out EUserChoice choice) && Enum.IsDefined(typeof(EUserChoice), choice))
        {
            return choice;
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter 1 for default path or 2 to enter a custom path.");
        }
    }
}

/// <summary>
/// Handles the user's valid choice and returns the appropriate file path.
/// </summary>
/// <param name="choice">The user's choice.</param>
/// <param name="defaultFilePath">The default file path to return if chosen.</param>
/// <returns>The appropriate file path based on the user's choice.</returns>
static string HandleUserChoice(EUserChoice choice, string defaultFilePath)
{
    switch (choice)
    {
        case EUserChoice.DefaultPath:
            return defaultFilePath;

        case EUserChoice.CustomPath:
            Console.WriteLine("Please enter your own path:");
            return Console.ReadLine();

        default:
            // This default case is unlikely to be reached, but we include it for safety.
            Console.WriteLine("Unexpected error. Returning null.");
            return null;
    }
}