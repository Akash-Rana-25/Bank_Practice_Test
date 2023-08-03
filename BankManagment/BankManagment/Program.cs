using BankManagmentDB;
using BankManagmentDB.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            //,ServiceLifetime.Transient
        );
//var numberOfBankAccountRecords = int.Parse(builder.Configuration["AppSettings:NumberOfBankAccountRecords"]);
//var numberOfBankTransactionRecords = int.Parse(builder.Configuration["AppSettings:NumberOfBankTransactionRecords"]);


//builder.Services.AddScoped(p => new ApplicationDbContext(p.GetRequiredService<DbContextOptions<ApplicationDbContext>>(), numberOfBankAccountRecords, numberOfBankTransactionRecords));

//builder.Services.AddScoped<ISeedDataService ,SeedDataService>();
builder.Services.AddTransient<DataSeeder>();


var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seed")
    Seed(app);

void Seed(IHost app)
{
    var scopeFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopeFactory.CreateScope())
    {

        var Service = scope.ServiceProvider.GetService<DataSeeder>();
        Service.SeedData();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
