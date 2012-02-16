namespace WinCompare
{
    using System;
    using System.Collections.Generic;

    public class Comparison : IComparison
    {
        public Dictionary<string, string> Same;

        public Dictionary<string, IDiffResult> Diff;

        public Comparison()
        {
            this.Create();
        }

        public TimeSpan ExecutionTime { get; private set; }

        private void Create()
        {
            this.Same = new Dictionary<string, string>();
            this.Diff = new Dictionary<string, IDiffResult>();
        }

        public void Compare(Dictionary<string, string> left, Dictionary<string, string> right)
        {
            var startTime = DateTime.Now;
            this.Create();
            this.Process(left, right);
            ExecutionTime = DateTime.Now.Subtract(startTime);
        }

        private void Process(Dictionary<string, string> left, Dictionary<string, string> right)
        {
            this.ProcessRight(left, right);
            this.ProcessLeft(left, right);
        }

        private void ProcessLeft(Dictionary<string, string> left, Dictionary<string, string> right)
        {
            foreach (var leftFile in left)
            {
                var log = string.Format("{0} {1}", leftFile.Key, leftFile.Value);
                if (!right.ContainsKey(leftFile.Key))
                {
                    this.Diff.Add(leftFile.Key, new MissingOnRightSideResult(leftFile.Key, left[leftFile.Key]));
                }
            }
        }

        private void ProcessRight(Dictionary<string, string> left, Dictionary<string, string> right)
        {
            foreach (var rightFile in right)
            {
                var log = string.Format("{0} {1}", rightFile.Key, rightFile.Value);
                if (left.ContainsKey(rightFile.Key))
                {
                    if (left[rightFile.Key] == rightFile.Value)
                    {
                        this.Same.Add(rightFile.Key, rightFile.Value);
                    }
                    else
                    {
                        this.Diff.Add(
                            rightFile.Key, new DifferentHashResult(rightFile.Key, left[rightFile.Key], rightFile.Value));
                    }
                }
                else
                {
                    this.Diff.Add(rightFile.Key, new MissingOnLeftSideResult(rightFile.Key, right[rightFile.Key]));
                }
            }
        }

        public bool AreSame()
        {
            return this.Diff.Count == 0;
        }
    }
}