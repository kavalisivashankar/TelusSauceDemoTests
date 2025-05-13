using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Linq;
using System.Threading;

namespace SauceDemoTests
{
    [TestFixture]
    public class TelusSauceDemoTests
    {
        private IWebDriver driver;
        private string baseUrl = "https://www.saucedemo.com/";

        [SetUp]
        public void Setup()
        {
            var options = new FirefoxOptions();
            //options.AddArgument("--headless"); // Remove this line if you want to see the browser
            driver = new FirefoxDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            driver.Dispose();
        }

        // Test 1: Verify login and products page
        [Test]
        public void Test_Login_VerifyProductsPage()
        {
            driver.Navigate().GoToUrl(baseUrl);
            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();

            Assert.That(driver.Url.Contains("inventory.html"), "Did not navigate to Products page.");
            Assert.That(driver.FindElement(By.ClassName("title")).Text.Contains("Products"));
        }

        // Test 2: Add 3 items, buy 2
        [Test]
        public void Test_Add3Items_Buy2()
        {
            Login();

            // Add 3 items to cart
            var addButtons = driver.FindElements(By.CssSelector(".inventory_item button")).Take(3).ToList();
            foreach (var btn in addButtons)
                btn.Click();

            driver.FindElement(By.ClassName("shopping_cart_link")).Click();

            // Remove one item
            driver.FindElements(By.CssSelector(".cart_button")).First().Click();

            // Checkout
            driver.FindElement(By.Id("checkout")).Click();
            driver.FindElement(By.Id("first-name")).SendKeys("Test");
            driver.FindElement(By.Id("last-name")).SendKeys("User");
            driver.FindElement(By.Id("postal-code")).SendKeys("12345");
            driver.FindElement(By.Id("continue")).Click();

            // Finish
            driver.FindElement(By.Id("finish")).Click();

            Assert.That(driver.Url.Contains("checkout-complete.html"), "Order not completed.");
        }

        // Test 3: Make an order with total $30-$60
        [Test]
        public void Test_OrderTotalBetween30And60()
        {
            Login();

            var items = driver.FindElements(By.ClassName("inventory_item"));
            decimal total = 0;
            foreach (var item in items)
            {
                var priceText = item.FindElement(By.ClassName("inventory_item_price")).Text.Replace("$", "");
                decimal price = decimal.Parse(priceText);
                if (total + price <= 60)
                {
                    item.FindElement(By.TagName("button")).Click();
                    total += price;
                    if (total >= 30)
                        break;
                }
            }

            driver.FindElement(By.ClassName("shopping_cart_link")).Click();
            driver.FindElement(By.Id("checkout")).Click();
            driver.FindElement(By.Id("first-name")).SendKeys("Test");
            driver.FindElement(By.Id("last-name")).SendKeys("User");
            driver.FindElement(By.Id("postal-code")).SendKeys("12345");
            driver.FindElement(By.Id("continue")).Click();

            // Get total from summary
            var summaryTotalText = driver.FindElement(By.ClassName("summary_total_label")).Text;
            decimal summaryTotal = decimal.Parse(summaryTotalText.Replace("Total: $", ""));
            Assert.That(summaryTotal >= 30 && summaryTotal <= 60, $"Order total ${summaryTotal} is not between $30 and $60.");

            driver.FindElement(By.Id("finish")).Click();
            Assert.That(driver.Url.Contains("checkout-complete.html"), "Order not completed.");
        }

        // Helper method for login
        private void Login()
        {
            driver.Navigate().GoToUrl(baseUrl);
            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();
        }
    }
}
