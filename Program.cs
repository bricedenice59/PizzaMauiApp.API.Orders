using MassTransit;
using PizzaMauiApp.API.Shared.Environment;

var builder = WebApplication.CreateBuilder(args);

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
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host($"rabbitmq://{rabbitMqConnectionConfig.Host}:{rabbitMqConnectionConfig.Port}", hostconfig =>
        {
            hostconfig.Username(rabbitMqConnectionConfig.Username);
            hostconfig.Password(rabbitMqConnectionConfig.Password);
        });
       cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddOptions<MassTransitHostOptions>().Configure(options =>
    {
        options.WaitUntilStarted = true;
    });


// Add services to the container.
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