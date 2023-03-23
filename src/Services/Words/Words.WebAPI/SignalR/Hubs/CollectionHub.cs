using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Messages;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Extensions;
using Words.BusinessAccess.Models;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.WebAPI.SignalR.Hubs;

[Authorize]
public class CollectionHub : Hub
{
    private readonly IConfiguration _configuration;
    private readonly IWordCollectionTestGenerator _testGenerator;
    private readonly WordsDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CollectionHub> _logger;

    private const string StartRoute = "Start";

    public CollectionHub(IConfiguration configuration, IWordCollectionTestGenerator testGenerator, WordsDbContext dbContext, IPublishEndpoint publishEndpoint, ILogger<CollectionHub> logger)
    {
        _configuration = configuration;
        _testGenerator = testGenerator;
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User.GetUserId();
        _logger.LogInformation("SignalR | Connection with id {ConnectionId} has been started with user {UserId}",
            Context.ConnectionId, userId);
        
        var answerOptionsCount = GetAnswerOptionsCountFromConfiguration();
        var collectionId = GetCollectionIdFromHttpContext();
        
        Context.Items.SetCollectionId(collectionId);
        
        var tests = await _testGenerator.GenerateTestsFromCollection(collectionId, answerOptionsCount);

        var enumerator = InitializeEnumerator(tests);
        Context.Items.SetTestEnumerator(enumerator);

        var testPassInformation = new WordCollectionTestPassInformation() { UserId = Context.User.GetUserId(), WordCollectionId = collectionId };
        Context.Items.SetTestPassInformation(testPassInformation);
        
        var testDto = enumerator.Current.Adapt<WordCollectionTestQuestionDto>();
        await Clients.Caller.SendAsync(StartRoute, testDto, tests.Count);
        _logger.LogInformation("SignalR | Connection id: {ConnectionId} | Test passing has been started with user {UserId}",
            Context.ConnectionId, userId);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        _logger.LogInformation("Connection {ConnectionId} has been closed", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task<WordCollectionTestQuestionDto> ReceiveAnswerAndSendNextWord(string userAnswer)
    {
        var userId = Context.User.GetUserId();
        _logger.LogInformation("SignalR | Connection id: {ConnectionId} | Answer {UserAnswer} received from user {UserId}: ", 
            Context.ConnectionId, userAnswer, userId);
        var testEnumerator = Context.Items.GetTestEnumerator();
        var collectionId = Context.Items.GetCollectionId();
        var test = testEnumerator.Current;

        if (testEnumerator.Current is null)
        {
            Context.Abort();
            return null;
        }

        var correctAnswer = test.GetCorrectAnswerOptionValue();
        var question = new WordCollectionTestQuestion(collectionId, correctAnswer, userAnswer);
        var testPassInformation = Context.Items.GetTestPassInformation();
        
        testPassInformation.AddAnswerToTestPassInformation(question);
        
        Context.Items.SetTestPassInformation(testPassInformation);
        
        var nextSucceed = testEnumerator.MoveNext();
        if (nextSucceed)
        {
            var testDto = testEnumerator.Current.Adapt<WordCollectionTestQuestionDto>();
            _logger.LogInformation("SignalR | Connection id: {ConnectionId} | Sending next question to user {UserId}",
                Context.ConnectionId, userId);
            return testDto;
        }
        
        await _dbContext.WordCollectionTestPassInformation.AddAsync(testPassInformation);
        await PublishTestPassedMessage(testPassInformation);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("SignalR | Connection id: {ConnectionId} | Test {TestId} successfully passed by user {UserId}",
            Context.ConnectionId, testPassInformation.Id, userId);
        return null;
    }

    private int GetCollectionIdFromHttpContext()
    {
        var routeParam = _configuration["SignalR:CollectionIdParameterName"];
        var httpContext = Context.GetHttpContext();
        return Convert.ToInt32(httpContext.GetRouteValue(routeParam));
    }

    private int GetAnswerOptionsCountFromConfiguration()
    {
        return Convert.ToInt32(_configuration["SignalR:AnswerOptionsCount"]);
    }

    private IEnumerator<WordCollectionTest> InitializeEnumerator(IEnumerable<WordCollectionTest> tests)
    {
        var enumerator = tests.GetEnumerator();
        enumerator.MoveNext();
        return enumerator;
    }

    private async Task PublishTestPassedMessage(WordCollectionTestPassInformation testPassInformation)
    {
        var message = testPassInformation.Adapt<WordCollectionTestPassedMessage>();
        await _publishEndpoint.Publish(message);
    }
}