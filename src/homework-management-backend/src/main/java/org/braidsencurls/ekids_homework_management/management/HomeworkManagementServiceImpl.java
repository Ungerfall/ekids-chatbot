package org.braidsencurls.ekids_homework_management.management;

import lombok.AllArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.braidsencurls.ekids_homework_management.entities.Homework;
import org.braidsencurls.ekids_homework_management.entities.HomeworkCriteria;
import org.braidsencurls.ekids_homework_management.exceptions.NoEntityFoundException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@Slf4j
@Service
@AllArgsConstructor
@Transactional
public class HomeworkManagementServiceImpl implements HomeworkManagementService {

    private HomeworkRepository homeworkRepository;
    private HomeworkCriteriaRepository criteriaRepository;
    private HomeworkMapper homeworkMapper;
    private HomeworkCriteriaMapper criteriaMapper;

    @Override
    public HomeworkDTO saveHomework(HomeworkDTO homework) {
        log.debug("Attempting to save a homework " + homework.toString());
        Homework homeworkEntity = homeworkMapper.toEntity(homework);
        homeworkEntity.setIsDeleted(false);
        Homework newHomework = homeworkRepository.save(homeworkEntity);
        log.debug("Homework has been successfully saved");
        return homeworkMapper.toDTO(newHomework);
    }

    @Override
    public List<HomeworkCriteriaDTO> saveCriteria(String homeworkId, List<HomeworkCriteriaDTO> criteria) {
        log.debug("Attempting to add criteria for homework {}", homeworkId);
        Homework homework = getActiveHomework(homeworkId);
        List<HomeworkCriteria> newCriteria = saveCriteria(homework, criteriaMapper.toEntities(criteria));
        log.debug("Criteria has been successfully added");
        return criteriaMapper.toDTOs(newCriteria);
    }

    private List<HomeworkCriteria> saveCriteria(Homework homework, List<HomeworkCriteria> criteria) {
        List<HomeworkCriteria> savedCriteria = new ArrayList<>();
        criteria.forEach(k-> {
            k.setHomework(homework);
            savedCriteria.add(criteriaRepository.save(k));
        });
        return savedCriteria;
    }

    @Override
    public void deleteHomework(String id) {
        log.debug("Attempting to delete a homework with id {}", id);
        Homework homework = getActiveHomework(id);
        homework.setIsDeleted(true);
        homeworkRepository.save(homework);
        log.debug("Successfully deleted homework with id {}", id);
    }

    @Override
    public HomeworkDTO findById(String id) {
        log.debug("Attempting to find a homework with id {}", id);
        Homework homework = getActiveHomework(id);
        log.debug("Homework with id {} has been found!", id);
        return homeworkMapper.toDTO(homework);
    }

    @Override
    public void deleteCriteria(String homeworkId, String criteriaId) {
        HomeworkCriteria criteria = getActiveCriteriaByIdAndHomeworkId(homeworkId, criteriaId);
        criteriaRepository.delete(criteria);
    }

    private HomeworkCriteria getActiveCriteriaByIdAndHomeworkId(String homeworkId, String criteriaId) {
        return criteriaRepository.findByIdAndHomeworkId(UUID.fromString(criteriaId), UUID.fromString(homeworkId))
                .orElseThrow(() -> new NoEntityFoundException("No criteria found with id " + criteriaId));
    }

    private Homework getActiveHomework(String id) {
        log.debug("Looking for homework entity with id {}", id);
        return homeworkRepository.findByIdAndIsDeletedIsFalse(UUID.fromString(id))
                .orElseThrow(() -> new NoEntityFoundException("No Homework with id " + id + " was found"));
    }
}
