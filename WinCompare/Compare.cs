namespace WinCompare
{
    public class Compare
    {
        private readonly IHasher hasherLeft;

        private readonly IHasher hasherRight;

        private readonly IComparison comparison;

        public Compare(IHasher hasherLeft, IHasher hasherRight, IComparison comparison)
        {
            this.hasherLeft = hasherLeft;
            this.hasherRight = hasherRight;
            this.comparison = comparison;
        }

        public Result Go(string leftFolder, string rightFolder)
        {
            hasherLeft.ExamineDir(leftFolder, leftFolder);
            hasherRight.ExamineDir(rightFolder, rightFolder);

            var left = hasherLeft.GetFileHashList();
            var right = hasherRight.GetFileHashList();

            comparison.Compare(left, right);
            var rtn = new Result ();
            return rtn;
        }
    }
}