using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Medicalgorithmics
{
    class MainPage
    {

        public string expectedURLAddress = "https://www.medicalgorithmics.pl/";
        private Boolean loadingError = false;

        public IWebElement contactButton;
        protected IWebElement acceptCookiesButton;
        protected IWebElement searchEngineButton;
        protected IWebElement searchEngineTextField;
        protected IWebElement searchEngingeSubmit;

        public void ContactClick()
        {
            contactButton.Click();
        }

        public string GetContactColor()
        {
            return contactButton.GetCssValue("color");
        }

        public void ContactGoTo(IWebDriver driver)
        {
            Actions builder = new Actions(driver);
            builder.MoveToElement(contactButton).Perform();
        }

        public void AcceptCookies()
        {
            acceptCookiesButton.Click();
        }

        public void SearchButtonClick()
        {
            searchEngineButton.Click();
        }

        public void SearchFieldFill(string search)
        {
            searchEngineTextField.SendKeys(search);
            searchEngineTextField.Submit();
        }

        public Boolean GetLoadingError()
        {
            return loadingError;
        }

        public Boolean IsSubmitVisible()
        {
            string ariaHidden = searchEngingeSubmit.GetAttribute("aria-hidden");

            if(ariaHidden == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public MainPage(IWebDriver driver)
        {

            if(driver.Url == expectedURLAddress)
            {
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
                wait.Until(Driver => driver.FindElement(By.Id("mega-menu-item-29")));

                contactButton = driver.FindElement(By.CssSelector("#mega-menu-item-29 > a"));
                acceptCookiesButton = driver.FindElement(By.Id("cn-accept-cookie"));

                searchEngineButton = driver.FindElement(By.XPath("//a[contains(@class, 'search_button')]"));
                searchEngineTextField = driver.FindElement(By.Name("s"));
                searchEngingeSubmit = driver.FindElement(By.XPath("//*[contains(@class, 'qode_icon_font_elegant arrow_right qode_icon_element')]"));
            }
            else
            {
                loadingError = true;
            }

        }

    }
}
