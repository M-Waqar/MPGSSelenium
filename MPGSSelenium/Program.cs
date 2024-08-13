using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;

namespace MPGSSelenium
{
    public class Program
    {
        public static IWebDriver driver = null;
        public static string midfile = @"C:\Users\muham\Desktop\Projects\MIDLIST.csv";
        public static void Main(string[] args)
        {
            Console.WriteLine("Program start...");
            List<string> midList = File.ReadAllLines(midfile).Skip(1).Select(v => MidFromCsv(v)).ToList();
            var options = new ChromeOptions();
            options.AddArgument("no-sandbox");
            using (driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Manage().Window.Maximize();
                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(5);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(5);
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

                    Thread.Sleep(5000);

                    //driver.Navigate().GoToUrl("https://adcb.gateway.mastercard.com/mm/createNewMerchant.s?selectedMenuItem=createMerchant");

                    //Thread.Sleep(200);
                    //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 100)");
                    //Thread.Sleep(200);

                    //driver.FindElement(By.Id("password.row")).Click();
                    //Thread.Sleep(50);

                    //driver.FindElement(By.ClassName("show-password")).Click(); 
                    //Thread.Sleep(50);
                    //IWebElement element = driver.FindElement(By.Id("password"));
                    //element.SendKeys("1");

                    foreach (string mid in midList)
                    {
                        Thread.Sleep(3000);

                        driver.Navigate().GoToUrl("https://adcb.gateway.mastercard.com/mm/merchantSearch.s?selectedMenuItem=merchantSearch");

                        IWebElement merchantIdTextBox = driver.FindElement(By.Id("merchantId"));
                        merchantIdTextBox.Clear();
                        merchantIdTextBox.SendKeys(mid);
                        merchantIdTextBox.SendKeys(Keys.Enter);

                        Thread.Sleep(3000);

                        //IList<IWebElement> links = driver.FindElements(By.TagName("a"));
                        //foreach (var link in links)
                        //{
                        //    string href = link.GetAttribute("href");
                        //    if (href.Contains("mm/paymentDetails.s?merchantSystemId"))
                        //    {
                        //        link.Click();
                        //        break;
                        //    }
                        //}
                        //Thread.Sleep(3000);

                        //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 100)");

                        //Thread.Sleep(200);

                        //IWebElement chkoptout = driver.FindElement(By.Id("mciSpProductsOptOut"));
                        //chkoptout.Click();

                        //IWebElement submitbtn = driver.FindElement(By.ClassName("SubmitSubmit"));
                        //submitbtn.Click();

                        //Thread.Sleep(2000);

                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 100)");

                        Thread.Sleep(200);

                        IWebElement linkApprove = driver.FindElement(By.Id("merchantApprove"));
                        linkApprove.Click();

                        Thread.Sleep(2000);

                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 100)");

                        Console.WriteLine("Press y to approve merchant");
                        string action = Console.ReadLine();
                        if (action == "y")
                        {
                            IWebElement btnapproved = driver.FindElement(By.ClassName("SubmitSubmit"));
                            btnapproved.Click();
                        }
                        else
                        {
                            Console.WriteLine("Pls manually check and approve MID: "+ mid);
                        }
                    }
                    
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
        public static string MidFromCsv(string csvLine)
        {
            return csvLine.Split(',')[0];
        }
    }
}
