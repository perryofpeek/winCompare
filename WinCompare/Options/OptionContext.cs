namespace NDesk.Options
{  
    public class OptionContext {
        public OptionContext ()
        {
        }

        public Option Option { get; set; }
        public string OptionName { get; set; }
        public int    OptionIndex { get; set; }
        public string OptionValue { get; set; }
    }
}