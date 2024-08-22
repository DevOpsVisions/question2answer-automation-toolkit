using Dovs.Q2AAutoKit.Interfaces;
using System;

namespace Dovs.Q2AAutoKit.Services
{
    public class PasswordService : IPasswordService
    {
        /// <summary>
        /// Reads a password from the console without echoing the characters.
        /// </summary>
        /// <returns>The password entered by the user.</returns>
        public string ReadPassword()
        {
            string password = string.Empty;
            ConsoleKeyInfo key;

            // Loop to read each character entered by the user
            do
            {
                key = Console.ReadKey(intercept: true); // Read key without displaying it

                // Handle backspace to allow for correction of the password
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b"); // Remove last character from console
                }
                else if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    // Append the character to the password and mask it with '*'
                    password += key.KeyChar;
                    Console.Write("*");
                }
            }
            while (key.Key != ConsoleKey.Enter); // Stop when the user presses Enter

            Console.WriteLine(); // Move to the next line after password input is complete
            return password; // Return the password entered by the user
        }
    }

}
