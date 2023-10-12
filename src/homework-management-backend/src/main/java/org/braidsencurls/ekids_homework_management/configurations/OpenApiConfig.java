package org.braidsencurls.ekids_homework_management.configurations;

import io.swagger.v3.oas.models.OpenAPI;
import io.swagger.v3.oas.models.info.Info;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class OpenApiConfig {

    @Bean
    public OpenAPI homeworkChecker() {
        return new OpenAPI()
                .info(new Info().title("Scratch Homework Evaluator")
                        .description("Evaluates scratch project")
                        .version("1.0.0"));
    }

}

