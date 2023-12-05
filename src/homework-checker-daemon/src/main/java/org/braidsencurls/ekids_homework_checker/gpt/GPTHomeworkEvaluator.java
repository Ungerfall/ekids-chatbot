package org.braidsencurls.ekids_homework_checker.gpt;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.theokanning.openai.completion.chat.*;
import com.theokanning.openai.service.FunctionExecutor;
import lombok.extern.slf4j.Slf4j;
import org.braidsencurls.ekids_homework_checker.EvaluationResponse;
import org.braidsencurls.ekids_homework_checker.HomeworkEvaluator;
import org.braidsencurls.ekids_homework_checker.entities.Homework;
import org.braidsencurls.ekids_homework_checker.entities.HomeworkCriteria;
import org.braidsencurls.ekids_homework_checker.exceptions.ClientException;
import org.braidsencurls.ekids_homework_checker.exceptions.ChatCompletionProcessingException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
@Transactional
@Slf4j
public class GPTHomeworkEvaluator implements HomeworkEvaluator {
    public static final String SYSTEM_ROLE = "system";
    public static final String SYSTEM_MESSAGE = "You are an assistant and your task is to evaluate SCRATCH programming homework. You will check if the project.json content meets the following criteria. In your JSON response, it should include a properties success = true, if the current criteria is met, criteria = the current criteria being checked and sprite = list of sprite names that met the current criteria. Only use the functions you have been provided with.";
    public static final String USER_ROLE = "user";
    public static final String UNABLE_TO_PROCESS_GPT_RESPONSE = "Unable to process GPT response";
    private OpenAIChatCompletionClient openAIChatCompletionClient;
    private OpenAIHomeworkAuditorService homeworkAuditor;
    private ObjectMapper objectMapper;

    public GPTHomeworkEvaluator(OpenAIChatCompletionClient openAIChatCompletionClient,
                                OpenAIHomeworkAuditorService homeworkAuditor,
                                ObjectMapper objectMapper) {
        this.openAIChatCompletionClient = openAIChatCompletionClient;
        this.homeworkAuditor = homeworkAuditor;
        this.objectMapper = objectMapper;

    }

    public  List<EvaluationResponse> evaluate(String content, Homework homework) {
        List<EvaluationResponse> evaluations = new ArrayList<>();
        List<ChatMessage> chatMessages = initializeChatMessages();
        FunctionExecutor functionExecutor = createFunctionExecutor();

        for(int index = 0; index < homework.getCriteria().size(); index++) {
            String currentMessageContent = chatMessages.get(index).getContent();
            try {
                HomeworkCriteria criterion = homework.getCriteria().get(index);
                prepareMessage(content, chatMessages, index, criterion);
                ChatCompletionResult result = askGpt(chatMessages, functionExecutor);

                processGptResponse(homework, evaluations, chatMessages, functionExecutor, currentMessageContent, result);

            } catch (JsonProcessingException | RuntimeException e) {
                log.error("Evaluation failed!", e);
                homeworkAuditor.saveAudit(homework, currentMessageContent, e.getMessage());
                throw new ClientException(e.getMessage());
            }
        }
        return evaluations;
    }

    private FunctionExecutor createFunctionExecutor() {
        List<ChatFunction> chatFunctions = List.of(new GPTEvaluatorFunction().getFunction());
        return new FunctionExecutor(chatFunctions);
    }

    private String createMessage(String content, HomeworkCriteria criterion, int ctr) {
        if (ctr == 0) {
            return "project.json content: " + content + ". criteria: " + criterion.getDetail();
        } else {
            return "Using the same project.json content above, evaluate the criteria: " + criterion.getDetail();
        }
    }

    private void prepareMessage(String content, List<ChatMessage> chatMessages, int index, HomeworkCriteria criterion) {
        String message = createMessage(content, criterion, index);
        chatMessages.add(getChatMessage(USER_ROLE, message));
    }
    private void processGptResponse(Homework homework, List<EvaluationResponse> evaluations, List<ChatMessage> chatMessages, FunctionExecutor functionExecutor,
                                    String currentMessageContent, ChatCompletionResult result) {
        ChatCompletionChoice chatCompletionChoice = result.getChoices().get(0);
        if (chatCompletionChoice != null && chatCompletionChoice.getFinishReason().equals("function_call")) {
            ChatMessage responseMessage = chatCompletionChoice.getMessage();
            chatMessages.add(responseMessage);

            evaluations.add(getEvaluation(functionExecutor, responseMessage));
            homeworkAuditor.saveAudit(homework, result, currentMessageContent);
        } else {
            log.error(UNABLE_TO_PROCESS_GPT_RESPONSE);
            homeworkAuditor.saveAudit(homework, currentMessageContent, UNABLE_TO_PROCESS_GPT_RESPONSE);
            throw new ClientException(UNABLE_TO_PROCESS_GPT_RESPONSE);
        }
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
        ChatCompletionResult result = openAIChatCompletionClient.send(request);
        log.debug(result.toString());
        return result;
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
