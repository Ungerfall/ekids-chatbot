package org.braidsencurls.ekids_homework_checker.entities;

import jakarta.persistence.*;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.data.annotation.CreatedBy;
import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.LastModifiedBy;
import org.springframework.data.annotation.LastModifiedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

@Data
@Entity
@EntityListeners(AuditingEntityListener.class)
@Table(name = "homework")
@NoArgsConstructor
public class Homework {
    @Id
    @EqualsAndHashCode.Exclude
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    @Column(name = "id")
    private UUID id;

    @Column(name = "code", unique = true)
    private String code;

    @Column(name = "description")
    private String description;

    @Column(name = "deleted")
    private Boolean isDeleted;

    @CreatedDate
    @EqualsAndHashCode.Exclude
    @Column(name = "created_on", nullable = false, updatable = false)
    private LocalDateTime created;

    @CreatedBy
    @EqualsAndHashCode.Exclude
    @Column(name = "created_by", nullable = false, updatable = false)
    private String createdBy;

    @LastModifiedDate
    @EqualsAndHashCode.Exclude
    @Column(name = "modified_on", nullable = false)
    private LocalDateTime modified;

    @LastModifiedBy
    @EqualsAndHashCode.Exclude
    @Column(name = "modified_by", nullable = false)
    private String modifiedBy;

    @OneToMany(mappedBy = "homework", fetch = FetchType.LAZY)
    private List<HomeworkCriteria> criteria;
}
