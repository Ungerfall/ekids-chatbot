package org.braidsencurls.ekids_homework_checker.management;

import org.braidsencurls.ekids_homework_checker.entities.Homework;

public interface HomeworkAuditorService {

    void saveAudit(Homework homework, String prompt, String errorMessage);
}
