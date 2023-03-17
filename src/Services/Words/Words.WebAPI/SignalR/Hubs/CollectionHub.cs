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

    private const string StartRoute = "Start";

    public CollectionHub(IConfiguration configuration, IWordCollectionTestGenerator testGenerator, WordsDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _configuration = configuration;
        _testGenerator = testGenerator;
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public override async Task OnConnectedAsync()
    {

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
    }

    public async Task<WordCollectionTestQuestionDto> ReceiveAnswerAndSendNextWord(string userAnswer)
    {
        var testEnumerator = Context.Items.GetTestEnumerator();
        var collectionId = Context.Items.GetCollectionId();
        var test = testEnumerator.Current;

        var correctAnswer = test.GetCorrectAnswerOptionValue();
        var question = new WordCollectionTestQuestion(collectionId, correctAnswer, userAnswer);
        var testPassInformation = Context.Items.GetTestPassInformation();
        
        testPassInformation.AddAnswerToTestPassInformation(question);
        
        Context.Items.SetTestPassInformation(testPassInformation);
        
        var nextSucceed = testEnumerator.MoveNext();
        if (nextSucceed)
        {
            var testDto = testEnumerator.Current.Adapt<WordCollectionTestQuestionDto>();
            return testDto;
        }
        
        await _dbContext.WordCollectionTestPassInformation.AddAsync(testPassInformation);
        await PublishTestPassedMessage(testPassInformation);
        await _dbContext.SaveChangesAsync();
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