using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Size = System.Drawing.Size;

namespace SeleniumTests_Ageeva;

public class SeleniumTestsForPractice
{ 
    public ChromeDriver driver;
    
    [SetUp]
    public void SetUp()
    {
        var option = new ChromeOptions();
        option.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        
        driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(6); 
        
        Authorization();
    }
    
    [Test] // Проверка авторизации и переход на страницу "Новости"
    public void NewsTest() 
    {
        // жду пока загрузится титул
        var news = driver.FindElement(By.CssSelector("[data-tid='Title']")); 
        // получаю URL страницы
        var currentUrl = driver.Url;
        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/news", 
            "текущий URL = " + currentUrl + " а должно https://staff-testing.testkontur.ru/news");
    }

    [Test] //проверка гамбургера в разрешении экрана 375 на 812
    public void Community()
    { 
        // ставлю разрешение 375 на 812
        driver.Manage().Window.Size = new Size(375, 812); 
        // ищу меню и кликаю на него
        var sideMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sideMenu.Click();
        // в открывшемся меню ищу "Сообщества" и кликаю
        var community = driver.FindElements(By.CssSelector("[data-tid='Community']")).First(element => element.Displayed );
        community.Click();
        // жду пока загрузится титул
        var communityTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        // получаю URL страницы
        var currentUrl = driver.Url;
        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/communities", "текущий URL = " 
            + currentUrl + " а должно https://staff-testing.testkontur.ru/communities");
        
    }

    [Test] // проверка поиска
    public void SearchTest()
    { 
        // ищу поисковыю строку и кликаю на нее
        var search = driver.FindElement(By.ClassName("react-ui-1uzh48y"));
        search.Click();
        // ввожу первые 4 буквы своей фамилии
        var searchInput = driver.FindElement(By.ClassName("react-ui-1oilwm3"));
        searchInput.SendKeys("Агее");
        // в списке ищу свое ФИО и кликаю на него
        var nameSearch = driver.FindElement(By.ClassName("react-ui-162kz0e"));
        nameSearch.Click();
        // осуществляется переход в профиль, проверяю что действительно мое имя
        var userName = driver.FindElement(By.CssSelector("[data-tid='EmployeeName']")).Text;
        Assert.That(userName == "Татьяна Агеева", "Имя пользователя должно быть 'Агеева Татьяна', а не " + userName);
    }

    [Test] // проверка новогодней темы
    public void NewYearTheme()
    {
        // кликаю на профиль
        var menu = driver.FindElement(By.CssSelector("[data-tid='PopupMenu__caption']"));
        menu.Click();
        
        // перехожу на настройки
        var settings = driver.FindElement(By.CssSelector("[data-tid='Settings']"));
        settings.Click();
        
        // нахожу checkbox и кликаю на него
        var btnNewYear = driver.FindElement(By.ClassName("react-ui-1lkzaoi"));
        btnNewYear.Click();
        
        // сохраняю настройки
        var save = driver.FindElement(By.ClassName("react-ui-1m5qr6w"));
        save.Click();
        
        // поиск ClassName гирлянды на странице
        var element = driver.FindElements(By.ClassName("sc-dvUynV"));
        
        // Проверяю что такой элемент (гирлянда) есть на сайте
        Assert.That(element.Count == 1, "Ужас! Гринч украл гирлянду. Никакого новогоднего настроения(Элемент не найден на сайте)))");
        
    }
    
    [Test] // изменение описания сообщества
    public void СreateNewDescription()
    {
        // ищу элемент на странице "новости", чтобы потом успешно перейти на страницу настроек сообщества
        var news = driver.FindElement(By.CssSelector("[data-tid='Title']")); 

        // перехожу на страницу настройки сообщества
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities/385ea47d-6842-41a7-b0e0-7b8bc69fb2b8/settings");
        
        // нахожу textarea - описание сообщества
        var communityDescription = driver.FindElement(By.ClassName("react-ui-r3t2bi"));
        
        // извлекаю значение у textarea
        string existingText = communityDescription.GetAttribute("value");
    
        // очищаю textarea отправкой клавиш Delete
        for (int i = 0; i < existingText.Length; i++)
        {
            communityDescription.SendKeys(Keys.Backspace);
        }
        
        // ввожу переменную с текстом
        string newText = "какое то описание";
        
        // отправляю в описание текст
        communityDescription.SendKeys(newText);
        
        // нахожу и жмакаю на кнопку "сохранить"
        var save = driver.FindElements(By.ClassName("sc-juXuNZ"))[1];
        save.Click();
        
        // после этого осуществляется выход из настроек и я получаю текст описания сообщества
        var description = driver.FindElement(By.CssSelector("[class='sc-bdnxRM sc-jcwpoC sc-iTVJFM kSMcXF hiYOHX']")).Text;
        
        Assert.That(description==newText, "описание сообщества должно быть - " + newText + ", а оно - " + description);
        
    }

    public void Authorization()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("");

        var password = driver.FindElement(By.Id("Password"));
        password.SendKeys("");

        var btnAuthorization = driver.FindElement(By.Name("button"));
        btnAuthorization.Click();
        
    }
    
    [TearDown]
    public void TearDown()
    { 
        driver.Close(); 
        driver.Quit(); 
    }
}
