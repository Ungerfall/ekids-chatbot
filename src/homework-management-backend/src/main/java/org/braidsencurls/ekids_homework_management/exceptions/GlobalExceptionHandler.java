package org.braidsencurls.ekids_homework_management.exceptions;

import jakarta.validation.ConstraintViolationException;
import lombok.extern.slf4j.Slf4j;
import org.springframework.context.support.DefaultMessageSourceResolvable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.AuthenticationException;
import org.springframework.web.bind.MethodArgumentNotValidException;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
@Slf4j
public class GlobalExceptionHandler {

    @ExceptionHandler(RuntimeException.class)
    public ResponseEntity<String> handleGenericException(RuntimeException e) {
        log.error("Technical Error occur", e);
        return ResponseEntity.internalServerError().body(e.getMessage());
    }

    @ExceptionHandler(NoEntityFoundException.class)
    public ResponseEntity<String> handleNotFound(NoEntityFoundException e) {
        log.error("Entity not found", e);
        return ResponseEntity.status(HttpStatus.NOT_FOUND).body(e.getMessage());
    }

    @ExceptionHandler(AuthenticationException.class)
    public ResponseEntity<String> handleUnauthorized(AuthenticationException e) {
        log.error("Unauthorized access", e);
        return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(e.getMessage());
    }

    @ExceptionHandler({MethodArgumentNotValidException.class, ConstraintViolationException.class})
    public ResponseEntity<String> handleValidationExceptions(Exception ex) {
        String message;
        if (ex instanceof MethodArgumentNotValidException methodArgumentNotValidException) {
            message = methodArgumentNotValidException.getBindingResult()
                    .getAllErrors()
                    .stream()
                    .findFirst().map(DefaultMessageSourceResolvable::getDefaultMessage)
                    .orElse(methodArgumentNotValidException.getMessage());
        } else {
            message = ex.getMessage();
        }

        log.error("The error occur with message={}", message);
        return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(message);
    }
}
