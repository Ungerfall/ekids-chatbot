using AutoMapper;
using EKids.Chatbot.Homeworks.Application.Models;
using EKids.Chatbot.Homeworks.WebApi.ViewModels;

namespace EKids.Chatbot.Homeworks.WebApi.Mapper;

public class HomeworkProfile : Profile
{
    public HomeworkProfile()
    {
        CreateMap<HomeworkModel, HomeworkViewModel>().ReverseMap();
    }
}
