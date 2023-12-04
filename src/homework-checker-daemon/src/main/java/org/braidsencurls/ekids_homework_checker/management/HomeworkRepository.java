package org.braidsencurls.ekids_homework_checker.management;

import org.braidsencurls.ekids_homework_checker.entities.Homework;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;

import java.util.Optional;
import java.util.UUID;

public interface HomeworkRepository extends CrudRepository<Homework, UUID> {
    @Query("SELECT h FROM Homework h JOIN FETCH h.criteria where h.code = :code")
    Optional<Homework> findByCode(String code);
}
