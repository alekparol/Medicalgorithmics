using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medicalgorithmics
{
    class MainPage
    {

        public string URLAddress = "https://www.medicalgorithmics.pl/";

        public IWebElement kontaktButton;

        void KontaktClick()
        {
            kontaktButton.Click();
        }

        public MainPage(IWebDriver driver)
        {

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            wait.Until(Driver => driver.FindElement(By.Id("mega-menu-item-29")));

            kontaktButton = driver.FindElement(By.Id("mega-menu-item-29"));
        
        }

    }
}
