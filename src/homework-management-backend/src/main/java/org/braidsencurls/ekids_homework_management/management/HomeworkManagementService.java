package org.braidsencurls.ekids_homework_management.management;

import java.util.List;

public interface HomeworkManagementService {

    HomeworkDTO saveHomework(HomeworkDTO homework);
    List<HomeworkCriteriaDTO> saveCriteria(String homeworkId, List<HomeworkCriteriaDTO> criteria);
    void deleteHomework(String id);
    HomeworkDTO findById(String id);
    void deleteCriteria(String homeworkId, String criteriaId);
}
