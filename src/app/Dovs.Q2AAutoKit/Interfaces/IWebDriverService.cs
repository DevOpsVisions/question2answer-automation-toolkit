using OpenQA.Selenium;

namespace Dovs.Q2AAutoKit.Interfaces
{
    // Interface to abstract WebDriver creation to allow flexibility in using different browsers
    public interface IWebDriverService
    {
        IWebDriver CreateWebDriver();
        void QuitWebDriver(IWebDriver driver);
    }
}
