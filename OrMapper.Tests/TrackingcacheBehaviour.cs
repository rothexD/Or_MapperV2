using System;
using FluentAssertions;
using NUnit.Framework;
using ORMapper.Caches;
using OrMapper.Tests.TestClasses;

namespace OrMapper.Tests
{
    public class TrackingcacheBehaviour
    {
        private TrackingCache cache;
        [SetUp]
        public void setup()
        {
            cache = new();
        }
        [Test]
        public void OrmTrackingCache_Get_Valid()
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
        public void OrmTrackingCache_Add_Valid()
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
        public void OrmTrackingCache_Remove_Valid()
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
        public void OrmTrackingCache_Remove2_Valid()
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
        public void OrmTrackingCache_Contains_Valid()
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
        public void OrmTrackingCache_Contains_NotValid()
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
        public void OrmTrackingCache_Haschanged_true()
        {
            var obj1 = new Class();
            obj1.ID = "1";
            obj1.Name = "hans";
            obj1.Teacher = new();
            
            cache.HasChanged(obj1).Should().BeTrue();
        }
        [Test]
        public void OrmTrackingCache_Haschanged_false()
        {
            var obj1 = new Class();
            obj1.ID = "1";
            obj1.Name = "hans";
            obj1.Teacher = new();
            
            cache.Add(obj1);
            cache.HasChanged(obj1).Should().BeFalse();
        }
        [Test]
        public void OrmTrackingCache_Haschanged_true2()
        {
            var obj1 = new Class();
            obj1.ID = "1";
            obj1.Name = "hans";
            obj1.Teacher = new();
            
            cache.Add(obj1);
            obj1.Name = "fritz";
            cache.HasChanged(obj1).Should().BeTrue();
        }
        [Test]
        public void OrmTrackingCache_Haschanged_Exception()
        {
            var obj1 = new Class();
            obj1.ID = "1";
            obj1.Name = "hans";
            cache.Add(obj1);
            obj1.Name = "fritz";
            
            Action act = () =>cache.HasChanged(null);

            act.Should().Throw<Exception>();
        }
    }
}