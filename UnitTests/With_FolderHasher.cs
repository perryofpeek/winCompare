namespace UnitTests
{
    using System.Collections.Generic;

    using WinCompare;

    using NUnit.Framework;

    using Rhino.Mocks;

    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class With_FolderHasher
    {
        [Test]
        public void should_get_single_folder()
        {
            var folder = "folder";
            var hasher = MockRepository.GenerateMock<IHasher>();
            var comparer = new FolderHasher(hasher, true);
            hasher.Expect(x => x.ExamineDir(folder, folder));
            var dict = new Dictionary<string, string> { { "a", "b" } };
            hasher.Expect(x => x.GetFileHashList()).Return(dict);
            //Test
            hasher.GetFileHashList();
            //Assert
            comparer.Process(folder);
            hasher.VerifyAllExpectations();
        }
    }
}