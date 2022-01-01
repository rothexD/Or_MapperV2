using System.Linq.Expressions;
using FluentAssertions;
using NUnit.Framework;
using ORMapper.Caches;
using ORMapper.Models;
using OrMapper.Tests.TestClasses;

namespace OrMapper.Tests
{
    public class CacheBehaviour
    {
        private Cache cache;
        [SetUp]
        public void setup()
        {
            cache = new();
        }
        [Test]
        public void OrmCache_Get_Valid()
        {
            var obj = new Class();
            obj.ID = "1";
            
            cache.storage[typeof(Class)] = new();
            cache.storage[typeof(Class)]["1"] = obj;
            
            
            var x = cache.Get(typeof(Class), "1");
            cache.storage.Count.Should().Be(1);
            cache.storage[typeof(Class)].Count.Should().Be(1);
            x.Should().Be(obj);
        }
        [Test]
        public void OrmCache_Add_Valid()
        {
            var obj = new Class();
            obj.ID = "1";
            
            cache.Add(obj);
            var result = cache.storage[typeof(Class)]["1"];
            
            cache.storage.Count.Should().Be(1);
            cache.storage[typeof(Class)].Count.Should().Be(1);
            
            result.Should().Be(obj);
        }
        [Test]
        public void OrmCache_Remove_Valid()
        {
            var obj = new Class();
            obj.ID = "1";
            
            cache.Add(obj);
            cache.storage.Count.Should().Be(1);
            cache.storage[typeof(Class)].Count.Should().Be(1);
            
            cache.Remove(obj);
            cache.storage.Count.Should().Be(1);
            cache.storage[typeof(Class)].Count.Should().Be(0);
        }
        [Test]
        public void OrmCache_Remove2_Valid()
        {
            var obj = new Class();
            obj.ID = "1";
            
            cache.Add(obj);
            cache.storage.Count.Should().Be(1);
            cache.storage[typeof(Class)].Count.Should().Be(1);
            
            cache.Remove(obj.GetType(), "1");
            cache.storage.Count.Should().Be(1);
            cache.storage[typeof(Class)].Count.Should().Be(0);
        }
        [Test]
        public void OrmCache_Contains_Valid()
        {
            var obj1 = new Class();
            obj1.ID = "1";
            var obj2 = new Class();
            obj2.ID = "2";
            
            cache.storage[typeof(Class)] = new();
            cache.storage[typeof(Class)]["1"] = obj1;
            cache.storage[typeof(Class)]["2"] = obj2;

            var result = cache.Contains(typeof(Class),"1");
            result.Should().Be(true);
        }
        [Test]
        public void OrmCache_Contains_NotValid()
        {
            var obj1 = new Class();
            obj1.ID = "1";
            var obj2 = new Class();
            obj2.ID = "2";
            
            cache.storage[typeof(Class)] = new();
            cache.storage[typeof(Class)]["1"] = obj1;

            var result = cache.Contains(typeof(Class),"2");
            result.Should().Be(false);
        }
        
        [Test]
        public void OrmCache_Haschanged_alwaystrue()
        {
            var obj1 = new Class();
            obj1.ID = "1";

            cache.HasChanged(obj1).Should().BeTrue();
        }
    }
}