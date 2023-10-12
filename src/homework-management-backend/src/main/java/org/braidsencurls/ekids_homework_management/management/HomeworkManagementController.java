package org.braidsencurls.ekids_homework_management.management;

import jakarta.validation.Valid;
import lombok.AllArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@AllArgsConstructor
@RestController
@RequestMapping("/homework")
public class HomeworkManagementController {

    private HomeworkManagementService homeworkManagementService;

    @PostMapping
    public ResponseEntity<HomeworkDTO> saveHomework(@RequestBody @Valid HomeworkDTO homework) {
        HomeworkDTO newHomework = homeworkManagementService.saveHomework(homework);
        return ResponseEntity.ok().body(newHomework);
    }

    @PutMapping("/{id}/criteria")
    public ResponseEntity<List<HomeworkCriteriaDTO>> saveCriteria(@PathVariable String id, @RequestBody @Valid List<HomeworkCriteriaDTO> criteria) {
        List<HomeworkCriteriaDTO> newCriteria = homeworkManagementService.saveCriteria(id, criteria);
        return ResponseEntity.ok().body(newCriteria);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<String> deleteHomework(@PathVariable String id) {
        homeworkManagementService.deleteHomework(id);
        return ResponseEntity.ok().body("Successfully deleted a homework with id " + id);
    }

    @GetMapping("/{id}")
    public ResponseEntity<HomeworkDTO> findHomeworkById(@PathVariable String id) {
        HomeworkDTO homework = homeworkManagementService.findById(id);
        return ResponseEntity.ok().body(homework);
    }

    @DeleteMapping("/{homeworkId}/criteria/{criteriaId}")
    public ResponseEntity<String> deleteCriteria(@PathVariable String homeworkId,
                                           @PathVariable String criteriaId) {
        homeworkManagementService.deleteCriteria(homeworkId, criteriaId);
        return ResponseEntity.ok().body("Successfully deleted criteria");
    }

}
