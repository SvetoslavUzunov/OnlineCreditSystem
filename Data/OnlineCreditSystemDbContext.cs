namespace OnlineCreditSystem.Data;

using Microsoft.EntityFrameworkCore;
using OnlineCreditSystem.Data.Models;

public class OnlineCreditSystemDbContext : DbContext
{
    public OnlineCreditSystemDbContext(DbContextOptions<OnlineCreditSystemDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    public DbSet<Transaction> Transactions { get; set; }
}
