using BusinessCard.core.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);
var ConnectionString = builder.Configuration.GetConnectionString("BusinessCardDbConnectionString");
if (ConnectionString == null)
{
    // Handle the case where the connection string is not found
    Console.WriteLine("Connection string not found.");
}
else
{
    // Attempt to open a connection to the database
    using (var connection = new SqlConnection(ConnectionString))
    {
        try
        {
            connection.Open();
            Console.WriteLine("Connection opened successfully.");
            // Connection is open, you can perform your database operations here

            // Always close the connection when done
            connection.Close();
        }
        catch (Exception ex)
        {
            // Handle any errors that occur when trying to open the connection
            Console.WriteLine("An error occurred while trying to open the connection: " + ex.Message);
        }
    }
}
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<BusinessCardDbContext>(options =>
    options.UseSqlServer(ConnectionString));



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
