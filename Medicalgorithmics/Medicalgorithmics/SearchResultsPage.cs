using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medicalgorithmics
{
    class SearchResultsPage
    {

        public string expectedURLAddress = "https://www.medicalgorithmics.pl/?s=Pocket+ECG+CRS";
        public string expectedTitle = "Wyniki wyszukiwania \"Pocket ECG CRS\" - Medicalgorithmics";
        private Boolean loadingError = false;

        protected List<IWebElement> articleList;
        protected List<IWebElement> specificArticleList;
        protected IWebElement nextPageButton;

        public int CountSearchResults()
        {
            return articleList.Count;
        }

        public int CountSpecificArticleList()
        {
            return specificArticleList.Count;
        }

        public void GoToNextPageButton(IWebDriver driver)
        {
            Actions builder = new Actions(driver);
            builder.MoveToElement(nextPageButton).Perform();
        }

        public void NextPageButtonClick()
        {
            nextPageButton.Click();
        }

        public Boolean GetLoadingError()
        {
            return loadingError;
        }

        public SearchResultsPage(IWebDriver driver)
        {
            if (driver.Url == expectedURLAddress)
            {
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
                wait.Until(Driver => driver.FindElement(By.XPath("//strong[text()='Logotypy']")));

                articleList = new List<IWebElement>(driver.FindElements(By.ClassName("latest_post_custom")));
                specificArticleList = new List<IWebElement>();

                foreach(IWebElement el in articleList)
                {
                    List<IWebElement> newList = new List<IWebElement>(el.FindElements(By.XPath("descendant::a[contains(.,\"PocketECG CRS – telerehabilitacja kardiologiczna\")]")));
                    if(newList.Count == 1)
                    {
                        specificArticleList.Add(newList[0]);
                    }
                }

                nextPageButton = driver.FindElement(By.XPath("//i[contains(@class, 'fa fa-angle-right')]"));

            }
            else
            {
                loadingError = true;
            }
        }

    }
}
