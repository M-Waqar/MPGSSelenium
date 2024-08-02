using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;
using System.Threading;

namespace MPGSSelenium
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Program start...");
            using (IWebDriver driver = new ChromeDriver())
            {
                try
                {
                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(5);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(5);
                    driver.Manage().Window.Maximize();
                    driver.Navigate().GoToUrl("https://adcb.gateway.mastercard.com/mm/");

                    IWebElement clientIdTextBox = driver.FindElement(By.Id("ownerId"));
                    clientIdTextBox.Clear();
                    clientIdTextBox.SendKeys("");

                    IWebElement userNameTextBox = driver.FindElement(By.Id("userName"));
                    userNameTextBox.Clear();
                    userNameTextBox.SendKeys("");

                    IWebElement passwordTextBox = driver.FindElement(By.Id("password"));
                    passwordTextBox.Clear();
                    passwordTextBox.SendKeys("");
                    passwordTextBox.SendKeys(Keys.Enter);

                    Thread.Sleep(3000);

                    driver.Navigate().GoToUrl("https://adcb.gateway.mastercard.com/mm/merchantSearch.s?selectedMenuItem=merchantSearch");

                    IWebElement merchantIdTextBox = driver.FindElement(By.Id("merchantId"));
                    merchantIdTextBox.Clear();
                    merchantIdTextBox.SendKeys("120810000118");
                    merchantIdTextBox.SendKeys(Keys.Enter);

                    Thread.Sleep(3000);

                    IList<IWebElement> links = driver.FindElements(By.TagName("a"));
                    foreach (var link in links)
                    {
                        string href = link.GetAttribute("href");
                        if (href.Contains("mm/paymentDetails.s?merchantSystemId"))
                        {
                            link.Click();
                            break;
                        }
                    }
                    Thread.Sleep(3000);

                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 100)");

                    IWebElement chkoptout = driver.FindElement(By.Id("mciSpProductsOptOut"));
                    chkoptout.Click();

                    IWebElement submitbtn = driver.FindElement(By.ClassName("SubmitSubmit"));
                    submitbtn.Click();

                    Thread.Sleep(3000);

                    Console.WriteLine("Program end...");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
                finally
                {
                    driver.Quit();
                }
            }
        }
    }
}
