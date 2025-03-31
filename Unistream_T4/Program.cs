using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using Unistream_T4.Dto;
using Unistream_T4.Exceptions;
using Unistream_T4.Mappings;
using Unistream_T4.Models.Transactions;
using Unistream_T4.Repositories;
using Unistream_T4.Repositories.Abstractions;
using Unistream_T4.Services;
using Unistream_T4.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TransactionDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddScoped<ITransactionService,TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));

var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new TransactionProfile()));
var executionPlan = configuration.BuildExecutionPlan(typeof(TransactionDto), typeof(Transaction));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
