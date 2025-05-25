using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Xunit;

namespace MovieApp.SeleniumTests
{
    public class MoviePageTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private const string BaseUrl = "https://localhost:5001";

        public MoviePageTests()
        {
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Fact]
        public void CanNavigateToMovieList()
        {
            // Arrange
            _driver.Navigate().GoToUrl($"{BaseUrl}/Movie");

            // Act
            var movieList = _wait.Until(ExpectedConditions.ElementExists(By.Id("movieList")));
            var movieItems = movieList.FindElements(By.TagName("tr"));

            // Assert
            Assert.True(movieItems.Count > 0, "Movie list should not be empty");
        }

        [Fact]
        public void CanCreateNewMovie()
        {
            // Arrange
            _driver.Navigate().GoToUrl($"{BaseUrl}/Movie/Create");

            // Act
            var titleInput = _wait.Until(ExpectedConditions.ElementExists(By.Id("Title")));
            var directorInput = _driver.FindElement(By.Id("Director"));
            var submitButton = _driver.FindElement(By.CssSelector("input[type='submit']"));

            titleInput.SendKeys("Test Movie");
            directorInput.SendKeys("Test Director");
            submitButton.Click();

            // Assert
            _wait.Until(ExpectedConditions.UrlContains("/Movie"));
            var successMessage = _wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".alert-success")));
            Assert.Contains("successfully", successMessage.Text.ToLower());
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
} 