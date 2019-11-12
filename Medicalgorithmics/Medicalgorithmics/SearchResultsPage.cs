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
        public string expectedURLAddressSecondPage = "https://www.medicalgorithmics.pl/page/2?s=Pocket+ECG+CRS";

        public string expectedTitle = "Wyniki wyszukiwania \"Pocket ECG CRS\" - Medicalgorithmics";
        private Boolean loadingError = false;

        protected List<IWebElement> articleList;
        protected List<IWebElement> specificArticleList;
        protected IWebElement nextPageButton;

        public int CountSearchResults()
        {
            return articleList.Count;
        }

        public List<IWebElement> GetSearchResults()
        {
            return articleList;
        }

        public int CountSpecificArticleList()
        {
            return specificArticleList.Count;
        }

        public void GoToNextPageButton(IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", nextPageButton);
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
            if (driver.Url == expectedURLAddress || driver.Url == expectedURLAddressSecondPage)
            {
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
                wait.Until(Driver => driver.FindElement(By.ClassName("latest_post_custom")));

                articleList = new List<IWebElement>(driver.FindElements(By.ClassName("latest_post_custom")));

                
                // This condition is created in order to initialize 'specificArticleList' only on the first page of the 
                // search results. 

                if(driver.Title == expectedTitle)
                {
                    specificArticleList = new List<IWebElement>();

                    foreach (IWebElement el in articleList)
                    {
                        List<IWebElement> newList = new List<IWebElement>(el.FindElements(By.XPath("descendant::a[contains(.,\"PocketECG CRS – telerehabilitacja kardiologiczna\")]")));
                        if (newList.Count == 1)
                        {
                            specificArticleList.Add(newList[0]);
                        }
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
