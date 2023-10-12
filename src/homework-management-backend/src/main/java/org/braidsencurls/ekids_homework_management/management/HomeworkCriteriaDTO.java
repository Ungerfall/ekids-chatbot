package org.braidsencurls.ekids_homework_management.management;

import jakarta.validation.constraints.NotBlank;
import lombok.Data;
import java.util.UUID;

@Data
public class HomeworkCriteriaDTO {

    private UUID id;
    @NotBlank(message = "detail is required field")
    private String detail;
}
