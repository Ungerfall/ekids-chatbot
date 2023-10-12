package org.braidsencurls.ekids_homework_checker.management;

import org.braidsencurls.ekids_homework_checker.entities.Homework;
import org.braidsencurls.ekids_homework_checker.entities.HomeworkEvaluationResult;

public interface HomeworkManagementService {
    Homework findByCode(String code);
    void saveEvaluationResult(HomeworkEvaluationResult homeworkEvaluationResult);

}
