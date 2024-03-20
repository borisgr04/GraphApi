using EntityGraphQL.AspNet;
using GraphApi;
using GraphQL.Server.Ui.Altair;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Again, just an example using EF but you do not have to
builder.Services.AddDbContext<CibContext>(opt => opt.UseInMemoryDatabase("Demo"));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// This registers a SchemaProvider<DemoContext> and uses reflection to build the schema with default options
builder.Services.AddGraphQLSchema<CibContext>();
builder.Services.AddGraphQLSchema<CibContext>();

var app = builder.Build();

var cibContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<CibContext>();
await DatabaseSeeder.SeedAsync(cibContext);

app.UseRouting();
app.UseAuthorization();

//app.MapControllers();
//app.MapGraphQL<CibContext>(); // default url: /
                              // 
app.UseEndpoints(routeBuilder =>
{
    routeBuilder.MapControllers();
    routeBuilder.MapGraphQL<CibContext>();
    routeBuilder.MapGraphQLAltair(new AltairOptions
    {
        GraphQLEndPoint = PathString.FromUriComponent("/graphql")
    });
});

//app.UseEndpoints(routeBuilder =>
//{
//    routeBuilder.MapControllers();
//    
//});

app.Run();
