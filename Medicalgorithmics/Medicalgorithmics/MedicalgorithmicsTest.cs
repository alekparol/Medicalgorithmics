using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
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

            var profileManager = new FirefoxProfileManager();
            FirefoxProfile profile = profileManager.GetProfile("Selenium");

            FirefoxProfile profile2 = new FirefoxProfile();
            // profile.setPreference("browser.download.useDownloadDir", true); This is true by default. Add it if it's not working without it.

            FirefoxOptions option = new FirefoxOptions();
            option.SetPreference("browser.download.folderList", 2);
            option.SetPreference("browser.download.dir", downloadDirectory); //Set the last directory used for saving a file from the "What should (browser) do with this file?" dialog.
            option.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf"); //list of MIME types to save to disk without asking what to use to open the file
            option.SetPreference("pdfjs.disabled", true);  // disable the built-in PDF viewer
            option.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/octet-stream doc xls pdf txt");
            //firefoxOptions.AddAdditionalCapability("browser.download.dir", downloadDirectory);

            /* Initializing firefoxdriver */

            IWebDriver driver = new FirefoxDriver(option);
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://www.medicalgorithmics.pl/");

            /* Initializing instance of a MainPage object which represent main page of Medicalgorithmics site */

            MainPage mainPage = new MainPage(driver);
            Assert.False(mainPage.GetLoadingError());

            mainPage.AcceptCookies();

            string buttonColorBeforeFocusing = mainPage.GetContactColor();
            mainPage.ContactGoTo(driver);

            /* Test for changing color of the "Kontakt" button */

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
            MainPage mp = new MainPage(driver);

            Thread.Sleep(3000);

            Assert.False(mp.GetLoadingError());
            mp.AcceptCookies();

            mp.SearchButtonClick();

            Thread.Sleep(3000);

            Assert.True(mp.IsSubmitVisible());
            mp.SearchFieldFill("Pocket ECG CRS");
           
            Thread.Sleep(3000);

            SearchResultsPage sr = new SearchResultsPage(driver);

            string expectedURL = "https://www.medicalgorithmics.pl/?s=Pocket+ECG+CRS";
            string expectedTitle = "Wyniki wyszukiwania \"Pocket ECG CRS\" - Medicalgorithmics";

            Assert.Equal(sr.expectedURLAddress, driver.Url);
            Assert.Equal(sr.expectedTitle, driver.Title);

            Assert.Equal(10, sr.CountSearchResults());
            Assert.Equal(1, sr.CountSpecificArticleList());

            sr.GoToNextPageButton(driver);
            sr.NextPageButtonClick();

            Assert.NotEqual(expectedURL, driver.Url);

            SearchResultsPage sr2 = new SearchResultsPage(driver);
            Assert.False(sr.CountSearchResults() == sr2.CountSearchResults());

            driver.Close();
        }
    }
}
