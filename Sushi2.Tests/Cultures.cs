using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sushi2.Tests
{
    [TestClass]
    public class Cultures
    {
        [TestMethod]
        public void English()
        {
            var ci = new CultureInfo("en-us");
            Assert.AreEqual(ci.LCID, Sushi2.Cultures.English.LCID);
        }

        [TestMethod]
        public void Czech()
        {
            var ci = new CultureInfo("cs-cz");
            Assert.AreEqual(ci.LCID, Sushi2.Cultures.Czech.LCID);
        }

        [TestMethod]
        public void Invariant()
        {
            Assert.AreEqual(CultureInfo.InvariantCulture.LCID, Sushi2.Cultures.Invariant.LCID);
        }
    }
}