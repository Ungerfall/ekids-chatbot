package org.braidsencurls.ekids_homework_checker.management;

import org.braidsencurls.ekids_homework_checker.entities.HomeworkEvaluationResult;
import org.springframework.data.repository.CrudRepository;

import java.util.UUID;

public interface HomeworkEvaluationResultRepository extends CrudRepository<HomeworkEvaluationResult, UUID> {
}
