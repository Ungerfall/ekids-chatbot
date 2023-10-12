package org.braidsencurls.ekids_homework_management;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.scheduling.annotation.EnableScheduling;

@EnableScheduling
@SpringBootApplication
public class HomeworkManagement {
    public static void main(String[] args) {
        SpringApplication.run(HomeworkManagement.class);
    }
}
