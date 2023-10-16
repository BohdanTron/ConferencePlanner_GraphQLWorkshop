using GraphQL.Data;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;

namespace GraphQL.Tests;

public class AttendeeTests
{
    [Fact]
    public async Task RegisterAttendee()
    {
        // Arrange

        //var requestExecutor = await new ServiceCollection()
        //    .AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Data Source=conference.db"))
        //    .AddGraphQLServer()
        //    .AddQueryType(d => d.Name(OperationTypeNames.Query))
        //    .AddMutationType(d => d.Name(OperationTypeNames.Mutation))
        //    .AddTypeExtension<AttendeeQueries>()
        //    .AddTypeExtension<AttendeeMutations>()
        //    .AddDataLoader<AttendeeByIdDataLoader>()
        //    .AddType<AttendeeType>()
        //    .RegisterDbContext<ApplicationDbContext>()
        //    .AddGlobalObjectIdentification()
        //    .AddMutationConventions()
        //    .BuildRequestExecutorAsync();

        var requestExecutor = await new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Data Source=conference.db"))
            .AddGraphQLServer()
            .AddConferencePlannerTypes()
            .AddGlobalObjectIdentification()
            .AddMutationConventions()
            .RegisterDbContext<ApplicationDbContext>()
            .AddFiltering()
            .AddSorting()
            .AddInMemorySubscriptions()
            .AddTypeConverter<DateTime, DateTimeOffset>(t =>
                t.Kind is DateTimeKind.Unspecified ? DateTime.SpecifyKind(t, DateTimeKind.Utc) : t)
            .BuildRequestExecutorAsync();


        // Act
        var result = await requestExecutor.ExecuteAsync(
        """
                mutation RegisterAttendee {
                    registerAttendee(
                        input: {
                            emailAddress: "michael@chillicream.com"
                                firstName: "michael"
                                lastName: "staib"
                                userName: "michael3"
                            })
                    {
                        attendee {
                            id
                        }
                    }
                }
            """);

        // Assert
        var json = result.ToJson();

        result.MatchSnapshot();
    }
}