using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medicalgorithmics
{
    class ContactPage
    {

        public string expectedURLAddress = "https://www.medicalgorithmics.pl/kontakt";
        private Boolean loadingError = false;

        private IWebElement mediaPackButton;

        public void MediaPackClick()
        {
            mediaPackButton.Click();
        }

        public void ScrollToTheMediaPack(IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", mediaPackButton);
        }

        public Boolean GetLoadingError()
        {
            return loadingError;
        }

        public ContactPage(IWebDriver driver)
        {
            if (driver.Url == expectedURLAddress)
            {
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
                wait.Until(Driver => driver.FindElement(By.XPath("//a[text()='Media pack']")));

                mediaPackButton = driver.FindElement(By.XPath("//a[text()='Media pack']"));
               
            }
            else
            {
                loadingError = true;
            }
        }

    }
}
