package org.braidsencurls.ekids_homework_management.security;

public interface AuthenticationService {

    UserDTO register(UserDTO user);

    AuthenticationResponse authenticate(AuthenticationRequest authenticationRequest);

}
