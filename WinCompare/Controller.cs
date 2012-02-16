namespace WinCompare
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class Controller
    {
        private readonly bool verbose;

        private readonly bool debug;       

        public Controller(bool verbose,bool debug)
        {
            this.verbose = verbose;
            this.debug = debug;
        }

        public int Compare(string leftPath, string rightPath, List<string> ignoreList,bool showFileDiff)
        {
            var startTime = DateTime.Now;
            var leftCompare = new FolderHasher(new Hasher(new Hash(), ignoreList),debug);
            var rightCompare = new FolderHasher(new Hasher(new Hash(), ignoreList),debug);

            leftCompare.SetFolder(leftPath);
            rightCompare.SetFolder(leftPath);
            var l = new Thread(leftCompare.Process1);
            var r = new Thread(rightCompare.Process1);

            l.Start();
            r.Start();
            while(l.IsAlive || r.IsAlive)
            {
                Thread.Sleep(10);
            }
            var comparison = new Comparison();
            comparison.Compare(leftCompare.Results,  rightCompare.Results);
            var executionTime = DateTime.Now.Subtract(startTime);
            if(showFileDiff)
            {
                foreach (var diff in comparison.Diff)
                {
                    Console.WriteLine(string.Format("{0} reason {1}", diff.Key, diff.Value.GetType()));
                }                
            }

            if(verbose)
            {
                Console.WriteLine(string.Format("Took {0} to run", executionTime));
                Console.WriteLine(string.Format("Folders are same = {0}", comparison.AreSame()));
                Console.WriteLine(string.Format("Number of files are same = {0}", comparison.Same.Count));
                Console.WriteLine(string.Format("Number of files are different = {0}", comparison.Diff.Count));
            }   
       
            return !comparison.AreSame() ? 1 : 0;
        }
    }
}