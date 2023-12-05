package org.braidsencurls.ekids_homework_checker.gpt;

import com.theokanning.openai.Usage;
import com.theokanning.openai.completion.chat.ChatCompletionChoice;
import com.theokanning.openai.completion.chat.ChatCompletionResult;
import org.braidsencurls.ekids_homework_checker.entities.Homework;
import org.braidsencurls.ekids_homework_checker.entities.OpenAIChatCompletionAudit;
import org.braidsencurls.ekids_homework_checker.management.HomeworkAuditorServiceImpl;
import org.springframework.stereotype.Service;

@Service
public class OpenAIHomeworkAuditorServiceImpl extends HomeworkAuditorServiceImpl implements OpenAIHomeworkAuditorService {

    public OpenAIHomeworkAuditorServiceImpl(OpenAIAuditRepository auditRepository) {
        super(auditRepository);
    }

    @Override
    public void saveAudit(Homework homework, ChatCompletionResult result, String prompt) {
        OpenAIChatCompletionAudit audit = new OpenAIChatCompletionAudit();

        ChatCompletionChoice chatCompletionChoice = result.getChoices().get(0);
        Usage usage = result.getUsage();

        audit.setHomework(homework);
        audit.setModel(result.getModel());
        audit.setPrompt(prompt);
        audit.setFinishReason(chatCompletionChoice.getFinishReason());
        audit.setPromptToken(usage.getPromptTokens());
        audit.setCompletionToken(usage.getCompletionTokens());
        audit.setTotalToken(usage.getTotalTokens());

        auditRepository.save(audit);
    }
}
