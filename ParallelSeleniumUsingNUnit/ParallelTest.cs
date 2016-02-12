using System.Collections.Concurrent;
using System.Threading.Tasks;
using NUnit.Framework;
using ParallelSeleniumUsingNUnit.Common;
using ParallelSeleniumUsingNUnit.PageModels;

namespace ParallelSeleniumUsingNUnit
{
    [TestFixture]
    class ParallelGrid
    {
        private IGooglePageModel _page;
        [SetUp]
        public void Setup()
        {
            var pages = new ConcurrentStack<IGooglePageModel>();
            Parallel.Invoke(() => pages.Push(new GooglePageModel<InternetExplorerGrid>()),
                () => pages.Push(new GooglePageModel<FirefoxGrid>()));
            var parallelPage = new ParallelPageModel<IGooglePageModel>(pages.ToArray());
            _page = parallelPage.Cast();
            _page.Search("SQL For .NET Programmers");
        }

        [TearDown]
        public void TearDown()
        {
            _page.Close();
        }

        [Test]
        public void SomeTest()
        {
            Assert.That(_page.PageContains("Bush"));
        }
    }
}
