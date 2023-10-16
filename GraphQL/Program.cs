using GraphQL.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite("Data Source=conferences.db");
});

builder.Services
    .AddGraphQLServer()
    .AddConferencePlannerTypes()
    .AddGlobalObjectIdentification()
    .AddMutationConventions()
    .RegisterDbContext<ApplicationDbContext>()
    .AddTypeConverter<DateTime, DateTimeOffset>(t =>
        t.Kind is DateTimeKind.Unspecified ? DateTime.SpecifyKind(t, DateTimeKind.Utc) : t);

var app = builder.Build();

app.MapGraphQL();

app.Run();