package org.braidsencurls.ekids_homework_checker;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.jcraft.jsch.*;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;

import java.io.ByteArrayInputStream;
import java.util.Vector;

@Component
@Slf4j
public class SftpHomeworkPoller {

    public static final String SB_3_FILE_EXTENSION = ".sb3";
    @Value("${sftp.hostname}")
    private String HOSTNAME;

    @Value("${sftp.username}")
    private String USERNAME;

    @Value("${sftp.password}")
    private String PASSWORD;

    @Value("${sftp.source.directory}")
    private String SOURCE_DIRECTORY;

    @Value("${sftp.success.destination.directory}")
    private String SUCCESS_DESTINATION_DIRECTORY;

    @Value("${sftp.error.destination.directory}")
    private String ERROR_DESTINATION_DIRECTORY;

    @Value("${sftp.maxFilesToPoll}")
    private int MAX_FILES_TO_POLL;

    private HomeworkProcessor homeworkProcessor;

    public SftpHomeworkPoller(HomeworkProcessor homeworkProcessor) {
        this.homeworkProcessor = homeworkProcessor;
    }

    @Scheduled(fixedRate = 5000)
    public void poll() {
        JSch jsch = new JSch();
        Session session = null;
        ChannelSftp channelSftp = null;

        try {
            session = jsch.getSession(USERNAME, HOSTNAME);
            session.setPassword(PASSWORD);
            session.setConfig("StrictHostKeyChecking", "no");
            session.connect();

            channelSftp = (ChannelSftp) session.openChannel("sftp");
            channelSftp.connect();

            int filesProcessed = 0;
            while (filesProcessed < MAX_FILES_TO_POLL) {
                Vector<ChannelSftp.LsEntry> files = channelSftp.ls(SOURCE_DIRECTORY);
                filesProcessed = processFiles(channelSftp, filesProcessed, files);
            }
        } catch (JSchException | SftpException e) {
            log.error("Exception occur!", e);
        } finally {
            if (channelSftp != null) {
                channelSftp.disconnect();
            }
            if (session != null) {
                session.disconnect();
            }
        }
    }

    private int processFiles(ChannelSftp channelSftp, int filesProcessed, Vector<ChannelSftp.LsEntry> files) throws SftpException {
        for (ChannelSftp.LsEntry entry : files) {
            String filename = entry.getFilename();
            if (!entry.getAttrs().isDir() && !filename.equals(".") && !filename.equals("..")
                    && filename.endsWith(SB_3_FILE_EXTENSION)
                    && !lockFileExists(channelSftp, filename)) {

                processEachFile(channelSftp, filename);

                filesProcessed++;
                if (filesProcessed >= MAX_FILES_TO_POLL) {
                    log.debug("MaxFilesToPoll has been reached. Will wait for the next polling...");
                    break;
                }
            }
        }
        return filesProcessed;
    }

    private void processEachFile(ChannelSftp channelSftp, String filename) throws SftpException {
        try {
            createLockFile(channelSftp, filename);
            homeworkProcessor.process(filename, channelSftp.get(SOURCE_DIRECTORY + filename));
            moveFile(channelSftp, filename, SUCCESS_DESTINATION_DIRECTORY);
        } catch (JsonProcessingException | RuntimeException e) {
            log.error("Unable to process file: {}", filename, e);
            moveFile(channelSftp, filename, ERROR_DESTINATION_DIRECTORY);
        } finally {
            removeLockFile(channelSftp, SOURCE_DIRECTORY, filename);
        }
    }

    private boolean lockFileExists(ChannelSftp channelSftp, String filename) {
        try {
            SftpATTRS lockFileAttrs = channelSftp.lstat(SOURCE_DIRECTORY + filename + ".lock");
            return lockFileAttrs != null;
        } catch (SftpException e) {
            return false;
        }
    }

    private void createLockFile(ChannelSftp channelSftp, String filename) throws SftpException {
        channelSftp.put(new ByteArrayInputStream(new byte[0]), SOURCE_DIRECTORY + filename + ".lock");
    }

    private void removeLockFile(ChannelSftp channelSftp, String remoteDirectory, String filename) throws SftpException {
        channelSftp.rm(remoteDirectory + filename + ".lock");
    }

    private void moveFile(ChannelSftp channelSftp, String filename, String destinationDirectory) throws SftpException {
        channelSftp.rename(SOURCE_DIRECTORY + filename, destinationDirectory + filename);
    }
}
