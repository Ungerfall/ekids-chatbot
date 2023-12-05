package org.braidsencurls.ekids_homework_checker.gpt;

import com.theokanning.openai.completion.chat.ChatCompletionRequest;
import com.theokanning.openai.completion.chat.ChatCompletionRequest.ChatCompletionRequestFunctionCall;
import com.theokanning.openai.completion.chat.ChatCompletionResult;
import com.theokanning.openai.completion.chat.ChatMessage;
import com.theokanning.openai.service.FunctionExecutor;
import com.theokanning.openai.service.OpenAiService;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Component;
import org.springframework.util.CollectionUtils;

import java.time.Duration;
import java.util.List;

@Component
@Slf4j
public class OpenAIChatCompletionClient {

    @Value("${openai.completion.endpoint}")
    private String COMPLETION_ENDPOINT;
    @Value("${openai.api.key}")
    private String API_KEY;
    @Value("${openai.model}")
    private String API_MODEL;
    @Value("${openai.timeout.seconds}")
    private long API_TIMEOUT;
    @Value("${openai.completion.temperature}")
    private double COMPLETION_TEMPERATURE;
    @Value("${openai.completion.top_p}")
    private double COMPLETION_TOP_P;
    @Value("${openai.completion.frequency.penalty}")
    private double COMPLETION_FREQUENCY_PENALTY;
    @Value("${openai.completion.presence.penalty}")
    private double COMPLETION_PRESENCE_PENALTY;

    public OpenAiService getOpenAiService() {
        return new OpenAiService(API_KEY, Duration.ofSeconds(API_TIMEOUT));
    }

    public ChatCompletionRequest buildRequest(List<ChatMessage> chatMessages, FunctionExecutor functionExecutor) {
        return ChatCompletionRequest.builder()
                .model(API_MODEL)
                .temperature(COMPLETION_TEMPERATURE)
                .frequencyPenalty(COMPLETION_FREQUENCY_PENALTY)
                .presencePenalty(COMPLETION_PRESENCE_PENALTY)
                .topP(COMPLETION_TOP_P)
                .messages(chatMessages)
                .functions(functionExecutor.getFunctions())
                .functionCall(new ChatCompletionRequestFunctionCall("auto"))
                .build();
    }

    public ChatCompletionResult send(ChatCompletionRequest chatCompletionRequest) {
        ChatCompletionResult result = getOpenAiService().createChatCompletion(chatCompletionRequest);
        if (CollectionUtils.isEmpty(result.getChoices())) {
            throw new RuntimeException("No response from GPT");
        }
        return result;
    }

}
