namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using WinCompare;
    using NUnit.Framework;
    using Rhino.Mocks;

    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class With_Comparer
    {
        [Test]
        public void ShouldCompareTwoFolders()
        {
            var folderLeft = @"root";
            var folderRight = @"root";
            var hasherLeft = MockRepository.GenerateMock<IHasher>();
            var hasherRight = MockRepository.GenerateMock<IHasher>();
            var comparison = MockRepository.GenerateMock<IComparison>();
            hasherLeft.Expect(x => x.ExamineDir(folderLeft, folderLeft));
            hasherRight.Expect(x => x.ExamineDir(folderRight, folderRight));
            var leftList = new Dictionary<string, string>();
            var rightList = new Dictionary<string, string>();
            hasherLeft.Expect(x => x.GetFileHashList()).Return(leftList);
            hasherRight.Expect(x => x.GetFileHashList()).Return(rightList);
            comparison.Expect(x => x.Compare(leftList, rightList));
            var compare = new Compare(hasherLeft, hasherRight, comparison);
            //Test
            var result = compare.Go(folderLeft, folderRight);

            //Assert

            Assert.That(result.AreEqual(), Is.True);
            comparison.VerifyAllExpectations();
            hasherLeft.VerifyAllExpectations();
            hasherRight.VerifyAllExpectations();
        }


        [Test]
        public void ShouldCompareTwoDifferentFolders()
        {
            var missingFile = @"a1\File1.txt";
            var folderLeft = @"root";
            var folderRight = Guid.NewGuid().ToString();
            //Copy folder and delete a file so they are different
            Copy.CopyDirectory(folderLeft, folderRight);
            File.Delete(Path.Combine(folderRight, missingFile));

            var hasherLeft = MockRepository.GenerateMock<IHasher>();
            var leftList = new Dictionary<string, string>();
            hasherLeft.Expect(x => x.ExamineDir(folderLeft, folderLeft));
            hasherLeft.Expect(x => x.GetFileHashList()).Return(leftList);

            var hasherRight = MockRepository.GenerateMock<IHasher>();
            hasherRight.Expect(x => x.ExamineDir(folderRight, folderRight));
            var rightList = new Dictionary<string, string>();
            hasherRight.Expect(x => x.GetFileHashList()).Return(rightList);

            var comparison = MockRepository.GenerateMock<IComparison>();
            var rtn = new List<FileData> { new FileData(missingFile, new Err(), ErrorTypes.Missing) };
            comparison.Expect(x => x.Compare(leftList, rightList));
            var compare = new Compare(hasherLeft, hasherRight, comparison);
            //Test
            var result = compare.Go(folderLeft, folderRight);

            //Assert
            Assert.That(result.AreEqual(), Is.False);
            Assert.That(result.Diff[0].Name, Is.EqualTo(missingFile));
            Assert.That(result.Diff[0].Type, Is.EqualTo(ErrorTypes.Missing));

            hasherLeft.VerifyAllExpectations();
            hasherRight.VerifyAllExpectations();
            comparison.VerifyAllExpectations();
            //Cleanup
            Directory.Delete(folderRight,true);
        }
    }
}