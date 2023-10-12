import org.braidsencurls.ekids_homework_checker.utilities.FileHelper;
import org.braidsencurls.ekids_homework_checker.exceptions.NoContentException;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.io.FileInputStream;
import java.io.FileNotFoundException;

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
}
