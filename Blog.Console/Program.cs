
using Blog.Domain.Enums;
using Blog.Infrastructure.Migrations;
using Blog.Infrastructure.Persistence;
using Blog.Infrastructure.Persistence.Seed;
using Bogus;
using Bogus.DataSets;

Console.WriteLine("Try to seed.");
Console.WriteLine("Seeding started...");

 var context = new BlogDbContext();
await SeedData.SeedDb(context);

Console.WriteLine("Seeding succeeded!");
