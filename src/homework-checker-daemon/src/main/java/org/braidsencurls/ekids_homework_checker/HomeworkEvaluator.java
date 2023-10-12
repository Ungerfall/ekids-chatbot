package org.braidsencurls.ekids_homework_checker;

import org.braidsencurls.ekids_homework_checker.entities.Homework;

import java.util.List;

public interface HomeworkEvaluator {
    List<EvaluationResponse> evaluate(String content, Homework homework);
}
