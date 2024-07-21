using AutoMapper;
using EKids.Chatbot.Homeworks.Application.Models;
using EKids.Chatbot.Homeworks.Core.Entities;
using System;

namespace EKids.Chatbot.Homeworks.Application.Mapper;

// The best implementation of AutoMapper for class libraries -> https://www.abhith.net/blog/using-automapper-in-a-net-core-class-library/
public static class ObjectMapper
{
    private static readonly Lazy<IMapper> Lazy = new(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            // This line ensures that internal properties are also mapped over.
            cfg.ShouldMapProperty = p => (p?.GetMethod?.IsPublic ?? false) || (p?.GetMethod?.IsAssembly ?? false);
            cfg.CreateMap<Homework, HomeworkModel>()
                .ReverseMap();
        });
        return config.CreateMapper();
    });
    public static IMapper Mapper => Lazy.Value;
}
