using FinanceWebScraperUsingAsp.NetAndSQL.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinanceWebScraperUsingAsp.NetAndSQL.Services
{
    public class Scraper
    {
        private string userid;
        private string password;

        public Scraper(string userid, string password)
        {
            this.userid = userid;
            this.password = password;
        }

        public List<Stock> Scrape()
        {
            var options = new ChromeOptions();
            //options.AddArgument("--headless");
            options.AddArguments("--disable-gpu");
            options.AddArguments("disable-popup-blocking");//to disable pop-up blocking

            var chromeDriver = new ChromeDriver();

            chromeDriver.Navigate().GoToUrl("https://login.yahoo.com/");
            Console.WriteLine("In Yahoo home page");
            chromeDriver.Manage().Window.Maximize();

            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            chromeDriver.FindElementById("login-username").SendKeys(this.userid + Keys.Enter);
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //chromeDriver.FindElementById("login-signin").Click();

            chromeDriver.FindElementById("login-passwd").SendKeys(this.password + Keys.Enter);
            //chromeDriver.FindElementById("login-signin").Click();

            chromeDriver.Url = "https://finance.yahoo.com/portfolio/p_1/view/v1";
            Console.WriteLine("In yahoo finance page");
            
            //var closePopup = chromeDriver.FindElementByXPath("//dialog[@id = '__dialog']/section/button");
            //closePopup.Click();
            //var items = chromeDriver.FindElementsByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[*]/td[*]/span/a").GetAttribute;
            IWebElement list = chromeDriver.FindElementByTagName("tbody");
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> items = list.FindElements(By.TagName("tr"));
            int count = items.Count();
            List<Stock> result = new List<Stock>();
            Console.WriteLine(count);
            //loop to get details of each stock symbol
            for (int i = 1; i <= count; i++)
            {
                Console.WriteLine(i);
                var symbol = chromeDriver.FindElementByXPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[1]").GetAttribute("innerText");
                var lastprice = chromeDriver.FindElementByXPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[2]").GetAttribute("innerText");
                var change = chromeDriver.FindElementByXPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[3]/span").GetAttribute("innerText");
                var percentChange = chromeDriver.FindElementByXPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[4]/span").GetAttribute("innerText");
                var currency = chromeDriver.FindElementByXPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[5]").GetAttribute("innerText");
                var avgVolume = chromeDriver.FindElementByXPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[9]").GetAttribute("innerText");
                var marketCap = chromeDriver.FindElementByXPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[13]").GetAttribute("innerText");

                Stock stock = new Stock();
                Console.WriteLine(symbol);
                stock.Symbol = symbol;
                Console.WriteLine(lastprice);
                stock.Price = lastprice;
                Console.WriteLine(change);
                stock.Change = change;
                Console.WriteLine(percentChange);
                stock.PercentChange = percentChange.Trim('%');
                Console.WriteLine(currency);
                stock.Currency = currency;
                Console.WriteLine(avgVolume);
                stock.AverageVolume = avgVolume;
                Console.WriteLine(marketCap);
                stock.MarketCap = marketCap;


                result.Add(stock);


            }
            return result;
        }
    }
}