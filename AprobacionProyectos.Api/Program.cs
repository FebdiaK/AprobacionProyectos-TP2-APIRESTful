using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Services;
using AprobacionProyectos.Application.Interfaces.ServicesInterfaces;
using AprobacionProyectos.Application.Interfaces.PersistenceInterfaces;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Repositories.Implementations;
using AprobacionProyectos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using AprobacionProyectos.Application.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AprobacionProyectos.Api;
using Microsoft.AspNetCore.Http;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ValidateModelAttribute>();
    })
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Continue; //para que las validaciones de las entidades se hagan de manera cascada

builder.Services.AddValidatorsFromAssemblyContaining<ProjectCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ProjectDecisionValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ProjectQueryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ProjectUpdateValidator>();



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
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//servicios de aplicacion
builder.Services.AddScoped<IApprovalStatusService, ApprovalStatusService>();
builder.Services.AddScoped<IApprovalWorkflowService, ApprovalWorkflowService>();
builder.Services.AddScoped<IApproverRoleService, ApproverRoleService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IProjectProposalCreatorService, ProjectProposalCreatorService>();
builder.Services.AddScoped<IProjectProposalQueryService, ProjectProposalQueryService>();
builder.Services.AddScoped<IProjectProposalUpdateService, ProjectProposalUpdateService>();
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
