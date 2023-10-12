package org.braidsencurls.ekids_homework_checker.entities;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
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
import java.util.UUID;

@AllArgsConstructor
@Data
@Entity
@EntityListeners(AuditingEntityListener.class)
@Table(name = "homework_criteria")
@NoArgsConstructor
public class HomeworkCriteria {

    @Id
    @EqualsAndHashCode.Exclude
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    @Column(name = "id")
    private UUID id;

    @ManyToOne
    @JoinColumn(name = "homework_id")
    private Homework homework;

    @Column(name = "detail")
    private String detail;

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

    @Override
    public String toString() {
        return "HomeworkCriteria{" + "id=" + id + ", detail='" + detail + '\'' + ", created=" + created + ", createdBy='" + createdBy + '\'' + ", modified=" + modified + ", modifiedBy='" + modifiedBy + '\'' + '}';
    }
}
