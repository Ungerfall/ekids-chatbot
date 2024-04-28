using EKids.Chatbot.Tasks.DataAccessLayer;
using EKids.Chatbot.Tasks.PresentationLayer;

namespace EKids.Chatbot.Tasks;

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        builder.Services.ConfigureHttpJsonOptions(options
            => options.SerializerOptions.TypeInfoResolverChain.Insert(0, TasksJsonSerializerContext.Default));

        var app = builder.Build();
        app.MapLearningTasks();

        app.Run();
    }
}
