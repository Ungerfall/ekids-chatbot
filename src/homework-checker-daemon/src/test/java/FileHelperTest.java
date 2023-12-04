import org.braidsencurls.ekids_homework_checker.utilities.FileHelper;
import org.braidsencurls.ekids_homework_checker.exceptions.NoContentException;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;

public class FileHelperTest {

    @Test
    public void readFileFromZip_validFile_shouldReadContent() throws FileNotFoundException {
        String sb3FilePath = "src/test/resources/Sample.sb3";
        FileInputStream fileInputStream = new FileInputStream(sb3FilePath);
        String content = FileHelper.readFileFromZip(fileInputStream, "project.json");
        Assertions.assertNotNull(content);
    }

    @Test
    public void readFileFromZip_inValidFile_shouldResultToNoContentException() throws FileNotFoundException {
        String sb3FilePath = "src/test/resources/Sample.sb3";
        FileInputStream fileInputStream = new FileInputStream(sb3FilePath);
        Assertions.assertThrows(NoContentException.class, () -> {
            FileHelper.readFileFromZip(fileInputStream, "no_such_file.json");
        });
    }

    @Test
    public void shrinkJson_validSchema_shouldShrinkJson() throws IOException {
        String projectJsonFilePath = "src/test/resources/sample-project.json";
        String expectedProjectJsonFilePath = "src/test/resources/shrunken-project.json";

        String projectContent = Files.readString(Path.of(projectJsonFilePath));
        String expectedContent = Files.readString(Path.of(expectedProjectJsonFilePath));

        List<String> jsonPathsToRemove = List.of("$.targets[*].costumes", "$.targets[*].sounds");
        String output = FileHelper.removeJsonPaths(projectContent, jsonPathsToRemove);

        Assertions.assertEquals(expectedContent, output);
    }
}
