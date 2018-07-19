using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sushi2.Tests
{
    [TestClass]
    public class RandomWordGenerator
    {
        [TestMethod]
        public void GetSome()
        {
            var word = Sushi2.RandomWordGenerator.GetWord(3);

            Assert.IsNotNull(word);
            Assert.AreNotEqual("", word);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Bad number of syllables was inappropriately allowed.")]
        public void WrongSyllable()
        {
            Sushi2.RandomWordGenerator.GetWord(0);
        }
    }
}