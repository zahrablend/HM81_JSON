using System.Text.Json;

namespace HM81_JSON
{
    // Exceptions best practices: https://learn.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions
    public class StudentData
    {
        private readonly string _fileName;

        public StudentData(string fileName)
        {
            // Validate method arguments (if the fileName parameter is null or empty)
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }
            _fileName = fileName;
        }

        public Student Load()
        {
            // Return JsonSerializer.Deserialize<Student>(json) gives warning CS8603: nullabe refference type
            try
            {
                string json = File.ReadAllText(_fileName);
                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new InvalidOperationException("File is empty");
                }
                var student = JsonSerializer.Deserialize<Student>(json);
                // Check if student parameter is null when loading data
                if (student == null)
                {
                    throw new InvalidOperationException("Failed to deserialize data");
                }
                return student;
            } 
            
            catch (JsonException exc)
            {
                throw new InvalidOperationException("Invalid JSON data", exc);
            }

            catch (FileNotFoundException exc)
            {
                throw new InvalidOperationException("File not found", exc);
            }
            
        }

        public void Save(Student student)
        {
            // Check if student parameter is null when saving data
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }
            string json = JsonSerializer.Serialize(student);
            File.WriteAllText(_fileName, json);
        }
    }
}
