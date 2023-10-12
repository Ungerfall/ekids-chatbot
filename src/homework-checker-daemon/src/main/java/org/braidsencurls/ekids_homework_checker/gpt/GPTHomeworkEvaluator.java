package org.braidsencurls.ekids_homework_checker.gpt;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.theokanning.openai.Usage;
import com.theokanning.openai.completion.chat.*;
import com.theokanning.openai.service.FunctionExecutor;
import lombok.AllArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.apache.commons.lang3.StringUtils;
import org.braidsencurls.ekids_homework_checker.EvaluationResponse;
import org.braidsencurls.ekids_homework_checker.HomeworkEvaluator;
import org.braidsencurls.ekids_homework_checker.entities.Homework;
import org.braidsencurls.ekids_homework_checker.entities.HomeworkCriteria;
import org.braidsencurls.ekids_homework_checker.entities.OpenAIChatCompletionAudit;
import org.braidsencurls.ekids_homework_checker.exceptions.ClientException;
import org.braidsencurls.ekids_homework_checker.exceptions.ChatCompletionProcessingException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@AllArgsConstructor
@Service
@Transactional
@Slf4j
public class GPTHomeworkEvaluator implements HomeworkEvaluator {
    public static final String SYSTEM_ROLE = "system";
    public static final String SYSTEM_MESSAGE = "You are an assistant and your task is to evaluate SCRATCH programming homework. You will check if the project.json content meets the following criteria. In your JSON response, it should include a properties success = true, if the current criteria is met, criteria = the current criteria being checked and sprite = list of sprite names that met the current criteria. ";
    //public static final String SYSTEM_MESSAGE = "You are an assistant and your task is to evaluate a SCRATCH project if it meets the given criteria. For the time-being, mock the response to success is equals to true";
    public static final String USER_ROLE = "user";
    private OpenAIChatCompletionClient openAIChatCompletionClient;
    private OpenAIAuditRepository auditRepository;
    private ObjectMapper objectMapper;

    public  List<EvaluationResponse> evaluate(String content, Homework homework) {
        List<EvaluationResponse> evaluations = new ArrayList<>();
        List<ChatMessage> chatMessages = initializeChatMessages();
        List<ChatFunction> chatFunctions = List.of(new GPTEvaluatorFunction().getFunction());
        FunctionExecutor functionExecutor = new FunctionExecutor(chatFunctions);

        for(int ctr = 0; ctr < homework.getCriteria().size(); ctr++) {
            try {
                HomeworkCriteria criterion = homework.getCriteria().get(ctr);
                String message;
                if(ctr == 0) {
                    message = "project.json content: " + content + ". criteria: " + criterion.getDetail();
                } else {
                    message = "Using the same project.json content above, evaluate the criteria: " + criterion.getDetail();
                }
                chatMessages.add(getChatMessage(USER_ROLE, message));
                ChatCompletionResult result = askGpt(chatMessages, functionExecutor);
                log.debug(result.toString());

                ChatMessage responseMessage = result.getChoices().get(0).getMessage();
                chatMessages.add(responseMessage);
                evaluations.add(getEvaluation(functionExecutor, responseMessage));

                saveAudit(homework, result, chatMessages.get(0).getContent(), null);
            } catch (JsonProcessingException | RuntimeException e) {
                log.error("Evaluation failed!", e);
                saveAudit(homework, null, chatMessages.get(0).getContent(), e.getMessage());
                throw new ClientException(e.getMessage());
            }
        }
        return evaluations;
    }

    private void saveAudit(Homework homework, ChatCompletionResult result,
                           String prompt, String errorMessage) {
        OpenAIChatCompletionAudit audit = new OpenAIChatCompletionAudit();

        if(StringUtils.isNotBlank(errorMessage)) {
            audit.setHomework(homework);
            audit.setErrorMessage(errorMessage);
            audit.setPrompt(prompt);
            auditRepository.save(audit);
            return;
        }

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

    private List<ChatMessage> initializeChatMessages() {
        List<ChatMessage> chatMessages = new ArrayList<>();
        String initialMessage = SYSTEM_MESSAGE;
        chatMessages.add(getChatMessage(SYSTEM_ROLE, initialMessage));
        return chatMessages;
    }

    private ChatMessage getChatMessage(String role, String message) {
        return new ChatMessage(role, message);
    }

    private ChatCompletionResult askGpt(List<ChatMessage> chatMessages, FunctionExecutor functionExecutor) throws JsonProcessingException {
        ChatCompletionRequest request = openAIChatCompletionClient.buildRequest(chatMessages, functionExecutor);
        log.info(objectMapper.writeValueAsString(request));
        return openAIChatCompletionClient.send(request);
    }

    private EvaluationResponse getEvaluation(FunctionExecutor functionExecutor, ChatMessage responseMessage) {
        try {
            ChatFunctionCall functionCall = responseMessage.getFunctionCall();
            if (functionCall == null) {
                throw new ChatCompletionProcessingException("No Function Call Found!");
            }
            log.debug("Trying to execute {} ...", functionCall.getName());
            Optional<ChatMessage> message = functionExecutor.executeAndConvertToMessageSafely(functionCall);
            if (message.isEmpty()) {
                log.error("Empty message from GPT");
                throw new ChatCompletionProcessingException("Chat Message is Empty");
            }
            return objectMapper.readValue(message.get().getContent(), EvaluationResponse.class);
        } catch (IOException e) {
            log.error("Exception occur", e);
            throw new ChatCompletionProcessingException("Unable to Process Evaluation due to " + e.getMessage());
        }
    }
}
