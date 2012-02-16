namespace WinCompare
{
    using System;

    using NDesk.Options;

    public class Params
    {
        protected OptionSet OptionsSet { get; set; }

        public Params()
        {
            OptionsSet = new OptionSet();
            this.LeftFolderPath = string.Empty;
            OptionsSet.Add("l|left=", "left folder path", (string v) => this.LeftFolderPath = v);
            OptionsSet.Add("i|ignore=", "space separated list to ignore", (string v) => this.IgnoreList = v);
            OptionsSet.Add("r|right=", "right folder path", (string v) => this.RightFolderPath = v);
            OptionsSet.Add("d|debug", "show debug data", v => Debug = v != null);
            OptionsSet.Add("v|verbose", "show verbose data", v => Verbose = v != null);
            OptionsSet.Add("s|showdiff", "show diff data", v => ShowDiff = v != null);
            OptionsSet.Add("h|help", "show help", v => Help = v != null);
        }

        public string IgnoreList { get; set; }

        public bool ShowDiff { get; set; }

        public bool Help { get; set; }

        public bool Verbose { get; set; }

        public bool Debug { get; set; }

        public string LeftFolderPath { get; set; }

        public string RightFolderPath { get; set; }

        public void Parse(string[] args)
        {
            try
            {
                OptionsSet.Parse(args);                
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                throw new ApplicationException();
            } 
        }
    }
}