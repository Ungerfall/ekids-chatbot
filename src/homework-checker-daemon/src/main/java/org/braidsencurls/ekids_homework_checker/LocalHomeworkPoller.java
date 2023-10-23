package org.braidsencurls.ekids_homework_checker;

import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.nio.channels.FileChannel;
import java.nio.channels.FileLock;
import java.nio.file.*;

import java.util.UUID;

import static java.nio.file.StandardCopyOption.REPLACE_EXISTING;

@Component
@Slf4j
public class LocalHomeworkPoller {

    private HomeworkProcessor homeworkProcessor;

    @Value("${local.source.directory}")
    private String sourceDirectory;
    @Value("${local.success.destination.directory}")
    private String successDirectory;
    @Value("${local.error.destination.directory}")
    private String errorDirectory;

    public static final String SB_3_FILE_EXTENSION = ".sb3";

    public LocalHomeworkPoller(HomeworkProcessor homeworkProcessor) {
        this.homeworkProcessor = homeworkProcessor;
    }

    /**
     * Polls files from a local directory
     */
    @Scheduled(fixedRate = 5000)
    public void poll() {
        log.debug("Polling files from {}", sourceDirectory);
        try (DirectoryStream<Path> directoryStream = Files.newDirectoryStream(Paths.get(sourceDirectory))) {
            for (Path filePath : directoryStream) {
                processEachFile(filePath);
            }
        } catch (IOException e) {
            log.error("Exception occur!", e);
        }
    }

    public void processEachFile(Path filePath) {
        String filename = UUID.randomUUID() + ".json";
        try (FileChannel channel = FileChannel.open(filePath, StandardOpenOption.READ);
            FileLock lock = channel.lock(0, Long.MAX_VALUE, true)) {
            File file = filePath.toFile();
            filename = file.getName();

            if(!filename.endsWith(SB_3_FILE_EXTENSION)) {
                return;
            }

            InputStream inputStream = new FileInputStream(file);
            homeworkProcessor.process(filename, inputStream);
            Path destination = Paths.get(successDirectory).resolve(file.getName());
            moveFile(filePath, destination);
        } catch (IOException e) {
            Path destination = Paths.get(errorDirectory).resolve(filename);
            moveFile(filePath, destination);
        }
    }

    private void moveFile(Path source, Path target) {
        try {
            Files.move(source, target, REPLACE_EXISTING);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
    }
}
