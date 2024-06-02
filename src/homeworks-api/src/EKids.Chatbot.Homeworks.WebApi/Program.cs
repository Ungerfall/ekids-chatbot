using AutoMapper;
using EKids.Chatbot.Homeworks.Application.Interfaces;
using EKids.Chatbot.Homeworks.Application.Mapper;
using EKids.Chatbot.Homeworks.Application.Models;
using EKids.Chatbot.Homeworks.Application.Services;
using EKids.Chatbot.Homeworks.Core.Configuration;
using EKids.Chatbot.Homeworks.Core.Entities;
using EKids.Chatbot.Homeworks.Core.Repositories.Base;
using EKids.Chatbot.Homeworks.Infrastructure.Data;
using EKids.Chatbot.Homeworks.Infrastructure.Repository;
using EKids.Chatbot.Homeworks.Infrastructure.Repository.Base;
using EKids.Chatbot.Homeworks.WebApi.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EKids.Chatbot.Homeworks.WebApi;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        builder.Services.Configure<HomeworkSettings>(builder.Configuration);
        builder.Services.AddDbContext<HomeworkContext>(c =>
            c.UseSqlServer(builder.Configuration.GetConnectionString("Homeworks")));
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IRepository<Homework>, HomeworkRepository>();
        builder.Services.AddScoped<IHomeworksService, HomeworkService>();
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();
        app.UseDeveloperExceptionPage();
        // seed db
        var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
        var dbContext = app.Services.GetRequiredService<HomeworkContext>();
        await HomeworkContextSeed.SeedAsync(dbContext, loggerFactory);

        var homeworkApi = app.MapGroup("/homeworks");
        homeworkApi.MapGet("/", (IHomeworksService service)
            => service.GetHomeworkList());
        homeworkApi.MapGet("/{homeworkId}", (Guid homeworkId, IHomeworksService service)
                => service.GetHomeworkById(homeworkId));
        homeworkApi.MapPost("/", async (HomeworkViewModel vm, IHomeworksService service) =>
        {
            HomeworkModel homeworkModel = ObjectMapper.Mapper.Map<HomeworkModel>(vm);
            var created = await service.Create(homeworkModel);
            return Results.Accepted(value: created);
        });
        homeworkApi.MapPatch("/", async (HomeworkViewModel vm, IHomeworksService service) =>
        {
            HomeworkModel homeworkModel = ObjectMapper.Mapper.Map<HomeworkModel>(vm);
            await service.Update(homeworkModel);
        });
        homeworkApi.MapDelete("/", async (HomeworkViewModel vm, IHomeworksService service) =>
        {
            HomeworkModel homeworkModel = ObjectMapper.Mapper.Map<HomeworkModel>(vm);
            await service.Delete(homeworkModel);
        });

        app.Run();
    }
}
