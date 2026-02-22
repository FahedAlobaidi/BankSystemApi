using BankSystemApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystemApi.DbContexts
{
    public class BankContext:DbContext
    {
        public DbSet<Client> clients { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Transaction> transactions { get; set; }
        public DbSet<Account> accounts { get; set; }

        public BankContext(DbContextOptions<BankContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Run Base Configuration First (Best Practice)
            base.OnModelCreating(modelBuilder);

            // =========================================================================
            // 2. SAFETY RULES (Delete Behavior)
            // =========================================================================

            //Rule A: Cannot delete a USER if they have a CLIENT profile
            modelBuilder.Entity<Client>()
                .HasOne(c => c.User)
                .WithOne(u => u.Client)
                .HasForeignKey<Client>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //Rule B: Cannot delete a CLIENT if they have ACCOUNTS
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Client)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            //Rule c: Cannot delete a ACCOUNT if they have TRANSACTIONS
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
               .Property(t => t.TransactionType)
               .HasConversion<string>();

            Guid userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            Guid clientId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            Guid accountId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            // Seed the initial Client data
            modelBuilder.Entity<Client>().HasData(
                new Client()
                {
                    Id = clientId,
                    FirstName = "Fahed",
                    LastName = "Alobaidi",
                    Email = "fahedalobaidi@gmail.com",
                    Phone = "00012341234",
                    UserId = userId,
                });

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = userId,
                    Email = "fahedalobaidi@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("sa123456"),
                    Role = "Admin",

                }
                );

            modelBuilder.Entity<Account>().HasData(
                new Account()
                {
                    Id=accountId,
                    AccountNumber="121233445",
                    AccountBalance = 10000,
                    ClientId =clientId,
                    Status="Active",
                    Currency="JOD",
                }
                );

            modelBuilder.Entity<Transaction>().HasData(
                new Transaction()
                {
                    Id=Guid.NewGuid(),
                    Amount = 10000,
                    Currency="JOD",
                    Description="Initial opening balance",
                    TransactionDate=DateTime.UtcNow,
                    RunningBalance= 10000,
                    TransactionType=TransactionType.Deposit,
                    AccountId=accountId,
                });
            
        }
    }

    
}
