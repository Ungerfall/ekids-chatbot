package org.braidsencurls.ekids_homework_management.management;

import org.braidsencurls.ekids_homework_management.entities.Homework;
import org.springframework.data.repository.CrudRepository;

import java.util.Optional;
import java.util.UUID;

public interface HomeworkRepository extends CrudRepository<Homework, UUID> {
    Optional<Homework> findByIdAndIsDeletedIsFalse(UUID id);
    Optional<Homework> findByCode(String code);
}
