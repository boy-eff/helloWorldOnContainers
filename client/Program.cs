using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using HelloWorldClient.Enums;
using HelloWorldClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Words.DataAccess.Models;

const string gatewayUrl = "http://localhost:5001/";
const string tokenUrl = gatewayUrl + "identity/connect/token";
const string usersUrl = gatewayUrl + "identity/api/users";
const string wordCollectionUrl = gatewayUrl + "words/api/wordcollection";
const string wordDictionaryUrl = gatewayUrl + "words/api/dictionary/words/";

var client = new HttpClient();

var user = await RegisterUserAsync(client);
var token = await RetrieveTokenAsync(user);

if (token is null)
{
    Console.WriteLine("Token retrieving failure");
}

var decodedToken = DecodeJwtToken(token.AccessToken);

var collection = await AddWordCollectionAsync(client, decodedToken);

await AddWordToUserDictionaryAsync(client, collection, decodedToken);



async Task<User> RegisterUserAsync(HttpClient client)
{
    HttpResponseMessage responseMessage;
    User user;
    do
    {
        Console.WriteLine("Enter username:");
        var username = Console.ReadLine();
        Console.WriteLine($"Trying to register user with username {username}");
        user = new User() { UserName = username, Password = "password", EnglishLevel = EnglishLevel.Elementary };
        var content = JsonContent.Create(user);
        responseMessage = await client.PostAsync(usersUrl, content);
        Console.WriteLine($"Server responded with status {responseMessage.StatusCode}");
    } while (!responseMessage.IsSuccessStatusCode);
    Thread.Sleep(5000);
    return user;
}

async Task<TokenResponse?> RetrieveTokenAsync(User user)
{
    var content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "grant_type", "password" },
        { "client_id", "client" },
        { "client_secret", "secret" },
        { "username", user.UserName },
        { "password", user.Password }
    });

    Console.WriteLine($"Retrieving token from url {tokenUrl}");
    var response = await client.PostAsync(tokenUrl, content);

    if (response.StatusCode != HttpStatusCode.OK)
    {
        return null;
    }

    var responseContent = await response.Content.ReadAsStringAsync();

    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent, new JsonSerializerSettings()
    {
        ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    });

    Console.WriteLine($"Token successfully retrieved: {tokenResponse.AccessToken}");
    Thread.Sleep(5000);
    return tokenResponse;
}

JwtSecurityToken DecodeJwtToken(string token)
{
    var handler = new JwtSecurityTokenHandler();
    var decodedToken = handler.ReadJwtToken(token);
    Console.WriteLine("JWT token successfully decoded");
    return decodedToken;

}

async Task<WordCollection> AddWordCollectionAsync(HttpClient client, JwtSecurityToken token)
{
    
    var userId = Convert.ToInt32(token.Claims.First(x => x.Type == "sub").Value);

    var collection = new WordCollection()
    {
        EnglishLevel = EnglishLevel.Advanced, 
        Name = "Test", 
        UserId = userId,
        Words = new List<Word>()
        {
            new Word()
            {
                Value = "TestWord",
                Translations = new List<WordTranslation>()
                {
                    new WordTranslation() { Translation = "TestTranslation" }
                }
            }
        }
    };

    var content = JsonContent.Create(collection);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.RawData);
    Console.WriteLine("Trying to save word collection with saved token");
    var responseMessage = await client.PostAsync(wordCollectionUrl, content);
    Console.WriteLine($"Server responded with status {responseMessage.StatusCode}");
    var response = await responseMessage.Content.ReadAsStringAsync();
    var returnedCollection = JsonConvert.DeserializeObject<WordCollection>(response, new JsonSerializerSettings());
    Thread.Sleep(5000);
    return returnedCollection;
}

async Task AddWordToUserDictionaryAsync(HttpClient client, WordCollection collection, JwtSecurityToken token)
{
    Console.WriteLine("Trying to add word to user dictionary");
    var word = collection.Words.First();
    var responseMessage = await client.PostAsync(wordDictionaryUrl + word.Id, null);
    Console.WriteLine($"Server responded with status {responseMessage.StatusCode}");
}