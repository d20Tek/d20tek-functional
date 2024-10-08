using D20Tek.Functional;
using TodoService.Endpoints.Todos;
using TodoService.Persistence;

namespace TodoService;

internal static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder) => 
        builder.Services.AddEndpointsApiExplorer()
                        .AddSwaggerGen()
                        .AddCors()
                        .AddRepository()
                        .Pipe(_ => builder);

    public static WebApplication ConfigurePipeline(this WebApplication app) =>
        app.ToIdentity()
           .Iter(app => app.UseSwaggerInDev()
                           .UseHttpsRedirection()
                           .UseCors())
           .Iter(app => app.MapTodoEndpoints());

    private static WebApplication UseSwaggerInDev(this WebApplication app) =>
        app.Pipe(a => a.Environment.IsDevelopment()
                       .IfTrueOrElse(
                            () => a.UseSwagger()
                                   .UseSwaggerUI()))
           .Pipe(a => a);
}
