package org.braidsencurls.ekids_homework_management.security;

import jakarta.persistence.EntityNotFoundException;
import lombok.AllArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.braidsencurls.ekids_homework_management.entities.User;
import org.braidsencurls.ekids_homework_management.exceptions.NoEntityFoundException;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

@AllArgsConstructor
@Service
@Slf4j
public class AuthenticationServiceImpl implements AuthenticationService {

    private UserRepository userRepository;
    private UserMapper userMapper;
    private PasswordEncoder passwordEncoder;
    private AuthenticationManager authenticationManager;
    private JwtService jwtService;

    @Override
    public UserDTO register(UserDTO user) {
        log.debug("Attempting to register a user");
        User userEntity = userMapper.toEntity(user);
        String encodedPassword = passwordEncoder.encode(user.getPassword());
        userEntity.setPassword(encodedPassword);

        User newUser = userRepository.save(userEntity);
        log.debug("Successfully registered a user: " + newUser.getUsername());
        return userMapper.toDTO(newUser);
    }

    @Override
    public AuthenticationResponse authenticate(AuthenticationRequest authenticationRequest) {
        log.debug("Attempting to authenticate user: " + authenticationRequest.getUsername());
        User userEntity = getByUsername(authenticationRequest.getUsername());
        authenticate(authenticationRequest, userEntity);

        String authToken = jwtService.generateToken(userEntity);
        String refreshToken = jwtService.generateRefreshToken(userEntity);

        log.debug("User has been successfully authenticated");
        return AuthenticationResponse.builder()
                .accessToken(authToken).refreshToken(refreshToken).build();
    }

    private void authenticate(AuthenticationRequest authenticationRequest, User userEntity) {
        if (!(passwordEncoder.matches(authenticationRequest.getPassword(), userEntity.getPassword()))) {
            throw new EntityNotFoundException("Wrong password. Please try again");
        }
        authenticationManager.authenticate(
                new UsernamePasswordAuthenticationToken(
                        authenticationRequest.getUsername(), authenticationRequest.getPassword()));
    }

    private User getByUsername(String username) {
        return userRepository.findByUsername(username).orElseThrow(() -> new NoEntityFoundException("No User with username " + username + " was found!"));
    }

}
