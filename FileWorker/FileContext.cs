using System;

namespace FileWorker
{
    public class FileContext
    {
        public string FilePath { get; private set; }
        public bool Exist { get { return File.Exists(FilePath); } }

        public FileContext(string filePath)
        {
            FilePath = filePath;

            if (!Exist)
                CreateFile();
        }

        public void CreateFile()
        {
            using (FileStream fs = File.Create(FilePath)) { };
        }

        public void WriteNewLine(string content)
        {
            using (StreamWriter sw = new(FilePath))
                sw.WriteLine(content);
        }

        public void WriteLines(string[] content)
        {
            using (StreamWriter sw = new(FilePath))
            {
                for (int i = 0; i < content.Length; i++)
                    sw.WriteLine(content);
            }
        }

        public string[] ReadLines()
        {
            return File.ReadAllLines(FilePath);
        }

        public string ReadLine()
        {
            using (StreamReader sr = new(FilePath))
            {
                string? line = sr.ReadLine();
                if (!string.IsNullOrEmpty(line))
                    return line;
                else
                    return string.Empty;
            }
        }

        #region TODO
        // public void DeleteLastLine() { }
        #endregion
    }
}
