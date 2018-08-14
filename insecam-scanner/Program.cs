using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace insecam_scanner
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("disable-infobars");

            IWebDriver driver = new ChromeDriver($"{System.Environment.CurrentDirectory}\\chromedriver_win32", options);
            driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 0, 4);

            DirectoryInfo folder = new DirectoryInfo(@"C:\tmp\insecam");
            foreach (var file in folder.GetFiles())
                file.Delete();

            for (int page = 1; page < 10; page++)
            {
                GoTo(driver, $"http://www.insecam.org/en/bycountry/IT/?page={page}");
                try
                {
                    int imgCount = driver.FindElements(By.CssSelector("img")).Count;

                    for (int i = 0; i < imgCount; i++)
                    {

                        GoTo(driver, $"http://www.insecam.org/en/bycountry/IT/?page={page}");

                        try
                        {
                            driver.FindElements(By.CssSelector("img"))[i].Click();
                            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                            jse.ExecuteScript("window.scrollBy(0,400)", "");

                            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                            screenshot.SaveAsFile(@"C:\tmp\insecam\" + page + "_" + i + ".jpg");
                        }
                        catch (WebDriverTimeoutException) { }


                    }
                }
                catch (WebDriverTimeoutException) { }
            }

            Console.WriteLine("This is the end!");
            Console.ReadKey();





        }

        private static void GoTo(IWebDriver driver, string url)
        {
            try
            {
                driver.Navigate().GoToUrl(url);
            }
            catch (WebDriverTimeoutException) { }
        }

    }
}
