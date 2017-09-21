using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace Sushi2.Tests
{
    [TestClass]
    public class GenericMemoryCache
    {
        [TestMethod]
        public void CacheTest()
        {
            IMemoryCache fembotCache = new MemoryCache(new MemoryCacheOptions());            
            
            // create
            Fembot f1 = fembotCache.GetOrCreate(1, entry =>
            {
                var fembot = new Fembot { Name = "Aoba" };
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(2));
                return fembot;
            });

            Fembot f2 = fembotCache.Set(2, new Fembot { Name = "Nenecchi" });
            Fembot f3 = fembotCache.GetOrCreate(1, entry =>
            {
                var fembot = new Fembot { Name = "Aoba" };
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(2));
                return fembot;
            });

            Assert.AreEqual(f1.Id, f3.Id);

            fembotCache.Remove(2);

            Fembot fake = fembotCache.GetOrCreate(2, entry =>
            {
                var fembot = new Fembot { Name = "Nenecchi" };
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(2));
                return fembot;
            });

            Assert.AreNotEqual(f2.Id, fake.Id);

            // force expiration
            Thread.Sleep(TimeSpan.FromSeconds(5));

            Fembot upcoming = fembotCache.GetOrCreate(1, entry =>
            {
                var fembot = new Fembot { Name = "Umiko" };
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(2));
                return fembot;
            });

            Assert.AreNotEqual(f1.Name, upcoming.Name);            
        }       

        public class Fembot
        {            
            public Guid Id { get; }
            public string Name { get; set; }         

            public Fembot()
            {
                Id = Guid.NewGuid();
            }
        }
    }
}
