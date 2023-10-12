package org.braidsencurls.ekids_homework_management.security;

import jakarta.validation.Valid;
import lombok.AllArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@AllArgsConstructor
@RestController
@RequestMapping("/authentication")
public class AuthenticationController {

    private final AuthenticationService authService;

    @PostMapping("/login")
    public ResponseEntity<AuthenticationResponse> authenticate(@RequestBody @Valid AuthenticationRequest authenticationRequest) {
        AuthenticationResponse authResponse = authService.authenticate(authenticationRequest);
        return ResponseEntity.ok().body(authResponse);
    }

    @PostMapping("/users")
    private ResponseEntity<UserDTO> register(@RequestBody @Valid UserDTO user) {
        UserDTO registeredUser = authService.register(user);
        return ResponseEntity.ok().body(registeredUser);
    }
}
