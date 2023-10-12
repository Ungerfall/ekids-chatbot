package org.braidsencurls.ekids_homework_management.management;

import org.braidsencurls.ekids_homework_management.entities.Homework;
import org.mapstruct.Mapper;

@Mapper(componentModel = "spring")
public interface HomeworkMapper {

    HomeworkDTO toDTO(Homework homework);
    Homework toEntity(HomeworkDTO homeworkDTO);
}
