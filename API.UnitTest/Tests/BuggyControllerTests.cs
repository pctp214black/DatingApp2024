namespace API.UnitTests.Tests;

using System.Net;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.UnitTests.Helpers;
using Newtonsoft.Json.Linq;

public class BuggyControllerTests
{
    private readonly string apiRoute = "api/v1/Buggy";
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUrl;
    private string loginObject;
    private string registerObject;
    private HttpContent httpContent;

    public BuggyControllerTests()
    {
        _client = TestHelper.Instance.Client;
    }

    [Fact]
    public async Task GetSecretShouldOK()
    {
        // Arrange
        var expectedStatusCode = "OK";
        requestUrl = "api/v1/account/login";
        var loginRequest = new LoginRequest
        {
            UserName = "arenita",
            Password = "123456"
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse.Token);

        requestUrl = $"{apiRoute}/auth";

        // Act
        httpResponse = await _client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(expectedStatusCode, true), httpResponse.StatusCode);
        Assert.Equal(expectedStatusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("Unauthorized", "arenita", "12345689")]
    [InlineData("Unauthorized", "arenita2", "123456")]
    public async Task GetLoginTest(string expectedStatusCode, string username, string password)
    {
        // Arrange
        requestUrl = "api/v1/account/login";
        var loginRequest = new LoginRequest
        {
            UserName = username,
            Password = password
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(expectedStatusCode, true), httpResponse.StatusCode);
        Assert.Equal(expectedStatusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("No content", "arenita", "123456")]
    public async Task GetUpdateUser(string expectedStatusCode, string username, string password)
    {
        // Arrange
        requestUrl = "api/v1/account/login";
        var loginRequest = new LoginRequest
        {
            UserName = username,
            Password = password
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse.Token);

        requestUrl = $"api/v1/users";
        var jsonContent = new StringContent("{\"Introduction\":\"Introduction updated from test\", \"Interests\":\"Interest updated from test\"}", Encoding.UTF8, "application/json");

        // Act
        httpResponse = await _client.PutAsync(requestUrl, jsonContent);
    }

    [Fact]
    [InlineData("No content", "arenita", "123456")]
    public async Task GetUpdateUserFalse()
    {
        // Arrange
        var expectedStatusCode = "OK";
        requestUrl = "api/v1/account/login";
        var loginRequest = new LoginRequest
        {
            UserName = "arenita",
            Password = "123456"
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse.Token);

        requestUrl = $"api/v1/users";
        var jsonContent = new StringContent("{\"Introduction\":\"Introduction updated from test\", \"Interests\":\"Interest updated from test\"}", Encoding.UTF8, "application/json");

        // Act
        httpResponse = await _client.PutAsync(requestUrl, jsonContent);
    }

    [Theory]
    [InlineData("OK", "arenita", "123456")]
    public async Task GetByUserName(string expectedStatusCode, string username, string password)
    {
        // Arrange
        requestUrl = "api/v1/account/login";
        var loginRequest = new LoginRequest
        {
            UserName = username,
            Password = password
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse.Token);

        requestUrl = $"api/v1/users/{username}";
        // Act
        httpResponse = await _client.GetAsync(requestUrl);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(expectedStatusCode, true), httpResponse.StatusCode);
        Assert.Equal(expectedStatusCode, httpResponse.StatusCode.ToString());
    }

    [Fact]
    public async Task GetByUserNameFalse()
    {
        // Arrange
        var expectedStatusCode = "NotFound";
        requestUrl = "api/v1/account/login";
        var loginRequest = new LoginRequest
        {
            UserName = "arenita",
            Password = "123456"
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse.Token);

        requestUrl = $"api/v1/users/usuario";
        // Act
        httpResponse = await _client.GetAsync(requestUrl);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(expectedStatusCode, true), httpResponse.StatusCode);
        Assert.Equal(expectedStatusCode, httpResponse.StatusCode.ToString());
    }

    // [Fact]
    // public async Task SeedTest(){
    //      var context = sp.GetRequiredService<DataContext>();
    //     await Seed.SeedUsersAsync(context);
    // }



    [Fact]
    public async Task GetAllUsers()
    {
        // Arrange
        var expectedStatusCode = "OK";
        requestUrl = $"api/v1/users";
        // Act
        httpResponse = await _client.GetAsync(requestUrl);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(expectedStatusCode, true), httpResponse.StatusCode);
        Assert.Equal(expectedStatusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "Pedro", "123456")]
    [InlineData("BadRequest", "arenita", "123456")]
    public async Task PostNewUser(string expectedStatusCode, string username, string password)
    {
        // Arrange
        requestUrl = "api/v1/account/register";
        var registerRequest = new RegisterRequest
        {
            UserName = username,
            Password = password
        };
        registerObject = GetRegisterObject(registerRequest);
        httpContent = GetHttpContent(registerObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        Console.WriteLine($"{username}: {httpResponse.StatusCode}");
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(expectedStatusCode, true), httpResponse.StatusCode);
        Assert.Equal(expectedStatusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("NotFound")]
    public async Task GetNotFoundShouldNotFound(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/not-found";

        // Act
        httpResponse = await _client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("InternalServerError")]
    public async Task GetServerErrorShouldNotInternalServerError(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/server-error";

        // Act
        httpResponse = await _client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("BadRequest")]
    public async Task GetBadRequestShouldBadRequest(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/bad-request";

        // Act
        httpResponse = await _client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    #region Privated methods

    private static string GetLoginObject(LoginRequest loginDto)
    {
        var entityObject = new JObject()
            {
                { nameof(loginDto.UserName), loginDto.UserName },
                { nameof(loginDto.Password), loginDto.Password }
            };

        return entityObject.ToString();
    }

    private static string GetRegisterObject(RegisterRequest registerDto)
    {
        var entityObject = new JObject()
            {
                { nameof(registerDto.UserName), registerDto.UserName },
                { nameof(registerDto.Password), registerDto.Password }
            };

        return entityObject.ToString();
    }

    private static StringContent GetHttpContent(string objectToCode) =>
        new(objectToCode, Encoding.UTF8, "application/json");


    #endregion
}