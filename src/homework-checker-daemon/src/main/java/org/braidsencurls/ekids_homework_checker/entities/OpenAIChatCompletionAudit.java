package org.braidsencurls.ekids_homework_checker.entities;

import jakarta.persistence.*;
import lombok.Data;
import lombok.EqualsAndHashCode;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.data.annotation.CreatedBy;
import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import java.time.LocalDateTime;
import java.util.UUID;

@Data
@Entity
@EntityListeners(AuditingEntityListener.class)
@Table(name = "openai_chat_completion_audit")
public class OpenAIChatCompletionAudit {

    @Id
    @EqualsAndHashCode.Exclude
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    @Column(name = "id")
    private UUID id;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "homework_id", nullable = false)
    private Homework homework;

    @Column(name = "prompt", columnDefinition="TEXT", nullable = false)
    @Lob
    private String  prompt;

    @Column(name = "model")
    private String model;

    @Column(name = "finish_reason")
    private String finishReason;

    @Column(name = "prompt_token")
    private long promptToken;

    @Column(name = "completion_token")
    private long completionToken;

    @Column(name = "total_token")
    private long totalToken;

    @Column(name = "error_message")
    private String errorMessage;

    @CreatedDate
    @EqualsAndHashCode.Exclude
    @Column(name = "created_on", nullable = false, updatable = false)
    private LocalDateTime created;

    @CreatedBy
    @EqualsAndHashCode.Exclude
    @Column(name = "created_by", nullable = false, updatable = false)
    private String createdBy;
}
