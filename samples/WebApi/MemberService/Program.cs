using D20Tek.Functional;
using MemberService;

CreateWebApplication(args)
    .Map(builder => builder.Build())
    .Iter(app => app.ConfigurePipeline())
    .Iter(app => app.Run());

Identity<WebApplicationBuilder> CreateWebApplication(string[] args) =>
    WebApplication.CreateBuilder(args)
                  .ConfigureServices();
