using Domain.Entities;
using Infrastructure.Persistence;
using System;

namespace Survey.Migration.App.Seed;

public static class UserSeed
{
    public static async Task AddAsync(ApplicationDbContext context)
    {
        if (context == null)
            return;

        if (!context.Users.Any())
        {
            var random = new Random();

            for (var i = 0; i < 10; i++)
            {
                int p = i + 1;
                var userName = "user" + p;
                await AddUserAsync(context, userName, "user", p.ToString());
            }

            await context.SaveChangesAsync();
        }
    }

    public static async Task AddUserAsync(ApplicationDbContext context, string userName, string firstName, string p)
    {
        var userId = "0672aad2-302c-4785-98a8-7c13fb2367" + p;
        context.Users.Add(new User
        {
            UserId = userId,
            Username = userName,
            FirstName = firstName,
            LastName = p,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123")
        });
    }
}
