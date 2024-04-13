using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests_Ageeva;

public class SeleniumTestsForPractice
{
    [Test] //nUnit для запуска теста
    public void Authorization()
    {
        var option = new ChromeOptions();
        option.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        
        //Зайти в хром
        var driver = new ChromeDriver(option);
        //перейти по ссылке
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");

        //вести логин и пароль
        //находим элемент
        var login = driver.FindElement(By.Id("Username"));
        // ввод текста в стоку
        login.SendKeys("agtanyav@yandex.ru");

        var password = driver.FindElement(By.Id("Password"));
        password.SendKeys("Lenovo2001!");

        var btn = driver.FindElement(By.Name("button"));
        btn.Click();
        
        //Проверка, что мы находимся на странице
        var currentUrl = driver.Url;
        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/news");
        
        
        //Закрыть браузер и процесс драйвера
        driver.Quit();
    }
}