package org.braidsencurls.ekids_homework_management.security;

import com.fasterxml.jackson.annotation.JsonInclude;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Pattern;
import lombok.Data;
import org.hibernate.validator.constraints.Length;

@Data
@JsonInclude(JsonInclude.Include.NON_NULL)
public class UserDTO {

    @NotBlank(message = "username is required field")
    @Length(min = 2, max = 30, message = "Username should contain from 2 to 30 characters")
    private String username;

    @NotBlank(message = "password is required field")
    @Pattern(regexp = "(^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{7,30}$)?",
            message = "Your password must contain upper and lower case letters and numbers, at least 7 and maximum 30 characters." +
                    "Password cannot contains spaces")
    private String password;

    private String role;
}
