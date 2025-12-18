using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace SelehiumTests
{
    public class Lab5Tests
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;
        protected string baseUrl = "https://the-internet.herokuapp.com/";

        [SetUp]
        public void Setup()
        {
            new DriverManager().SetUpDriver(new EdgeConfig());
            driver = new EdgeDriver();

            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void TestCheckboxes()
        {
            driver.Navigate().GoToUrl(baseUrl + "checkboxes");
            var checkboxes = driver.FindElements(By.CssSelector("input[type='checkbox']"));
            Assert.That(checkboxes.Count, Is.EqualTo(2));

            if (!checkboxes[0].Selected)
                checkboxes[0].Click();

            Assert.That(checkboxes[0].Selected, Is.True);
        }

        [Test]
        public void TestAddRemoveElements()
        {
            driver.Navigate().GoToUrl(baseUrl + "add_remove_elements/");
            var addButton = driver.FindElement(By.XPath("//button[text()='Add Element']"));
            addButton.Click();

            var deleteButtons = driver.FindElements(By.CssSelector(".added-manually"));
            Assert.That(deleteButtons.Count, Is.EqualTo(1));

            deleteButtons[0].Click();
            deleteButtons = driver.FindElements(By.CssSelector(".added-manually"));
            Assert.That(deleteButtons.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestDropdown()
        {
            driver.Navigate().GoToUrl(baseUrl + "dropdown");
            var dropdown = new SelectElement(driver.FindElement(By.Id("dropdown")));
            dropdown.SelectByText("Option 2");
            Assert.That(dropdown.SelectedOption.Text, Is.EqualTo("Option 2"));
        }

        [Test]
        public void TestInputs()
        {
            driver.Navigate().GoToUrl(baseUrl + "inputs");
            var input = driver.FindElement(By.TagName("input"));
            input.Clear();
            input.SendKeys("123");
            Assert.That(input.GetAttribute("value"), Is.EqualTo("123"));
        }

        [Test]
        public void TestStatusCodes()
        {
            driver.Navigate().GoToUrl(baseUrl + "status_codes");
            driver.FindElement(By.LinkText("200")).Click();
            var message = driver.FindElement(By.TagName("p")).Text;
            Assert.That(message, Does.Contain("This page returned a 200 status code"));
        }

        [Test]
        public void TestNestedFrames()
        {
            driver.Navigate().GoToUrl(baseUrl + "nested_frames");
            driver.SwitchTo().Frame("frame-top");
            driver.SwitchTo().Frame("frame-left");
            var bodyText = driver.FindElement(By.TagName("body")).Text;
            Assert.That(bodyText, Is.EqualTo("LEFT"));
            driver.SwitchTo().DefaultContent();
        }

        [Test]
        public void TestFormAuthentication()
        {
            driver.Navigate().GoToUrl(baseUrl + "login");
            driver.FindElement(By.Id("username")).SendKeys("tomsmith");
            driver.FindElement(By.Id("password")).SendKeys("SuperSecretPassword!");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            var flash = driver.FindElement(By.Id("flash")).Text;
            Assert.That(flash, Does.Contain("You logged into a secure area!"));
        }

        [Test]
        public void TestDynamicContent()
        {
            driver.Navigate().GoToUrl(baseUrl + "dynamic_content");
            var contentBefore = driver.FindElements(By.CssSelector(".large-10.columns")).Select(e => e.Text).ToList();
            driver.Navigate().Refresh();
            var contentAfter = driver.FindElements(By.CssSelector(".large-10.columns")).Select(e => e.Text).ToList();
            Assert.That(contentBefore, Is.Not.EqualTo(contentAfter));
        }

        [Test]
        public void TestDisappearingElements()
        {
            driver.Navigate().GoToUrl(baseUrl + "disappearing_elements");
            var menuItems = driver.FindElements(By.CssSelector("ul li")).Select(e => e.Text).ToList();
            Assert.That(menuItems.Count, Is.GreaterThanOrEqualTo(4));
        }

        [Test]
        public void TestHover()
        {
            driver.Navigate().GoToUrl(baseUrl + "hovers");
            var figures = driver.FindElements(By.ClassName("figure"));
            var caption = figures[0].FindElement(By.ClassName("figcaption"));
            Assert.That(caption.Displayed, Is.False);

            var action = new OpenQA.Selenium.Interactions.Actions(driver);
            action.MoveToElement(figures[0]).Perform();

            Assert.That(caption.Displayed, Is.True);
            Assert.That(caption.Text, Does.Contain("name: user1"));
        }
    }
}