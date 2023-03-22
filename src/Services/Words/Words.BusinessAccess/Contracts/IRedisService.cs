using StackExchange.Redis;

namespace Words.BusinessAccess.Contracts;

public interface IRedisService
{
    IServer GetServer();
}