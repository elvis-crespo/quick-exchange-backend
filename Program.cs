using Microsoft.OpenApi.Models;
using quick_exchange_backend.Services;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options => {
    //options.AddSecurityDefinition("oauth2", new openapisec) te lo completa
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddScoped<IAuthService, AuthService>();

//Cors
var allowOriginsCors = "allowOriginsCors";

builder.Services.AddCors(opt =>
    opt.AddPolicy(name: allowOriginsCors,
        policy => {
            policy.AllowAnyHeader().
            AllowAnyOrigin().
            AllowAnyMethod();
        })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(allowOriginsCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
