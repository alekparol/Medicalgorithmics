using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medicalgorithmics
{
    class MediaPackPage
    {

        public string expectedURLAddress = "https://www.medicalgorithmics.pl/media-pack";
        private Boolean loadingError = false;

        protected IWebElement logotypyButton;

        public void DonwloadLogotypy()
        {
            logotypyButton.Click();
        }

        public Boolean GetLoadingError()
        {
            return loadingError;
        }

        public MediaPackPage(IWebDriver driver)
        {
            if (driver.Url == expectedURLAddress)
            {
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
                wait.Until(Driver => driver.FindElement(By.XPath("//strong[text()='Logotypy']")));

                logotypyButton = driver.FindElement(By.XPath("//strong[text()='Logotypy']"));

            }
            else
            {
                loadingError = true;
            }
        }

    }
}
