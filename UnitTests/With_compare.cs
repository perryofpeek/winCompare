namespace UnitTests
{
    using System.Collections.Generic;
    using WinCompare;
    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class With_Comparison
    {
        [Test]
        public void should_find_no_differences_between_left_and_right()
        {
            var comparison = new Comparison();
            var left = new Dictionary<string, string>();
            var right = new Dictionary<string, string>();
            left.Add("A", "1");
            left.Add("B", "2");
            right.Add("A", "1");
            right.Add("B", "2");
            comparison.Compare(left, right);
            Assert.That(comparison.AreSame(), Is.EqualTo(true));           
        }

        [Test]
        public void should_find_one_difference_in_hash_between_left_and_right()
        {
            var comparison = new Comparison();
            var left = new Dictionary<string, string>();
            var right = new Dictionary<string, string>();
            left.Add("A", "1");
            left.Add("AA", "4");
            left.Add("B", "2");
            right.Add("A", "1");
            right.Add("AA", "4");
            right.Add("B", "1");
            comparison.Compare(left, right);
            Assert.That(comparison.AreSame(), Is.EqualTo(false));
            Assert.That(comparison.Diff.Count, Is.EqualTo(1));
            Assert.That(comparison.Same.Count, Is.EqualTo(2));            
            Assert.That(comparison.Diff["B"].GetType(), Is.EqualTo(typeof(DifferentHashResult)));
            var item = (DifferentHashResult)  comparison.Diff["B"];
            Assert.That(item.LeftHash, Is.EqualTo("2"));
            Assert.That(item.RightHash, Is.EqualTo("1"));
        }

        [Test]
        public void should_find_one_difference_missing_on_left_between_left_and_right()
        {
            var comparison = new Comparison();
            var left = new Dictionary<string, string>();
            var right = new Dictionary<string, string>();
            left.Add("A", "1");           
            left.Add("B", "2");
            right.Add("A", "1");
            right.Add("B", "2");
            right.Add("AA", "4");
            comparison.Compare(left, right);
            Assert.That(comparison.AreSame(), Is.EqualTo(false));
            Assert.That(comparison.Diff.Count, Is.EqualTo(1));
            Assert.That(comparison.Same.Count, Is.EqualTo(2));
            Assert.That(comparison.Diff["AA"].GetType(), Is.EqualTo(typeof(MissingOnLeftSideResult)));
            var item = (MissingOnLeftSideResult)comparison.Diff["AA"];
            Assert.That(item.Hash, Is.EqualTo("4"));
        }

        [Test]
        public void should_find_one_difference_missing_on_right_between_left_and_right()
        {
            var comparison = new Comparison();
            var left = new Dictionary<string, string>();
            var right = new Dictionary<string, string>();
            left.Add("A", "1");
            left.Add("B", "2");
            left.Add("AA", "4");
            right.Add("A", "1");
            right.Add("B", "2");            
            comparison.Compare(left, right);
            Assert.That(comparison.AreSame(), Is.EqualTo(false));
            Assert.That(comparison.Diff.Count, Is.EqualTo(1));
            Assert.That(comparison.Same.Count, Is.EqualTo(2));
            Assert.That(comparison.Diff["AA"].GetType(), Is.EqualTo(typeof(MissingOnRightSideResult)));
            var item = (MissingOnRightSideResult)comparison.Diff["AA"];
            Assert.That(item.Hash, Is.EqualTo("4")); 
        }

        [Test]
        public void should_find_differences_missing_on_both_between_left_and_right()
        {
            var comparison = new Comparison();
            var left = new Dictionary<string, string>();
            var right = new Dictionary<string, string>();
            left.Add("A", "1");
            left.Add("B", "2");
            left.Add("AA", "4");
            right.Add("A", "1");
            right.Add("B", "2");
            right.Add("BB", "5");
            comparison.Compare(left, right);
            Assert.That(comparison.AreSame(), Is.EqualTo(false));
            Assert.That(comparison.Diff.Count, Is.EqualTo(2));
            Assert.That(comparison.Same.Count, Is.EqualTo(2));
            Assert.That(comparison.Diff["AA"].GetType(), Is.EqualTo(typeof(MissingOnRightSideResult)));
            Assert.That(comparison.Diff["BB"].GetType(), Is.EqualTo(typeof(MissingOnLeftSideResult)));
            var item1 = (MissingOnRightSideResult)comparison.Diff["AA"];
            Assert.That(item1.Hash, Is.EqualTo("4"));
            var item2 = (MissingOnLeftSideResult)comparison.Diff["BB"];
            Assert.That(item2.Hash, Is.EqualTo("5"));
        }

        [Test]
        public void should_return_timespan_of_time_taken_to_compare()
        {
            var comparison = new Comparison();
            var left = new Dictionary<string, string>();
            var right = new Dictionary<string, string>();
            left.Add("A", "1");
            left.Add("B", "2");
            left.Add("AA", "4");
            right.Add("A", "1");
            right.Add("B", "2");
            right.Add("BB", "5");
            comparison.Compare(left, right);
            Assert.That(comparison.ExecutionTime, Is.Not.Null);            
        }
    }
}