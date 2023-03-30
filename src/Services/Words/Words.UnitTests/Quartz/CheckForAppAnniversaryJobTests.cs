using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Quartz;
using Shared.Messages;
using Words.BusinessAccess.Quartz.Jobs;
using Words.DataAccess;
using Words.DataAccess.Models;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.Quartz;

[TestFixture]
public class CheckForAppAnniversaryJobTests
{
    private WordsDbContext _dbContext;
    private Mock<IPublishEndpoint> _publishEndpointMock;
    private Mock<ILogger<CheckForAppAnniversaryJob>> _loggerMock;
    private IJobExecutionContext _jobExecutionContext;
    private CheckForAppAnniversaryJob _sut;
    
    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextProvider.GetMemoryContext();
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _loggerMock = new Mock<ILogger<CheckForAppAnniversaryJob>>();
        _jobExecutionContext = JobExecutionContextProvider.GetJobExecutionContext();
        _sut = new CheckForAppAnniversaryJob(_dbContext, _publishEndpointMock.Object, _loggerMock.Object);
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Execute_WhenCalled_ShouldPublishAppAnniversaryMessage()
    {
        const int yearsCount = 1;
        var user = new User() { Id = 1, CreatedAt = DateTimeOffset.Now.AddYears(-yearsCount) };
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        await _sut.Execute(_jobExecutionContext);
        
        _publishEndpointMock.Verify(publishEndpoint 
            => publishEndpoint.Publish(It.Is<AppAnniversaryMessage>(message 
                => message.UserId == user.Id && message.Years == yearsCount), 
                It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test]
    public async Task Execute_WhenRegistrationDateIsInvalid_ShouldNotPublishAppAnniversaryMessage()
    {
        const int yearsCount = 1;
        var user = new User() { Id = 1, CreatedAt = DateTimeOffset.Now.AddYears(yearsCount) };
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        await _sut.Execute(_jobExecutionContext);
        
        _publishEndpointMock.Verify(publishEndpoint 
            => publishEndpoint.Publish(It.IsAny<AppAnniversaryMessage>(), 
                It.IsAny<CancellationToken>()), Times.Never);
    }
}