package org.braidsencurls.ekids_homework_management.management;

import com.fasterxml.jackson.annotation.JsonInclude;
import jakarta.validation.constraints.NotBlank;
import lombok.Data;

import java.util.List;
import java.util.UUID;

@Data
@JsonInclude(JsonInclude.Include.NON_NULL)
public class HomeworkDTO {

    private UUID id;
    @NotBlank(message = "code is required field")
    private String code;
    @NotBlank(message = "description is required field")
    private String description;
    private List<HomeworkCriteriaDTO> criteria;
}
