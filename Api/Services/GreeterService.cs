using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Api.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private readonly IUserRepository _userRepository;

    public GreeterService(ILogger<GreeterService> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        var faker = new Faker();
        var user = new User
        {
            Email = faker.Person.Email,
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName
        };
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync(true);
        var userResult = await _userRepository.ListAsync();
        var users = await _userRepository.Queryable.Where(x => x.CreatedAt > DateTime.UtcNow.AddDays(-1)).ToListAsync();

        return await Task.FromResult(new HelloReply
        {
            Message = $"An User is Created {request.Name}. User : {JsonConvert.SerializeObject(users, Formatting.None)}"
        });
    }
}