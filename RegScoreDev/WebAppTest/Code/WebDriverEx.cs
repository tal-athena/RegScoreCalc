using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace WebAppTest
{
    public static class WebDriverEx
    {
        public static void CaptureWebPageToFile(this IWebDriver browser, string filePath)
        {
            if (!GlobalSettings.Instance.SaveScreenShots)
                return;
            Screenshot ss = ((ITakesScreenshot)browser).GetScreenshot();
            ss.SaveAsFile(filePath, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
        }

        public static void WaitForPageLoad(this IWebDriver driver)
        {
            var wait =
                new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(30.00));

            wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public static void WaitForJqueryAjax(this IWebDriver driver)
        {
            while (true) // Handle timeout somewhere
            {
                Thread.Sleep(100);
                var ajaxIsComplete = (bool)(driver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
                if (ajaxIsComplete)
                    break;
            }
        }

        public static void WaitForElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(ExpectedConditions.ElementExists(by));
        }

        public static bool IsTextPresent(this IWebDriver driver, string text)
        {
            var bodyTag = driver.FindElement(By.TagName("body"));
            return bodyTag.Text.Contains(text);
        }
    }
}
