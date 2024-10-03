﻿
using BankingApi.EventReceiver.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApi.EventReceiver
{
    public class BankingApiDbContext : DbContext
    {
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=BankingApiTest;Integrated Security=True;TrustServerCertificate=True;");
    }
}
