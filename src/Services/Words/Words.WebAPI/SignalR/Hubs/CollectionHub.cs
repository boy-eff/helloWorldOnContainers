using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Messages;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Extensions;
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
    
    private static string StartRoute = "Start";

    public CollectionHub(IConfiguration configuration, IWordCollectionTestGenerator testGenerator, WordsDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _configuration = configuration;
        _testGenerator = testGenerator;
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public override async Task OnConnectedAsync()
    {
        var routeParam = _configuration["SignalR:CollectionIdParameterName"];
        var answerOptionsCount = Convert.ToInt32(_configuration["SignalR:AnswerOptionsCount"]);
        var collectionId = Convert.ToInt32(this.Context.GetHttpContext().GetRouteValue(routeParam));
        
        Context.Items.SetCollectionId(collectionId);
        
        var tests = await _testGenerator.GenerateTestsFromCollection(collectionId, answerOptionsCount);
        
        var enumerator = tests.GetEnumerator();
        enumerator.MoveNext();
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
        
        var correctAnswer = test.AnswerOptions.FirstOrDefault(x => x.IsCorrect).Value;
        var question = new WordCollectionTestQuestion(collectionId, correctAnswer, userAnswer);
        var testPassInformation = Context.Items.GetTestPassInformation();
        testPassInformation.TotalQuestions++;
        if (question.IsCorrect)
        {
            testPassInformation.CorrectAnswersAmount++;
        }
        testPassInformation.Questions.Add(question);
        
        Context.Items.SetTestPassInformation(testPassInformation);
        
        var nextSucceed = testEnumerator.MoveNext();
        if (nextSucceed)
        {
            var testDto = testEnumerator.Current.Adapt<WordCollectionTestQuestionDto>();
            return testDto;
        }
        
        await _dbContext.WordCollectionTestPassInformation.AddAsync(testPassInformation);
        var message = testPassInformation.Adapt<WordCollectionTestPassedMessage>();
        await _publishEndpoint.Publish(message);
        await _dbContext.SaveChangesAsync();
        return null;
    }
}