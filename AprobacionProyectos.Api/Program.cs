using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Services;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces; 
using AprobacionProyectos.Infrastructure.Repositories.Implementations;
using AprobacionProyectos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AprobacionProyectos.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); ;

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString, sqlOptions =>
{
    sqlOptions.EnableRetryOnFailure();
    sqlOptions.MigrationsAssembly("AprobacionProyectos.Infrastructure");
}));

// Add services to the container

//repositorios (interfaces e implementaciones)
builder.Services.AddScoped<IApprovalRuleRepository, ApprovalRuleRepository>();
builder.Services.AddScoped<IApprovalStatusRepository, ApprovalStatusRepository>(); 
builder.Services.AddScoped<IApproverRoleRepository, ApproverRoleRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IProjectApprovalStepRepository, ProjectApprovalStepRepository>();
builder.Services.AddScoped<IProjectProposalRepository, ProjectProposalRepository>();
builder.Services.AddScoped<IProjectTypeRepository, ProjectTypeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//servicios de aplicacion
builder.Services.AddScoped<IApprovalStatusService, ApprovalStatusService>();
builder.Services.AddScoped<IApprovalWorkflowService, ApprovalWorkflowService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IProjectProposalCreatorService, ProjectProposalCreatorService>();
builder.Services.AddScoped<IProjectProposalQueryService, ProjectProposalQueryService>();
builder.Services.AddScoped<IProjectTypeService, ProjectTypeService>();
builder.Services.AddScoped<IUserService, UserService>();


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
