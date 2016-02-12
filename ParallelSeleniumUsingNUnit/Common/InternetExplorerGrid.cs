using System;
using OpenQA.Selenium.Remote;

namespace ParallelSeleniumUsingNUnit.Common
{
    class InternetExplorerGrid : RemoteWebDriver
    {
        public InternetExplorerGrid() : base(new Uri("http://localhost:4444/wd/hub"), DesiredCapabilities.InternetExplorer()) { }
    }
}
