namespace OnlineCreditSystem.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;
using OnlineCreditSystem.Data;
using OnlineCreditSystem.Data.Models;
using System.Security.Cryptography;
using System.Text;

public static class AppBuilderExtension
{
    public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
    {
        using var scopedServices = app.ApplicationServices.CreateScope();
        var data = scopedServices.ServiceProvider.GetService<OnlineCreditSystemDbContext>();

        data.Database.Migrate();
        SeedAdmin(data);

        return app;
    }

    private static void SeedAdmin(OnlineCreditSystemDbContext data)
    {
        if (data.Users.Any())
        {
            return;
        }

        const string adminEmail = "admin@gmail.com";
        const string adminPassword = "admin123";
        const string adminPhoneNumber = "0123456789";

        var adminPasswordHash = ComputeSha256Hash(adminPassword);
        var user = new User
        {
            Email = adminEmail,
            Password = adminPasswordHash,
            ConfirmPassword = adminPasswordHash,
            PhoneNumber = adminPhoneNumber
        };

        data.Users.Add(user);
        data.SaveChanges();
    }

    private static string ComputeSha256Hash(string rawData)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }
}
