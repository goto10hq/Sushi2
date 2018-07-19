using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sushi2.Tests
{
    [TestClass]
    public class HashTools
    {
        [TestMethod]
        public void Default()
        {
            var hash = Sushi2.HashTools.GetHash("test");
            Assert.AreEqual("fe520676b1a1d93dabab2319eea03674f3632eaeeb163d1e88244f5eb1de10eb", hash);
        }

        [TestMethod]
        public void Sha256WithAsciiEncoding()
        {
            var hash = Sushi2.HashTools.GetHash("žluťoučký", Sushi2.HashTools.HashType.SHA256, Encoding.ASCII);
            Assert.AreEqual("e9a581eb9201da5644e8fd13eb1705e2c0020fb565e4e4f67a6814c2e4cf909b", hash);
        }

        [TestMethod]
        public void Sha256WithUtf7Encoding()
        {
            var hash = Sushi2.HashTools.GetHash("žluťoučký", Sushi2.HashTools.HashType.SHA256, Encoding.UTF8);
            Assert.AreEqual("ebde13e62a6c7cbec252867d7711d37ae252562d3fbca5f537e74a2f90f01e6b", hash);
        }

        [TestMethod]
        public void Md5()
        {
            var hash = Sushi2.HashTools.GetHash("test", Sushi2.HashTools.HashType.MD5);
            Assert.AreEqual("c8059e2ec7419f590e79d7f1b774bfe6", hash);
        }

        [TestMethod]
        public void Sha1()
        {
            var hash = Sushi2.HashTools.GetHash("test", Sushi2.HashTools.HashType.SHA1);
            Assert.AreEqual("87f8ed9157125ffc4da9e06a7b8011ad80a53fe1", hash);
        }

        [TestMethod]
        public void Sha256()
        {
            var hash = Sushi2.HashTools.GetHash("test", Sushi2.HashTools.HashType.SHA256);
            Assert.AreEqual("fe520676b1a1d93dabab2319eea03674f3632eaeeb163d1e88244f5eb1de10eb", hash);
        }

        [TestMethod]
        public void Sha384()
        {
            var hash = Sushi2.HashTools.GetHash("test", Sushi2.HashTools.HashType.SHA384);
            Assert.AreEqual("f62692128acdca72e77dd790ad5baab178e3f0773b99b44492ecf55777cf2085b491b4c477b7fcbec219d395d5cea65b", hash);
        }

        [TestMethod]
        public void Sha512()
        {
            var hash = Sushi2.HashTools.GetHash("test", Sushi2.HashTools.HashType.SHA512);
            Assert.AreEqual("9f7d8627e02f97cc5a52dcb2ba96038fe12f2a34b0fac50e041359ae13d5ede8a8a50562da58ba7916da378e7343ef91e85efbd6a0a70ab237ada4c2274df13d", hash);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Null text to hash was inappropriately allowed.")]
        public void NullText()
        {
            Sushi2.HashTools.GetHash(null);
        }
    }
}