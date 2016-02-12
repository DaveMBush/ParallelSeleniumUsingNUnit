using System;
using JetBrains.Annotations;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace ParallelSeleniumUsingNUnit.PageModels
{
    class GooglePageModel<TDriver> : IGooglePageModel where TDriver : IWebDriver, new()
    {
        [FindsBy(How = How.Id, Using = "gbqfq")]
        private IWebElement SearchField { get; [UsedImplicitly] set; }

        private readonly IWebDriver _driver;
        private WebDriverWait Wait { get; set; }
        public GooglePageModel()
        {
            _driver = new TDriver();
            // any time we wait we will wait for 60 seconds.
            Wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 60));
            const string baseUrl = @"http://www.google.com";
            _driver.Navigate().GoToUrl(baseUrl);
            Wait.Until(x => this.SearchField.Displayed);
            PageFactory.InitElements(_driver, this);

        }

        public IGooglePageModel Search(string search)
        {
            SearchField.Clear();
            SearchField.SendKeys(search + Keys.Enter);
            return this;
        }

        public bool PageContains(string content)
        {
            try
            {
                Wait.Until(x => x.PageSource.Contains(content));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void Close()
        {
            _driver.Quit();
        }
    }
}
