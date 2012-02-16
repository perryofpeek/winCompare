namespace WinCompare
{
    using System.Collections.Generic;

    public interface IComparison
    {
        void Compare(Dictionary<string,string> left,Dictionary<string,string> right);
    }
}