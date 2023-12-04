package org.braidsencurls.ekids_homework_checker.entities;

import jakarta.persistence.*;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.data.annotation.CreatedBy;
import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import java.time.LocalDateTime;
import java.util.UUID;

@Data
@Entity
@EntityListeners(AuditingEntityListener.class)
@NoArgsConstructor
@Table(name = "homework_evaluation_result")
public class HomeworkEvaluationResult {

    @Id
    @EqualsAndHashCode.Exclude
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    @Column(name = "id")
    private UUID id;

    @Column(name = "evaluation", columnDefinition = "TEXT")
    private String evaluation;

    @Column(name = "remarks")
    private String remarks;

    @Column(name = "student_name")
    private String studentName;

    @Column(name = "file_reference")
    private String fileReference;

    @ManyToOne
    @JoinColumn(name = "homework_id")
    private Homework homework;

    @CreatedDate
    @EqualsAndHashCode.Exclude
    @Column(name = "created_on", nullable = false, updatable = false)
    private LocalDateTime created;

    @CreatedBy
    @EqualsAndHashCode.Exclude
    @Column(name = "created_by", nullable = false, updatable = false)
    private String createdBy;

}
