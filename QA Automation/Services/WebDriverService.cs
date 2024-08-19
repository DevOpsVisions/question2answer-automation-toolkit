using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using QA_Automation.Interfaces;

namespace QA_Automation.Services
{
    // Class responsible for creating instances of WebDriver (currently using ChromeDriver)
    public class WebDriverService : IWebDriverService
    {
        /// <summary>
        /// Creates and returns an instance of Chrome WebDriver for automated browser actions.
        /// </summary>
        /// <returns>An instance of IWebDriver using ChromeDriver.</returns>
        public IWebDriver CreateWebDriver()
        {
            return new ChromeDriver();
        }
    }
}
