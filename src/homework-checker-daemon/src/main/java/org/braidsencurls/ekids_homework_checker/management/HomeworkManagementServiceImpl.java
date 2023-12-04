package org.braidsencurls.ekids_homework_checker.management;

import lombok.AllArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.braidsencurls.ekids_homework_checker.entities.HomeworkEvaluationResult;
import org.braidsencurls.ekids_homework_checker.entities.Homework;
import org.braidsencurls.ekids_homework_checker.exceptions.NoEntityFoundException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Slf4j
@Service
@AllArgsConstructor
@Transactional
public class HomeworkManagementServiceImpl implements HomeworkManagementService {

    private HomeworkRepository homeworkRepository;
    private HomeworkEvaluationResultRepository homeworkEvaluationResultRepository;

    @Override
    public Homework findByCode(String code) {
        return getHomeworkByCode(code);
    }

    @Override
    public void saveEvaluationResult(HomeworkEvaluationResult homeworkEvaluationResult) {
        homeworkEvaluationResultRepository.save(homeworkEvaluationResult);
    }

    private Homework getHomeworkByCode(String code) {
        Homework homework = homeworkRepository.findByCode(code).orElseThrow(() ->
            new NoEntityFoundException("No Homework with code " + code + " was found"));
        return homework;
    }
}
