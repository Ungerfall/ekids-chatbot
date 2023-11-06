package org.braidsencurls.ekids_homework_checker.utilities;

import com.jayway.jsonpath.Configuration;
import com.jayway.jsonpath.DocumentContext;
import com.jayway.jsonpath.JsonPath;
import com.jayway.jsonpath.Option;
import org.braidsencurls.ekids_homework_checker.exceptions.NoContentException;

import java.io.*;
import java.util.List;
import java.util.zip.ZipEntry;
import java.util.zip.ZipInputStream;

public class FileHelper {

    public static String readFileFromZip(InputStream inputStream, String fileNameOfFileToRead) {
        try (inputStream; ZipInputStream zipInputStream = new ZipInputStream(inputStream)) {
            ZipEntry entry;
            while ((entry = zipInputStream.getNextEntry()) != null) {
                if (entry.getName().equals(fileNameOfFileToRead)) {
                    // Read the content
                    StringBuilder content = new StringBuilder();
                    BufferedReader reader = new BufferedReader(new InputStreamReader(zipInputStream));
                    String line;
                    while ((line = reader.readLine()) != null) {
                        content.append(line);
                    }
                    return content.toString();
                }
            }
        } catch (IOException e) {
            throw new RuntimeException("Technical Error Occur!", e);
        }
        throw new NoContentException("No content found for the zip");
    }

    public static String removeJsonPaths(String jsonString, List<String> jsonPaths) {
        try {
            Configuration configuration = Configuration.builder()
                    .options(Option.ALWAYS_RETURN_LIST)
                    .build();

            DocumentContext document = JsonPath.using(configuration).parse(jsonString);

            for (String jsonPath : jsonPaths) {
                document.delete(jsonPath);
            }

            return document.jsonString();
        } catch (Exception e) {
            return jsonString;
        }
    }
}
