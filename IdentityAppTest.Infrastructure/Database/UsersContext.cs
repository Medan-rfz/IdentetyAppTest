using IdentityAppTest.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace IdentityAppTest.Infrastructure.Database;

public class UsersContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Moderator> Moderators { get; set; }

    private const string SettingsFilePath = "../IdentityAppTest.Infrastructure/Configs/dbsettings.json";

    public UsersContext() { }

    public UsersContext(DbContextOptions<UsersContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        try
        {
            string jsonString = File.ReadAllText(SettingsFilePath);
            var jsonConverter = JsonSerializer.Deserialize<DbSettings>(jsonString);
            optionsBuilder.UseNpgsql(jsonConverter?.ConnectionString, b => b.MigrationsAssembly("IdentityAppTest"));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
