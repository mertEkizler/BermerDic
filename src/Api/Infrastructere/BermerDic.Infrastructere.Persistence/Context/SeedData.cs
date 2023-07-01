using BermerDic.Api.Domain.Models;
using BermerDic.Common.Infrastructure;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BermerDic.Infrastructere.Persistence.Context
{
    internal class SeedData
    {
        private static List<User> GetUsers()
        {
            var results = new Faker<User>("tr")
                    .RuleFor(i => i.Id, i => Guid.NewGuid())
                    .RuleFor(i => i.Created,
                             i => i.Date.Between(DateTime.UtcNow.AddDays(-100), DateTime.UtcNow))
                    .RuleFor(i => i.FirstName, i => i.Person.FirstName)
                    .RuleFor(i => i.LastName, i => i.Person.LastName)
                    .RuleFor(i => i.Email, i => i.Person.Email)
                    .RuleFor(i => i.UserName, i => i.Person.UserName)
                    .RuleFor(i => i.Password, i => PasswordEncryptor.Encrypt(i.Internet.Password()))
                    .RuleFor(i => i.EmailConfirmed, i => i.PickRandom(true, false))
                .Generate(500);

            return results;
        }

        public async Task SeedAsync(IConfiguration configuration)
        {
            var dbContextOptionBuilder = new DbContextOptionsBuilder();
            dbContextOptionBuilder.UseSqlServer(configuration["BermerDicDbConnectionString"]);

            var context = new BermerDicContext(dbContextOptionBuilder.Options);

            if (context.Users.Any())
            {
                await Task.CompletedTask;
                return;
            }

            var users = GetUsers();
            var userIds = users.Select(u => u.Id).ToArray();

            await context.Users.AddRangeAsync(users);

            var guids = Enumerable.Range(0, 150).Select(i => Guid.NewGuid()).ToArray();
            var counter = 0;

            var entries = new Faker<Entry>("tr")
                    .RuleFor(i => i.Id, i => guids[counter++])
                    .RuleFor(i => i.Created,
                                 i => i.Date.Between(DateTime.UtcNow.AddDays(-100), DateTime.UtcNow))
                    .RuleFor(i => i.Subject, i => i.Lorem.Sentence(5, 5))
                    .RuleFor(i => i.Content, i => i.Lorem.Paragraph(2))
                    .RuleFor(i => i.CreatedById, i => i.PickRandom(userIds))
                .Generate(150);


            await context.Entries.AddRangeAsync(entries);

            var comments = new Faker<EntryComment>("tr")
                    .RuleFor(i => i.Id, i => Guid.NewGuid())
                    .RuleFor(i => i.Created,
                                 i => i.Date.Between(DateTime.UtcNow.AddDays(-100), DateTime.UtcNow))
                    .RuleFor(i => i.Content, i => i.Lorem.Paragraph(2))
                    .RuleFor(i => i.CreatedById, i => i.PickRandom(userIds))
                    .RuleFor(i => i.EntryId, i => i.PickRandom(guids))
                .Generate(1000);

            await context.EntryComments.AddRangeAsync(comments);

            await context.SaveChangesAsync();
        }
    }
}