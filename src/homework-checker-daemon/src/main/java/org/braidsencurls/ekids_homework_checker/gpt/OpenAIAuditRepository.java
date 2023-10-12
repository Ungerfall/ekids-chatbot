package org.braidsencurls.ekids_homework_checker.gpt;

import org.braidsencurls.ekids_homework_checker.entities.OpenAIChatCompletionAudit;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.util.UUID;

@Repository
public interface OpenAIAuditRepository extends CrudRepository<OpenAIChatCompletionAudit, UUID> {
}
