using System.Text.Json;

namespace HM81_JSON
{
    public class StudentData
    {
        private readonly string _fileName;

        public StudentData(string fileName)
        {
            _fileName = fileName;
        }

        public Student Load()
        {
            // return JsonSerializer.Deserialize<Student>(json) gives warning CS8603: nullabe refference type
            try
            {
                string json = File.ReadAllText(_fileName);
                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new InvalidOperationException("File is empty");
                }
                var student = JsonSerializer.Deserialize<Student>(json);
                if (student == null)
                {
                    throw new InvalidOperationException("Failed to deserialize data");
                }
                return student;
            } 
            
            catch (JsonException exc)
            {
                throw new InvalidOperationException("Invalid JSOn data", exc);
            }

            catch (FileNotFoundException exc)
            {
                throw new InvalidOperationException("File not found", exc);
            }
            
        }

        public void Save(Student student)
        {
            string json = JsonSerializer.Serialize(student);
            File.WriteAllText(_fileName, json);
        }
    }
}
