using System.Text;
using GravityFs;
using GravityFs.Models;
using GravityFs.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// db
builder.Services.AddDbContext<GravityfsdbContext>();

// services
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddSingleton<IAtchmnflGroupService, AtchmnflGroupService>();
builder.Services.AddSingleton<IAtchmnflService, AtchmnflService>();

// nlog
builder.Logging.AddNLog(new NLogAspNetCoreOptions
{
    ShutdownOnDispose = true
});

// quartz
// @see https://www.quartz-scheduler.net/documentation/quartz-3.x/packages/aspnet-core-integration.html
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("job1");
    q.AddJob<DeleteOldDeletedFilesJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("trigger1")
        .WithCronSchedule("0 */1 * * * ?")); // 1 min
});
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

//
builder.WebHost.UseUrls([$"https://*:{config.GetValue<int>("App:httpsPort")}", $"http://*:{config.GetValue<int>("App:httpPort")}"]);

//
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        // options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config.GetValue<string>("App:Issuer"),
            ValidAudience = config.GetValue<string>("App:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("App:SecretKey"))),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy => policy.RequireClaim("Admin"))
    .AddPolicy("User", policy => policy.RequireClaim("User"));

//
// builder.Host.ConfigureWebHostDefaults(webBuilder =>
// {
//     webBuilder.UseStartup<Startup>().UseKestrel(options =>
//     {
//         options.ListenAnyIP(config.GetValue<int>("App:httpPort"));
//         options.ListenAnyIP(config.GetValue<int>("App:httpsPort"), listenOptions =>
//         {
//             listenOptions.UseHttps("certificate.pfx", "password");
//         });
//     });
// });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.UsePathBase(config.GetValue<string>("App:ContextPath"));



app.Run();
