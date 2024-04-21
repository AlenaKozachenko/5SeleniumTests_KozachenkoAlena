using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Kozachenko_Alena__s_pravkami;

public class SeleniumTestsForPractic
{
    public ChromeDriver driver;

    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArguments ("--no-sandbox", "--start-maximized", "--disable-extensions");
        
        driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        // Авторизация
        Autorization();
        
    }

    [Test]
    public void Authorization()
    {
        var news = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        var currentUrl = driver.Url;
        currentUrl.Should().Be("https://staff-testing.testkontur.ru/news");
    }

    [Test]
    public void NavigationTest()
    {
        // клик на боковое меню
        var sideMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sideMenu.Click();
        // клик на "сообщества"
        var community = driver.FindElements(By.CssSelector("[data-tid='Community']"))
            .First(element => element.Displayed);
        community.Click();
        // проверяем, что Сообщества есть на странице + урл правильный
        var communityTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        Assert.That(driver.Url == "https://staff-testing.testkontur.ru/communities",
            "На странице 'сообщества' Url неправильный");

    }
    
    [Test]
    public void ToProfile()
    {
        // ввожу фамилию в строке поиска
        var searchBar = driver.FindElement(By.CssSelector("[data-tid='SearchBar']"));
        searchBar.SendKeys(text:"Козаченко");  // не понимаю, почему он не активен, ведь он один единственный
        
        // проверяю, что среди вариантов есть нужный 
        Assert.IsTrue(driver.FindElement(By.ClassName("react-ui-162kz0e")).Text.Equals("Алена Козаченко"));
    
    }
    
    [Test]
    public void Security()
    {
        // клик на "меню профиля"
        var profileMenu = driver.FindElement(By.CssSelector("[data-tid='ProfileMenu']"));
        profileMenu.Click();
        // клик на "Безопасность"
        var settings = driver.FindElement(By.CssSelector("[data-tid='Security']"));
        settings.Click();
        // проверяю что нахожусь на нужной странице
        var currentUrl = driver.Url;
        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/security");

    }
    [Test]
    public void ToEvents()
    {
        // загружаем страницу "Мероприятия"
        driver.Navigate().GoToUrl("staff-testing.testkontur.ru/events"); 
        // проверяем, что на странице есть "Актуальные" 
        Assert.IsTrue(driver.FindElements(By.CssSelector("[href data-tid = 'Actual']"))); // не понимаю, как должны срабатывать проверки такого типа
        
    }



    public void Autorization()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys(text:"geodezia09@mail.ru");
        
        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("Agile2324!");
        
        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
 
}