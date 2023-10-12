package org.braidsencurls.ekids_homework_management.security;

import org.braidsencurls.ekids_homework_management.entities.User;
import org.mapstruct.Mapper;
import org.mapstruct.Mapping;
import org.mapstruct.Mappings;

@Mapper(componentModel = "spring")
public interface UserMapper {

    @Mappings(@Mapping(target = "password", ignore = true))
    UserDTO toDTO(User user);

    User toEntity(UserDTO userDTO);
}
