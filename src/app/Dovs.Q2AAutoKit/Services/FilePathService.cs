using Dovs.Q2AAutoKit.Common;
using Dovs.Q2AAutoKit.Interfaces;
using System;
using System.IO;

namespace Dovs.Q2AAutoKit.Services
{
    public class FilePathService : IFilePathService
    {
        public string GetBasePath(int levelsToTraverse)
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

        public string GetFilePath(string defaultFilePath)
        {
            // Display file path options based on whether the default file exists
            PrintFilePathOptions(defaultFilePath);

            // Continuously prompt the user until they provide a valid choice
            EUserChoice choice = GetValidUserChoice();

            // Handle the user's choice and return the corresponding file path (default or custom)
            return HandleUserChoice(choice, defaultFilePath);
        }

        private void PrintFilePathOptions(string defaultFilePath)
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

        private EUserChoice GetValidUserChoice()
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

        private string HandleUserChoice(EUserChoice choice, string defaultFilePath)
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
    }
}
