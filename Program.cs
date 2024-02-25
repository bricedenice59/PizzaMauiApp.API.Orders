using MassTransit;
using Microsoft.EntityFrameworkCore;
using PizzaMauiApp.API.Core.Environment;
using PizzaMauiApp.API.Orders.Configuration;
using PizzaMauiApp.API.Orders.Consumers;
using PizzaMauiApp.API.Orders.DbOrders;
using PizzaMauiApp.API.Orders.Services;


var builder = WebApplication.CreateBuilder(args);

//decode configuration environment variables;
#if DEBUG
var dbOrderConnectionConfig = new DbConnectionConfig(builder.Configuration, "Order");
#else
    var dbOrderConnectionConfig = new DbConnectionConfig("order_db");
#endif

//check if secrets data are correctly read and binded to object
ArgumentException.ThrowIfNullOrEmpty(dbOrderConnectionConfig.Host);
ArgumentException.ThrowIfNullOrEmpty(dbOrderConnectionConfig.Port);
ArgumentException.ThrowIfNullOrEmpty(dbOrderConnectionConfig.Username);
ArgumentException.ThrowIfNullOrEmpty(dbOrderConnectionConfig.Password);
ArgumentException.ThrowIfNullOrEmpty(dbOrderConnectionConfig.Database);

var connectionDbOrders = dbOrderConnectionConfig.ToString();

RabbitMQConfiguration rmqConfiguration = new();
var msgTTlFromConfig = builder.Configuration.GetSection("RabbitMQ")["OrderMessageTTL_In_Minutes"];
if (msgTTlFromConfig is not null && int.TryParse(msgTTlFromConfig, out int ttl))
    rmqConfiguration.MessageTTL = ttl;

//decode configuration environment variables;
var rabbitMqConnectionConfig = new DbConnectionConfig(builder.Configuration, "RabbitMq");
//check if secrets data are correctly read and binded to object
ArgumentException.ThrowIfNullOrEmpty(rabbitMqConnectionConfig.Host);
ArgumentException.ThrowIfNullOrEmpty(rabbitMqConnectionConfig.Port);
ArgumentException.ThrowIfNullOrEmpty(rabbitMqConnectionConfig.Username);
ArgumentException.ThrowIfNullOrEmpty(rabbitMqConnectionConfig.Password);

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<OrderConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host($"rabbitmq://{rabbitMqConnectionConfig.Host}:{rabbitMqConnectionConfig.Port}", hostconfig =>
        {
            hostconfig.Username(rabbitMqConnectionConfig.Username);
            hostconfig.Password(rabbitMqConnectionConfig.Password);
        });
        cfg.ReceiveEndpoint("order-kitchen-processed", z =>
        {
            z.ConfigureConsumer<OrderConsumer>(context);
            z.BindDeadLetterQueue("order-kitchen-processed-dead");
        });
    });
});

builder.Services.AddOptions<MassTransitHostOptions>().Configure(options =>
{
    options.WaitUntilStarted = true;
});

// Add services to the container.
builder.Services.AddSingleton<RabbitMQConfiguration>(rmqConfiguration);
builder.Services.AddScoped(typeof(OrdersDbRepository<>),typeof(OrdersDbRepository<>));
builder.Services.AddScoped<IOrderService, OrdersService>();

// Add Db Context options
builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseNpgsql(connectionDbOrders));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();