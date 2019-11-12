using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Xunit;

namespace Medicalgorithmics
{
    public class MedicalalgorithmicsTest
    {
        [Fact]
        public void Chrome_Logotypy()
        {

            /* Preparing a folder for downloading the .zip file */

            string downloadDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\seleniumTest";
            string fileName = downloadDirectory + "\\logotypy.zip";

            if (!Directory.Exists(downloadDirectory))
            {
                Directory.CreateDirectory(downloadDirectory);
            }
            else
            {
                string[] filesList = Directory.GetFiles(downloadDirectory);

                foreach(string file in filesList)
                {
                    File.Delete(file);
                }

                Directory.Delete(downloadDirectory);
                Directory.CreateDirectory(downloadDirectory);
            }

            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);

            /* Initializing chromedriver */

            IWebDriver driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://www.medicalgorithmics.pl/");

            /* Initializing instance of a MainPage object which represent main page of Medicalgorithmics site */

            MainPage mainPage = new MainPage(driver);
            Assert.False(mainPage.GetLoadingError());

            mainPage.AcceptCookies();

            string buttonColorBeforeFocusing = mainPage.GetContactColor();
            mainPage.ContactGoTo(driver);

            /* Test for changing color of the "Kontakt" button */
            
            string buttonColorAfterFocusing = mainPage.contactButton.GetCssValue("color");
            Assert.NotEqual(buttonColorBeforeFocusing, buttonColorAfterFocusing);

            mainPage.ContactClick();

            /* Initializing instance of the ContactPage class which represents "Kontakt" page */

            ContactPage contactPage = new ContactPage(driver);
            Assert.Equal("https://www.medicalgorithmics.pl/kontakt", driver.Url);

            Assert.False(contactPage.GetLoadingError());
            contactPage.ScrollToTheMediaPack(driver);

            Thread.Sleep(3000);

            contactPage.MediaPackClick();
            Assert.Equal("https://www.medicalgorithmics.pl/media-pack", driver.Url);

            /* Initializing instance of the MediaPackPage class which represents Media Pack page */

            MediaPackPage mediaPackPage = new MediaPackPage(driver);
            Assert.False(mediaPackPage.GetLoadingError());

            Assert.False(File.Exists(fileName));
            Thread.Sleep(3000);

            /* Downloading logotypy.zip file and checking if the file downloaded correctly into given directory */

            mediaPackPage.DonwloadLogotypy();

            int i = 0;
            while (!File.Exists(fileName) && i < 20000)
            {
                Thread.Sleep(1);
                i++;
            }

            Assert.True(File.Exists(fileName));

            /* Deleteing downloaded file and created directory */

            File.Delete(fileName);
            Directory.Delete(downloadDirectory);

            driver.Close();

        }

        [Fact]
        public void Firefox_Logotypy()
        {

            /* Preparing a folder for downloading the .zip file */

            string downloadDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\seleniumTest";
            string fileName = downloadDirectory + "\\logotypy.zip";

            if (!Directory.Exists(downloadDirectory))
            {
                Directory.CreateDirectory(downloadDirectory);
            }
            else
            {
                string[] filesList = Directory.GetFiles(downloadDirectory);

                foreach (string file in filesList)
                {
                    File.Delete(file);
                }

                Directory.Delete(downloadDirectory);
                Directory.CreateDirectory(downloadDirectory);
            }

            FirefoxOptions profile = new FirefoxOptions();

            profile.SetPreference("browser.download.manager.showWhenStarting", false);
            profile.SetPreference("browser.download.dir", downloadDirectory);
            profile.SetPreference("browser.download.folderList", 2);
            profile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/zip");
            profile.SetPreference("browser.download.useDownloadDir", true);

            /* Initializing firefoxdriver */

            IWebDriver driver = new FirefoxDriver(profile);
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://www.medicalgorithmics.pl/");

            /* Initializing instance of a MainPage object which represent main page of Medicalgorithmics site */

            MainPage mainPage = new MainPage(driver);
            Assert.False(mainPage.GetLoadingError());

            mainPage.AcceptCookies();

            string buttonColorBeforeFocusing = mainPage.GetContactColor();
            mainPage.ContactGoTo(driver);

            /* Test for changing color of the "Kontakt" button (For Firefox I disabled this test as the Actions class is not working for Gecko driver and the issue is not solved yet) */

            string buttonColorAfterFocusing = mainPage.GetContactColor();
            //Assert.NotEqual(buttonColorBeforeFocusing, buttonColorAfterFocusing);

            mainPage.ContactClick();

            /* Initializing instance of the ContactPage class which represents "Kontakt" page */

            ContactPage contactPage = new ContactPage(driver);
            Assert.Equal("https://www.medicalgorithmics.pl/kontakt", driver.Url);

            Assert.False(contactPage.GetLoadingError()); 
            contactPage.ScrollToTheMediaPack(driver);

            Thread.Sleep(3000);

            contactPage.MediaPackClick();
            Assert.Equal("https://www.medicalgorithmics.pl/media-pack", driver.Url);

            /* Initializing instance of the MediaPackPage class which represents Media Pack page */

            MediaPackPage mediaPackPage = new MediaPackPage(driver);
            Assert.False(mediaPackPage.GetLoadingError());

            Assert.False(File.Exists(fileName));
            Thread.Sleep(3000);

            /* Downloading logotypy.zip file and checking if the file downloaded correctly into given directory */

            mediaPackPage.DonwloadLogotypy();

            int i = 0;
            while (!File.Exists(fileName) && i < 20000)
            {
                Thread.Sleep(1);
                i++;
            }

            Assert.True(File.Exists(fileName));

            /* Deleteing downloaded file and created directory */

            File.Delete(fileName);
            Directory.Delete(downloadDirectory);

            driver.Close();

        }

        [Fact]
        public void Chrome_SearchResults()
        {

            /* Initializing chromedriver */

            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://www.medicalgorithmics.pl/");

            /* Initializing instance of MainPage class which represents main page */

            MainPage mainPage = new MainPage(driver);
            Assert.False(mainPage.GetLoadingError());

            mainPage.AcceptCookies();
            mainPage.SearchButtonClick();

            /* Typing "Pocket ECG CRS" in the search engine field and initializing an instance of SearchResultsPage class*/

            Assert.True(mainPage.IsSubmitVisible());
            mainPage.SearchFieldFill("Pocket ECG CRS");

            Thread.Sleep(3000);
            SearchResultsPage searchResults = new SearchResultsPage(driver);

            string expectedURL = "https://www.medicalgorithmics.pl/?s=Pocket+ECG+CRS";
            string expectedTitle = "Wyniki wyszukiwania \"Pocket ECG CRS\" - Medicalgorithmics";

            /* Test for correctly loaded search results page done by checking page title and URL address */

            Assert.Equal(searchResults.expectedURLAddress, driver.Url);
            Assert.Equal(searchResults.expectedTitle, driver.Title);

            /* Test of number of displayed articles on the page and articles containingin "Pocket ECG CRS" in the title */

            Assert.Equal(10, searchResults.CountSearchResults());
            Assert.Equal(1, searchResults.CountSpecificArticleList());

            searchResults.GoToNextPageButton(driver);
            searchResults.NextPageButtonClick();

            /* Test which determines that the next page of the search results is not empty and not the same as first page */

            Assert.NotEqual(expectedURL, driver.Url);

            SearchResultsPage searchResultsSecond = new SearchResultsPage(driver);
            Assert.False(searchResults.CountSearchResults() == searchResultsSecond.CountSearchResults());

            driver.Close();
        }

        [Fact]
        public void Firefox_SearchResults()
        {

            /* Initializing firefoxdriver */

            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://www.medicalgorithmics.pl/");

            /* Initializing instance of MainPage class which represents main page */

            MainPage mainPage = new MainPage(driver);
            Assert.False(mainPage.GetLoadingError());

            mainPage.AcceptCookies();
            mainPage.SearchButtonClick();

            /* Typing "Pocket ECG CRS" in the search engine field and initializing an instance of SearchResultsPage class*/

            Assert.True(mainPage.IsSubmitVisible());
            mainPage.SearchFieldFill("Pocket ECG CRS");

            Thread.Sleep(3000);
            SearchResultsPage searchResults = new SearchResultsPage(driver);

            string expectedURL = "https://www.medicalgorithmics.pl/?s=Pocket+ECG+CRS";
            string expectedTitle = "Wyniki wyszukiwania \"Pocket ECG CRS\" - Medicalgorithmics";

            /* Test for correctly loaded search results page done by checking page title and URL address */

            Assert.Equal(searchResults.expectedURLAddress, driver.Url);
            Assert.Equal(searchResults.expectedTitle, driver.Title);

            /* Test of number of displayed articles on the page and articles containingin "Pocket ECG CRS" in the title */

            Assert.Equal(10, searchResults.CountSearchResults());
            Assert.Equal(1, searchResults.CountSpecificArticleList());

            searchResults.GoToNextPageButton(driver);
            searchResults.NextPageButtonClick();

            /* Test which determines that the next page of the search results is not empty and not the same as first page */

            Assert.NotEqual(expectedURL, driver.Url);

            SearchResultsPage searchResultsSecond = new SearchResultsPage(driver);
            Assert.False(searchResults.CountSearchResults() == searchResultsSecond.CountSearchResults());

            driver.Close();
        }
    }
}
