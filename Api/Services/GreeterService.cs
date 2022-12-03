using Repository.DatabaseContext;

namespace Api.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private readonly AppDbContext _dbContext;

    public GreeterService(ILogger<GreeterService> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        var faker = new Faker();
        var user = new User
        {
            Id = faker.Random.Guid(),
            Email = faker.Person.Email,
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName
        };
        var result = await _dbContext.Users.AddAsync(user);

        return await Task.FromResult(new HelloReply
        {
            Message = $"An User is Created {request.Name}. User : {result.Entity.FirstName}"
        });
    }
}