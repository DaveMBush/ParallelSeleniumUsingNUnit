using System;
using OpenQA.Selenium.Remote;

namespace ParallelSeleniumUsingNUnit.Common
{
    class FirefoxGrid : RemoteWebDriver
    {
        public FirefoxGrid() : base(new Uri("http://localhost:4444/wd/hub"), DesiredCapabilities.Firefox()) { }
    }
}
