using NUnit.Framework;

namespace UnitTests
{
    using System.Collections.Generic;

    using WinCompare;

    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class With_Hasher
    {
        [Test]
        public void Should_hash_folder()
        {
            var hash= new Hash();
            var ignoreList = new List<string>();
            var hasher = new Hasher(hash, ignoreList);
            hasher.ExamineDir("root", "root");
            var results = hasher.GetFileHashList();
            Assert.That(results.Count, Is.EqualTo(7));
            Assert.That(results.ContainsKey(@"a1\File1.txt"),Is.True);
            Assert.That(results.ContainsKey(@"a1\File2.txt"), Is.True);
            Assert.That(results.ContainsKey(@"a1\Copy of File2.txt"), Is.True);
            Assert.That(results.ContainsKey(@"b1\someFile.txt"), Is.True);
            Assert.That(results.ContainsKey(@"b1\b2\someFile.txt"), Is.True);
            Assert.That(results.ContainsKey(@".svn\ignoreMe.txt"), Is.True);
            Assert.That(results.ContainsKey(@"b1\b2\.svn\ignoreMe2.txt"), Is.True);
        }

        [Test]
        public void Should_ignore_a_folder()
        {
            var hash = new Hash();
            var ignoreList = new List<string> { ".svn" };
            var hasher = new Hasher(hash, ignoreList);            
            hasher.ExamineDir("root", "root");
            var results = hasher.GetFileHashList();
            Assert.That(results.Count, Is.EqualTo(5));
            Assert.That(results.ContainsKey(@"a1\File1.txt"), Is.True);
            Assert.That(results.ContainsKey(@"a1\File2.txt"), Is.True);
            Assert.That(results.ContainsKey(@"a1\Copy of File2.txt"), Is.True);
            Assert.That(results.ContainsKey(@"b1\someFile.txt"), Is.True);
            Assert.That(results.ContainsKey(@"b1\b2\someFile.txt"), Is.True);
            Assert.That(results.ContainsKey(@".svn\ignoreMe.txt"), Is.False);
            Assert.That(results.ContainsKey(@"b1\b2\.svn\ignoreMe2.txt"), Is.False);
        }
    }
}