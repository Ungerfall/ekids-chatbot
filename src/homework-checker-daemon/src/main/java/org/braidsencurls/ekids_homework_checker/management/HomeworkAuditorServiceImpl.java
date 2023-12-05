package org.braidsencurls.ekids_homework_checker.management;

import lombok.AllArgsConstructor;
import org.apache.commons.lang3.StringUtils;
import org.braidsencurls.ekids_homework_checker.entities.Homework;
import org.braidsencurls.ekids_homework_checker.entities.OpenAIChatCompletionAudit;
import org.braidsencurls.ekids_homework_checker.gpt.OpenAIAuditRepository;
import org.springframework.stereotype.Service;

@Service
@AllArgsConstructor
public class HomeworkAuditorServiceImpl implements HomeworkAuditorService {

    protected OpenAIAuditRepository auditRepository;

    @Override
    public void saveAudit(Homework homework, String prompt, String errorMessage) {
        OpenAIChatCompletionAudit audit = new OpenAIChatCompletionAudit();

        if (StringUtils.isNotBlank(errorMessage)) {
            audit.setHomework(homework);
            audit.setErrorMessage(errorMessage);
            audit.setPrompt(prompt);
            auditRepository.save(audit);
        }
    }
}
