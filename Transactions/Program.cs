using AutoMapper;
using GreenPipes;
using Invocation.MassTransit;
using MassTransit;
using Serilog;
using Transactions.Common;
using Transactions.Configuration;
using Transactions.DataAccess;
using Transactions.DataAccess.Contracts;
using Transactions.Service;
using Transactions.Service.Contracts;

var builder = WebApplication.CreateBuilder(args);

var environmentName = builder.Configuration.GetSection("ASPNETCORE_ENVIRONMENT").Value;

var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();
var queueSettings = configuration.GetSection(Constants.QUEUE_SETTINGS);
var options = queueSettings.Get<QueueSettings>();

Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
builder.Services.AddLogging(configure => configure
    .AddSerilog()
    .AddConfiguration(configuration.GetSection("Logging")));

builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddSingleton<IQueueMessageService, QueueMessageService>();
builder.Services.AddSingleton<IAppConfigurationService, AppConfigurationService>();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<TransactionCreationResponseQueue>();
    config.AddBus((sp) =>
    {
        return Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(new Uri(options.Address), h =>
            {
                h.Username(options.Username);
                h.Password(options.Password);

                //if (options.UseSSL == true)
                //{
                //    h.UseSsl(ssl =>
                //    {
                //        ssl.Protocol = System.Security.Authentication.SslProtocols.Tls12;
                //        ssl.CertificatePath = options.CertificatePath;
                //        ssl.CertificatePassphrase = options.CertificatePassphrase;
                //        ssl.ServerName = options.ServerName;
                //    });
                //}
            });
            // define retry policy
            cfg.UseMessageRetry(r =>
            {
                r.Interval(1, TimeSpan.FromSeconds(3));
            });

            cfg.ConfigureJsonDeserializer(settings =>
            {
                settings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include;
                return settings;
            });

            //define the receive endpoint
            cfg.ReceiveEndpoint(options.TransactionInsertProcessQueueSettings.ResponseQueue, ep =>
            {
                //link the queue to the Consumer Class
                ep.Consumer<TransactionCreationResponseQueue>(sp);
            });
        });
    });
});

builder.Services.AddMassTransitHostedService();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperConfig());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

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
