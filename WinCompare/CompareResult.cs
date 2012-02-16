namespace WinCompare
{
    public class CompareResult
    {
        public CompareResult(string state, string name)
        {
            this.State = state;
            this.Name = name;
        }

        public string State { get; set; }

        public string Name { get; set; }
    }
}