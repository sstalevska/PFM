using Microsoft.EntityFrameworkCore;
using Npgsql;
using PFM.Database;
using PFM.Database.Repositories;
using PFM.Services;
using System.Reflection;
using System.Text.Json.Serialization;
using TransactionDbContext = PFM.Database.TransactionDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


builder.Services.AddScoped<ISplitRepository, SplitRepository>();
builder.Services.AddScoped<ISplitService, SplitService>();

builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICSVService, CSVService>();
builder.Services.AddScoped<ITransactionCSVService, TransactionCSVService>();





// AutoMapper definition
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter(System.Text.Json.JsonNamingPolicy.CamelCase)
        );
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DB Context registration
builder.Services.AddDbContext<CategoryDbContext>(opt =>
{
    opt.UseNpgsql(CreateConnectionString(builder.Configuration));
}
);
builder.Services.AddDbContext<SplitDbContext>(opt =>
{
    opt.UseNpgsql(CreateConnectionString(builder.Configuration));
}
);
builder.Services.AddDbContext<TransactionDbContext>(opt =>
{
    opt.UseNpgsql(CreateConnectionString(builder.Configuration));
}
);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
    scope.ServiceProvider.GetRequiredService<CategoryDbContext>().Database.Migrate();
    scope.ServiceProvider.GetRequiredService<SplitDbContext>().Database.Migrate();
    scope.ServiceProvider.GetRequiredService<TransactionDbContext>().Database.Migrate();
}

app.UseAuthorization();

app.MapControllers();

app.Run();



string CreateConnectionString(IConfiguration configuration)
{

    var username = Environment.GetEnvironmentVariable("DATABASE_USERNAME") ?? "postgres";
    var pass = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "sara";
    var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "pfm";
    var host = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "localhost";
    var port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";

    var connBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = host,
        Port = int.Parse(port),
        Username = username,
        Database = databaseName,
        Password = pass,
        Pooling = true
    };
    return connBuilder.ConnectionString;
}