package org.braidsencurls.ekids_homework_checker;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import lombok.AllArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.braidsencurls.ekids_homework_checker.entities.Homework;
import org.braidsencurls.ekids_homework_checker.entities.HomeworkEvaluationResult;
import org.braidsencurls.ekids_homework_checker.exceptions.ClientException;
import org.braidsencurls.ekids_homework_checker.management.HomeworkManagementService;
import org.braidsencurls.ekids_homework_checker.utilities.FileHelper;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Component;
import org.springframework.util.CollectionUtils;

import java.io.InputStream;
import java.util.List;
import java.util.stream.Collectors;

@AllArgsConstructor
@Component
@Slf4j
public class HomeworkProcessor {

    public static final String PROJECT_JSON = "project.json";
    private HomeworkEvaluator homeworkEvaluator;
    private HomeworkManagementService homeworkManagementService;
    private ObjectMapper objectMapper;

    @Value("#{'${scratch.irrelevant.jsonPath}'.split(',')}")
    private List<String> irrelevantPaths;

    public void process(String filename, InputStream inputStream) throws JsonProcessingException {
        log.info("Processing homework with filename {}", filename);

        String content = getContent(inputStream);
        String homeworkCode = filename.split("_")[0];

        Homework homework = homeworkManagementService.findByCode(homeworkCode);
        List<EvaluationResponse> evaluations;
        try {
            evaluations = homeworkEvaluator.evaluate(content, homework);
            saveResult(filename, homework, evaluations);
        } catch (ClientException e) {
            saveResult(filename, homework, null);
            throw e;
        }
    }

    private String getContent(InputStream inputStream) {
        String content = FileHelper.readFileFromZip(inputStream, PROJECT_JSON);
        if(!CollectionUtils.isEmpty(irrelevantPaths)) {
            content = FileHelper.removeJsonPaths(content, irrelevantPaths);
        }
        return content;
    }

    private void saveResult(String filename, Homework homework, List<EvaluationResponse> evaluations) throws JsonProcessingException {
        log.info("Saving the result to the database: Filename: {}, Evaluation Result: {}",
                filename, evaluations);
        HomeworkEvaluationResult result = new HomeworkEvaluationResult();
        String remarks;

        if(CollectionUtils.isEmpty(evaluations)) {
            remarks = "Failed to evaluate homework";
        } else {
            List<Boolean> isSuccess = evaluations.stream().map(EvaluationResponse::isSuccess)
                    .collect(Collectors.toList());
            remarks = isSuccess.contains(false) ? "Not all criteria has been met" :
                    "All Criteria has been met";
        }

        String studentName = filename.split("_")[1].split("\\.")[0];

        result.setStudentName(studentName);
        result.setFileReference(filename);
        result.setEvaluation(objectMapper.writeValueAsString(evaluations));
        result.setHomework(homework);
        result.setRemarks(remarks);
        homeworkManagementService.saveEvaluationResult(result);
    }
}
