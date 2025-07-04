using FluentMigrator.Runner;
using News.Business;
using News.Business.Interfaces;
using News.Repositories;
using News.Repositories.Models;
using News.Repositories.Interfaces;
using Serilog;
using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Net.Mail;
using News.Business.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false);

//builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
builder.Services.Configure<Mail>(builder.Configuration.GetSection("EmailSettings"));

// Add services to the container.
builder.Services.AddScoped<INewsItemsRepository, NewsItemsRepository>();
builder.Services.AddScoped<ILanguageRepository, LanguagesRepository>();
builder.Services.AddScoped<ISourceRepository, SourcesRepository>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<ISourceRepository, SourcesRepository>();
builder.Services.AddScoped<IMailerService, MailerService>();

builder.Services.AddScoped<INewsItemsService, NewsItemService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISourcesService, SourcesService>();

builder.Services.AddHostedService<FeedService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services
    .AddFluentMigratorCore()
    .ConfigureRunner(rb =>
    {
        rb.AddSQLite()
        .WithGlobalConnectionString("Data Source=mydatabase.db;Version=3;")
        .ScanIn(typeof(AddNewsTableMigration).Assembly).For.Migrations();
    });

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Console()
                .WriteTo.File("logs/MyAppLog.txt")
                .CreateLogger();

builder.Host.UseSerilog();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c =>
{
    c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
