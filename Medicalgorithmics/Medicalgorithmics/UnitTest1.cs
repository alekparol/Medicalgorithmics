using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Xunit;

namespace Medicalgorithmics
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

            string downloadDirectory = "C:\\Users\\Aleksander.Parol\\Downloads" + "\\seleniumTest";
            if(!Directory.Exists(downloadDirectory))
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

            IWebDriver driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://www.medicalgorithmics.pl/");

            MainPage mp = new MainPage(driver);
            Assert.False(mp.GetLoadingError());

            mp.AcceptCookies();

            string buttonColor = mp.GetContactColor();
            mp.ContactGoTo(driver);

            string buttonColor2 = mp.GetContactColor();
            mp.ContactClick();

            Assert.NotEqual(buttonColor, buttonColor2);

            ContactPage cp = new ContactPage(driver);
            Assert.Equal("https://www.medicalgorithmics.pl/kontakt", driver.Url);

            Assert.False(cp.GetLoadingError());
            cp.ScrollToTheMediaPack(driver);

            Thread.Sleep(3000);

            cp.MediaPackClick();
            Assert.Equal("https://www.medicalgorithmics.pl/media-pack", driver.Url);

            MediaPackPage mdp = new MediaPackPage(driver);
            Assert.False(mdp.GetLoadingError());

            string fileName = downloadDirectory + "\\logotypy.zip";
            Assert.False(File.Exists(fileName));

            Thread.Sleep(3000);

            mdp.DonwloadLogotypy();

            int i = 0;
            while (!File.Exists(fileName) && i < 20000)
            {
                Thread.Sleep(1);
                i++;
            }

            Assert.True(File.Exists(fileName));

            File.Delete(fileName);
            Directory.Delete(downloadDirectory);

            driver.Close();

        }

        [Fact]
        public void Test2()
        {

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
            

            //IWebElement searchSubmit = driver.FindElement(By.XPath("//*[contains(@class, 'qode_icon_font_elegant arrow_right qode_icon_element')]"));
            //Assert.Equal("true", searchSubmit.GetAttribute("aria-hidden"));
            //searchSubmit.Click();
            //searchTextField.Submit();

            Thread.Sleep(3000);

            string expectedURL = "https://www.medicalgorithmics.pl/?s=Pocket+ECG+CRS";
            string expectedTitle = "Wyniki wyszukiwania \"Pocket ECG CRS\" - Medicalgorithmics";

            Assert.Equal(expectedURL, driver.Url);
            Assert.Equal(expectedTitle, driver.Title);

            List<IWebElement> postsList = new List<IWebElement>(driver.FindElements(By.ClassName("latest_post_custom")));
            Assert.Equal(10, postsList.Count);

            List<IWebElement> newList = new List<IWebElement>(driver.FindElements(By.XPath("descendant::a[contains(.,\"PocketECG CRS – telerehabilitacja kardiologiczna\")]")));
            Assert.True(1 == newList.Count);

            IWebElement nextPageButton = driver.FindElement(By.CssSelector("body > div.wrapper > div > div > div > div.container > div.container_inner.default_template_holder.clearfix > div > ul > div.pagination > ul > li.next > a"));
            Actions builder = new Actions(driver);

            builder.MoveToElement(nextPageButton).Perform();
            nextPageButton.Click();

            Assert.NotEqual(expectedURL, driver.Url);

            List<IWebElement> listOfNewElements = new List<IWebElement>(driver.FindElements(By.ClassName("latest_post_custom")));
            Assert.False(postsList.Equals(listOfNewElements));


            driver.Close();



        }
    }
}
