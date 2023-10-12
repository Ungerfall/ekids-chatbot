package org.braidsencurls.ekids_homework_checker.gpt;

import com.theokanning.openai.completion.chat.ChatFunction;
import org.braidsencurls.ekids_homework_checker.EvaluationResponse;

public class GPTEvaluatorFunction {
    public ChatFunction getFunction() {
        return ChatFunction.builder().name("get_evaluation")
                .description("Get the homework evaluation based on the given criteria")
                .executor(EvaluationResponse.class, w -> new EvaluationResponse(w.isSuccess(), w.getCriteria(), w.getSprite())).build();
    }
}
