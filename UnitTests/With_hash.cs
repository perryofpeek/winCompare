using NUnit.Framework;

namespace UnitTests
{
    using System;
    using System.IO;

    using WinCompare;

    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class With_hash
    {
        [Test]
        public void Should_hash_a_file()
        {
            var fileName = Guid.NewGuid().ToString();
            File.WriteAllText(fileName, "This is a test for md5Hash");
            var hash = new Hash();
            var hashValue = hash.GetHash(fileName);
            Assert.That(hashValue, Is.EqualTo("12c56ae99c2d5c07703c647f7dc637b5"));
        }
    }
}