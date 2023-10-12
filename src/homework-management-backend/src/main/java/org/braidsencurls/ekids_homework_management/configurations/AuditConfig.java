package org.braidsencurls.ekids_homework_management.configurations;

import org.springframework.data.domain.AuditorAware;

import java.util.Optional;

public class AuditConfig implements AuditorAware<String> {

    @Override
    public Optional<String> getCurrentAuditor() {
        //TODO: Return actual user
        return Optional.of("system");
    }
}
