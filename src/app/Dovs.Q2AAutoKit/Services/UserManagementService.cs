using OpenQA.Selenium;
using Dovs.Q2AAutoKit.Common;
using Dovs.Q2AAutoKit.Interfaces;
using System;
using System.Collections.Generic;

namespace Dovs.Q2AAutoKit.Services
{
    // Class responsible for user registration process
    public class UserManagementService : IUserManagementService
    {
        private readonly IWebDriverService _webDriverService;
        private readonly IConfigurationService _configurationService;

        public UserManagementService(IWebDriverService webDriverService, IConfigurationService configurationService)
        {
            _webDriverService = webDriverService;
            _configurationService = configurationService;
        }

        public bool RegisterUsers(List<UserData> users, string password)
        {
            using (var driver = _webDriverService.CreateWebDriver())
            {
                string registrationUrl = _configurationService.GetConfigValue("RegistrationUrl");
                bool isFirstIteration = true;

                foreach (var user in users)
                {
                    Console.WriteLine($"Registering user: {user.UserName}, {user.Email}");
                    driver.Navigate().GoToUrl(registrationUrl);

                    FillRegistrationForm(driver, user, password);
                    ClickRegisterButton(driver);
                    System.Threading.Thread.Sleep(1000);

                    if (isFirstIteration && IsErrorElementPresent(driver))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error detected, retrying registration...");
                        Console.ResetColor();
                        ClickRegisterButton(driver);
                        System.Threading.Thread.Sleep(1000);
                        isFirstIteration = false; // Set the flag to false after the first iteration
                    }

                    Logout(driver);

                    if (!IsRegistrationFormPresent(driver))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("User registered successfully.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Failed to register user.");
                        Console.ResetColor();
                        return false;
                    }
                }

                return true;
            }
        }

        private bool IsErrorElementPresent(IWebDriver driver)
        {
            return ElementExists(driver, By.CssSelector(ElementIds.ERROR));
        }

        private void FillRegistrationForm(IWebDriver driver, UserData userData, string password)
        {
            driver.FindElement(By.Id(ElementIds.HANDLE)).SendKeys(userData.UserName);
            driver.FindElement(By.Id(ElementIds.PASSWORD)).SendKeys(password);
            driver.FindElement(By.Id(ElementIds.EMAIL)).SendKeys(userData.Email);
        }

        private void ClickRegisterButton(IWebDriver driver)
        {
            ClickElement(driver, By.CssSelector(ElementIds.SELECTOR_REGISTER), "Registration button not found; form may have already been submitted.");
        }

        private void Logout(IWebDriver driver)
        {
            string logoutUrl = _configurationService.GetConfigValue("LogoutUrl");
            driver.Navigate().GoToUrl(logoutUrl);
        }

        private bool IsRegistrationFormPresent(IWebDriver driver)
        {
            return ElementExists(driver, By.Id(ElementIds.HANDLE));
        }

        private bool ElementExists(IWebDriver driver, By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private void ClickElement(IWebDriver driver, By by, string errorMessage)
        {
            try
            {
                driver.FindElement(by).Click();
            }
            catch (NoSuchElementException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(errorMessage);
                Console.ResetColor();
            }
        }
    }
}
