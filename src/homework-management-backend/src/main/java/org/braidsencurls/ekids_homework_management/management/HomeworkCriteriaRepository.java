package org.braidsencurls.ekids_homework_management.management;

import org.braidsencurls.ekids_homework_management.entities.HomeworkCriteria;
import org.springframework.data.repository.CrudRepository;

import java.util.Optional;
import java.util.UUID;

public interface HomeworkCriteriaRepository extends CrudRepository<HomeworkCriteria, UUID> {
    Optional<HomeworkCriteria> findByIdAndHomeworkId(UUID id, UUID homeworkId);
}
