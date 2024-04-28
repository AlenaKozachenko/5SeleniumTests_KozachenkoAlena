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
        options.AddArguments ("--no-sandbox", "--window-size=1920,1080", "--disable-extensions");
        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        
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
      
        var community = driver.FindElements(By.CssSelector("[data-tid='Community']"))
            .First(element => element.Displayed);
        community.Click();
        
        var communityTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        Assert.That(driver.Url == "https://staff-testing.testkontur.ru/communities",
            "На странице 'сообщества' Url неправильный");

    }
    
    [Test]
    public void ToProfile()
    {
        var searchBar = driver.FindElement(By.CssSelector("[data-tid='SearchBar']")).FindElement(By.TagName("span"));
        searchBar.Click();    
        
        var searchField = driver.FindElement(By.CssSelector("[data-tid='SearchBar']"))
            .FindElement(By.TagName("label"))
            .FindElements(By.TagName("span"))[1]
            .FindElement(By.TagName("input"));
        
        searchField.SendKeys(text:"Козаченко");

        var buttonsCount = driver.FindElements(By.CssSelector("[data-tid='ComboBoxMenu__item']")).Count();
        Assert.IsTrue(buttonsCount > 0);


    }
   
    [Test]
    public void Security()
    {
        var profileMenu = driver.FindElement(By.CssSelector("[data-tid='PopupMenu__caption']")).FindElement(By.TagName("button"));
        profileMenu.Click();
        
        var settings = driver.FindElement(By.CssSelector("[data-tid='Security']"));
        settings.Click();
        
        var currentUrl = driver.Url;
        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/security");

    }

    [Test]
    public void ToEvents()
    {
        
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/events"); 
        
        var findElement = driver.FindElements(By.CssSelector("[data-tid='Actual']"));
        Assert.IsTrue(findElement.Count()>0); 
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
        
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
        wait.Until(ExpectedConditions.UrlContains("https://staff-testing.testkontur.ru/news"));
    }

[TearDown]
public void TearDown()
{
    driver.Quit();
}

}