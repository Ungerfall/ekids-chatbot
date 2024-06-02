using EKids.Chatbot.Homeworks.Application.Interfaces;
using EKids.Chatbot.Homeworks.Application.Mapper;
using EKids.Chatbot.Homeworks.Application.Models;
using EKids.Chatbot.Homeworks.Core.Entities;
using EKids.Chatbot.Homeworks.Core.Repositories.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EKids.Chatbot.Homeworks.Application.Services;

// TODO : add validation , authorization, logging, exception handling etc. -- cross cutting activities in here.
public class HomeworkService(IRepository<Homework> repository, ILogger<HomeworkService> logger) : IHomeworksService
{
    public async Task<IEnumerable<HomeworkModel>> GetHomeworkList()
    {
        var homeworks = await repository.GetAllAsync();
        return ObjectMapper.Mapper.Map<IEnumerable<HomeworkModel>>(homeworks);
    }

    public async Task<HomeworkModel?> GetHomeworkById(Guid homeworkId)
    {
        Homework? homework = await repository.GetByIdAsync(homeworkId);
        return ObjectMapper.Mapper.Map<HomeworkModel?>(homework);
    }

    public async Task<HomeworkModel> Create(HomeworkModel homeworkModel)
    {
        await ValidateHomeworkIfExist(homeworkModel);

        Homework? mappedEntity = ObjectMapper.Mapper.Map<Homework?>(homeworkModel)
            ?? throw new ApplicationException("Entity could not be mapped.");
        var newEntity = await repository.AddAsync(mappedEntity);
        logger.LogInformation("Entity successfully added - HomeworkService");

        return ObjectMapper.Mapper.Map<HomeworkModel>(newEntity);
    }

    public async Task Update(HomeworkModel productModel)
    {
        await ValidateHomeworkIfNotExist(productModel);

        Homework editProduct = await repository.GetByIdAsync(productModel.Id)
            ?? throw new ApplicationException("Entity could not be loaded.");
        ObjectMapper.Mapper.Map(source: productModel, destination: editProduct);

        await repository.UpdateAsync(editProduct);
        logger.LogInformation("Entity successfully updated - HomeworkService");
    }

    public async Task Delete(HomeworkModel productModel)
    {
        await ValidateHomeworkIfNotExist(productModel);
        Homework deletedProduct = await repository.GetByIdAsync(productModel.Id)
            ?? throw new ApplicationException("Entity could not be loaded.");

        await repository.DeleteAsync(deletedProduct);
        logger.LogInformation("Entity successfully deleted - HomeworkService");
    }

    private async Task ValidateHomeworkIfExist(HomeworkModel homeworkModel)
    {
        Homework? existingEntity = await repository.GetByIdAsync(homeworkModel.Id);
        if (existingEntity is not null)
        {
            throw new ApplicationException($"{homeworkModel} with this id already exists");
        }
    }

    private async Task ValidateHomeworkIfNotExist(HomeworkModel homeworkModel)
    {
        Homework? existingEntity = await repository.GetByIdAsync(homeworkModel.Id);
        if (existingEntity is null)
        {
            throw new ApplicationException($"{homeworkModel} with this id is not exists");
        }
    }
}
