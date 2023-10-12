package org.braidsencurls.ekids_homework_management.management;

import org.braidsencurls.ekids_homework_management.entities.HomeworkCriteria;
import org.mapstruct.Mapper;

import java.util.List;

@Mapper(componentModel = "spring")
public interface HomeworkCriteriaMapper {
    List<HomeworkCriteria> toEntities(List<HomeworkCriteriaDTO> homeworks);

    List<HomeworkCriteriaDTO> toDTOs(List<HomeworkCriteria> homeworks);
}
