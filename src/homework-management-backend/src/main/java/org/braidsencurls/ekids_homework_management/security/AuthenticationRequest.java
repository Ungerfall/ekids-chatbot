package org.braidsencurls.ekids_homework_management.security;

import jakarta.validation.constraints.NotBlank;
import lombok.Data;

@Data
public class AuthenticationRequest {
    @NotBlank(message = "Username is required field")
    private String username;
    @NotBlank(message = "Password is required field")
    private String password;
}
