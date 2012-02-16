using System;

namespace WinCompare
{
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var p = new Params();
                p.Parse(args);
                if (CheckValues(p))
                {
                    var ignoreList = new List<string>() ;
                    if(!string.IsNullOrEmpty(p.IgnoreList))
                    {
                        ignoreList = new List<string>(p.IgnoreList.Split(' '));
                    }                   
                    var controller = new Controller(p.Verbose, p.Debug);
                    var exitCode = controller.Compare(p.LeftFolderPath, p.RightFolderPath, ignoreList, p.ShowDiff);
                    Environment.Exit(exitCode);
                }
                Environment.Exit(2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Environment.Exit(2);
            }
        }

        private static bool CheckValues(Params p)
        {
            if (string.IsNullOrEmpty(p.LeftFolderPath))
            {
                Console.WriteLine("you must specify the left folder path");
                return false;
            }

            if (string.IsNullOrEmpty(p.RightFolderPath))
            {
                Console.WriteLine("you must specify the right folder path");
                return false;
            }

            if (p.Help)
            {
                Console.WriteLine("Display help");
                return false;
            }

            return true;
        }
    }
}