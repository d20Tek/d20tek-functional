using D20Tek.Functional;
using TodoService;

CreateWebApplication(args)
    .Map(builder => builder.Build())
    .Iter(app => app.ConfigurePipeline())
    .Iter(app => app.Run());

static Identity<WebApplicationBuilder> CreateWebApplication(string[] args) =>
    WebApplication.CreateBuilder(args)
                  .ConfigureServices();
