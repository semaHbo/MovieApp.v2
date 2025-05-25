using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;
using FluentAssertions;

namespace MovieApp.v2.Tests.UI;

[Trait("Category", "UI")]
public class MovieUITests : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private const string BaseUrl = "https://localhost:7001";

    public MovieUITests()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless"); // Headless mode for CI/CD
        _driver = new ChromeDriver(options);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    [Fact]
    public void Index_DisplaysMovieList()
    {
        // Arrange
        _driver.Navigate().GoToUrl($"{BaseUrl}/Movie");

        // Act
        var movieElements = _wait.Until(d => d.FindElements(By.CssSelector(".movie-item")));

        // Assert
        movieElements.Should().NotBeEmpty();
    }

    [Fact]
    public void Create_SubmitsNewMovie()
    {
        // Arrange
        _driver.Navigate().GoToUrl($"{BaseUrl}/Movie/Create");

        // Act
        _wait.Until(d => d.FindElement(By.Id("Title"))).SendKeys("Test Movie");
        _wait.Until(d => d.FindElement(By.Id("Director"))).SendKeys("Test Director");
        _wait.Until(d => d.FindElement(By.Id("Description"))).SendKeys("Test Description");
        var genreSelect = new SelectElement(_wait.Until(d => d.FindElement(By.Id("GenreId"))));
        genreSelect.SelectByIndex(1);
        _wait.Until(d => d.FindElement(By.Id("Rating"))).SendKeys("4.5");
        _wait.Until(d => d.FindElement(By.Id("ReleaseDate"))).SendKeys(DateTime.Now.ToString("yyyy-MM-dd"));

        _wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']"))).Click();

        // Assert
        _wait.Until(d => d.Url.EndsWith("/Movie"));
        _driver.PageSource.Should().Contain("Test Movie");
    }

    [Fact]
    public void Edit_UpdatesExistingMovie()
    {
        // Arrange
        _driver.Navigate().GoToUrl($"{BaseUrl}/Movie");
        var firstEditLink = _wait.Until(d => d.FindElement(By.LinkText("Edit")));
        firstEditLink.Click();

        // Act
        var titleInput = _wait.Until(d => d.FindElement(By.Id("Title")));
        titleInput.Clear();
        titleInput.SendKeys("Updated Movie Title");

        _wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']"))).Click();

        // Assert
        _wait.Until(d => d.Url.EndsWith("/Movie"));
        _driver.PageSource.Should().Contain("Updated Movie Title");
    }

    [Fact]
    public void Delete_RemovesMovie()
    {
        // Arrange
        _driver.Navigate().GoToUrl($"{BaseUrl}/Movie");
        var firstDeleteLink = _wait.Until(d => d.FindElement(By.LinkText("Delete")));
        var movieTitle = firstDeleteLink.FindElement(By.XPath("../../td[1]")).Text;
        firstDeleteLink.Click();

        // Act
        _wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']"))).Click();

        // Assert
        _wait.Until(d => d.Url.EndsWith("/Movie"));
        _driver.PageSource.Should().NotContain(movieTitle);
    }

    [Fact]
    public void Details_DisplaysMovieInformation()
    {
        // Arrange
        _driver.Navigate().GoToUrl($"{BaseUrl}/Movie");
        var firstDetailsLink = _wait.Until(d => d.FindElement(By.LinkText("Details")));
        firstDetailsLink.Click();

        // Assert
        var movieDetails = _wait.Until(d => d.FindElement(By.CssSelector(".movie-details")));
        movieDetails.Should().NotBeNull();
        movieDetails.Text.Should().NotBeEmpty();
    }

    public void Dispose()
    {
        _driver?.Quit();
        _driver?.Dispose();
    }
} 