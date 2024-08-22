using OpenQA.Selenium;
using Dovs.Q2AAutoKit.Common;
using Dovs.Q2AAutoKit.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Dovs.Q2AAutoKit.Services
{
    // Class responsible for user registration process
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IWebDriverService _webDriverService;

        /// <summary>
        /// Constructor to inject WebDriver service dependency.
        /// </summary>
        /// <param name="webDriverService">The service responsible for providing WebDriver instances.</param>
        public UserRegistrationService(IWebDriverService webDriverService)
        {
            _webDriverService = webDriverService;
        }

        /// <summary>
        /// Registers a list of users by automating the registration process in a browser.
        /// </summary>
        /// <param name="users">List of users containing usernames and emails to register.</param>
        /// <param name="password">Password to use for all user registrations.</param>
        /// <returns>True if all users are registered successfully, false otherwise.</returns>
        public bool RegisterUsers(List<UserData> users, string password)
        {
            using (var driver = _webDriverService.CreateWebDriver())
            {
                string registrationUrl = ConfigurationManager.AppSettings.Get("RegistrationUrl");

                // Loop through each user and perform the registration process
                foreach (var user in users)
                {
                    Console.WriteLine($"Registering user: {user.UserName}, {user.Email}");
                    driver.Navigate().GoToUrl(registrationUrl);

                    FillRegistrationForm(driver, user, password);
                    ClickRegisterButton(driver);
                    System.Threading.Thread.Sleep(1000); // Wait for 1 second to allow form processing

                    Logout(driver);

                    // Verify if the registration form is still present, indicating a failed registration
                    if (!IsRegistrationFormPresent(driver))
                    {
                        Console.WriteLine("User registered successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to register user.");
                        return false; // Early exit on failure
                    }
                }

                return true; // Return true if all users are registered successfully
            }
        }

        /// <summary>
        /// Fills the registration form with user data.
        /// </summary>
        /// <param name="driver">Instance of the WebDriver controlling the browser.</param>
        /// <param name="userData">The user data (username, email) to fill the form.</param>
        /// <param name="password">The password to be used for registration.</param>
        private void FillRegistrationForm(IWebDriver driver, UserData userData, string password)
        {
            driver.FindElement(By.Id(ElementIds.HANDLE)).SendKeys(userData.UserName);
            driver.FindElement(By.Id(ElementIds.PASSWORD)).SendKeys(password);
            driver.FindElement(By.Id(ElementIds.EMAIL)).SendKeys(userData.Email);
        }

        /// <summary>
        /// Clicks the registration button on the form.
        /// </summary>
        /// <param name="driver">Instance of the WebDriver controlling the browser.</param>
        private void ClickRegisterButton(IWebDriver driver)
        {
            try
            {
                var registerButton = driver.FindElement(By.CssSelector(ElementIds.SELECTOR_REGISTER));
                registerButton.Click();
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Registration button not found; form may have already been submitted.");
            }
        }

        /// <summary>
        /// Logs out the user after registration is complete.
        /// </summary>
        /// <param name="driver">Instance of the WebDriver controlling the browser.</param>
        private void Logout(IWebDriver driver)
        {
            string logoutUrl = ConfigurationManager.AppSettings.Get("LogoutUrl");
            driver.Navigate().GoToUrl(logoutUrl);
        }

        /// <summary>
        /// Checks whether the registration form is still present after an attempt to register.
        /// </summary>
        /// <param name="driver">Instance of the WebDriver controlling the browser.</param>
        /// <returns>True if the registration form is still present, false otherwise.</returns>
        private bool IsRegistrationFormPresent(IWebDriver driver)
        {
            try
            {
                driver.FindElement(By.Id(ElementIds.HANDLE)); // Check for the presence of the 'handle' field in the form
                return true;
            }
            catch (NoSuchElementException)
            {
                return false; // Return false if the form is not present, indicating successful registration
            }
        }
    }
}
