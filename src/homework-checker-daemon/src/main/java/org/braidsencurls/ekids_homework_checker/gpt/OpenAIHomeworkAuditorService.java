package org.braidsencurls.ekids_homework_checker.gpt;

import com.theokanning.openai.completion.chat.ChatCompletionResult;
import org.braidsencurls.ekids_homework_checker.entities.Homework;
import org.braidsencurls.ekids_homework_checker.management.HomeworkAuditorService;

public interface OpenAIHomeworkAuditorService extends HomeworkAuditorService {
    void saveAudit(Homework homework, ChatCompletionResult result, String prompt);
}
