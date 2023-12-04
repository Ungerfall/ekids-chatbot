package org.braidsencurls.ekids_homework_checker;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.scheduling.annotation.EnableScheduling;

@EnableScheduling
@SpringBootApplication
public class HomeworkCheckerApplication {
    public static void main(String[] args) {
        SpringApplication.run(HomeworkCheckerApplication.class);
    }
}
