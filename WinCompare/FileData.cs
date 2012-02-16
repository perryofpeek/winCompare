namespace WinCompare
{
    public class FileData
    {
        public FileData(string name, Err error, string type)
        {
            this.Name = name;
            this.Error = error;
            this.Type = type;
        }

        public string Name { get; set; }

        public Err Error { get; set; }

        public string Type { get; set; }
    }
}