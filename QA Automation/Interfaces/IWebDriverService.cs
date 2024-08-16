using OpenQA.Selenium;

namespace QA_Automation.Interfaces
{
    // Interface to abstract WebDriver creation to allow flexibility in using different browsers
    public interface IWebDriverService
    {
        IWebDriver CreateWebDriver();
    }
}
