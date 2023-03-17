using Words.WebAPI.SignalR.Hubs;

namespace Words.WebAPI.Extensions;

public static class SignalRExtensions
{
    public static void UseSignalR(this WebApplication app)
    {
        var param = app.Configuration["SignalR:CollectionIdParameterName"];
        app.MapHub<CollectionHub>($"/collectionhub/{{{param}}}");
    }
}