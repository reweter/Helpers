namespace FileWorker
{
    internal class FileWorker
    {
        private List<FileContext> Contexts { get; set; }
        public FileContext WorkContext { get; private set; }
        private int ContextsCount { get { return Contexts.Count; } }

        public FileWorker()
        {
            Contexts = new List<FileContext>();
            WorkContext = AddContext(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
        }

        public void SetWorkContext(FileContext fc) => WorkContext = fc;

        public FileContext AddContext(string path)
        {
            FileContext fc = new(path);
            Contexts.Add(fc);
            return fc;
        }

        public void DeleteContext(string path) => Contexts.Remove(Contexts.FirstOrDefault(c => (c.FilePath == path)));

        public void DeleteContext(FileContext fc)
        {
            if (Contexts.Contains(fc))
                Contexts.Remove(fc);
        }
    }
}