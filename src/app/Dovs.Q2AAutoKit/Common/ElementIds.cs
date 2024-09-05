namespace Dovs.Q2AAutoKit.Common
{
    /// <summary>
    /// The ElementIds class serves as a centralized location for storing commonly used HTML element IDs
    /// and CSS selectors within the application. By using constants for these values, we can avoid hardcoding
    /// strings directly in the code, improving maintainability and reducing errors.
    /// </summary>
    public class ElementIds
    {
        /// <summary>
        /// The HTML element ID for the username (handle) field in the registration form.
        /// </summary>
        public const string HANDLE = "handle";

        /// <summary>
        /// The HTML element ID for the password field in the registration form.
        /// </summary>
        public const string PASSWORD = "password";

        /// <summary>
        /// The HTML element ID for the email field in the registration form.
        /// </summary>
        public const string EMAIL = "email";

        /// <summary>
        /// The CSS selector used to locate the registration button in the form.
        /// This selector targets the button with the specific value attribute of 'Register'.
        /// </summary>
        public const string SELECTOR_REGISTER = ".qa-form-tall-button-register[value='Register']";

        /// <summary>
        /// The CSS selector used to locate the error element in the form.
        /// This selector targets elements with the class 'qa-error'.
        /// </summary>
        public const string ERROR = ".qa-error";
    }

}
