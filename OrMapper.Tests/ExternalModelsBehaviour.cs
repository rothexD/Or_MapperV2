using System;
using FluentAssertions;
using NUnit.Framework;
using OrMapper.ExternalModels;
using OrMapper.Helpers.extentions;
using ORMapper.Models;
using OrMapper.Tests.TestClasses;

namespace OrMapper.Tests
{
    public class ExternalModelsBehaviour
    {
        [Test]
        public void MakeCaseInsensitive_extention()
        {
            var result = "1".MakeCaseIns();
            result.Parameter.Should().Be("1");
            result.Parameter.Should().BeOfType(typeof(string));
        }
        [Test]
        public void MakeSecureParam_extention()
        {
            var result = "1".MakeSecure();
            result.Parameter.Should().Be("1");
            result.Parameter.Should().BeOfType(typeof(string));
        }
        [Test]
        public void MakeSecureParam_MakeSecure_argumentexception()
        {
            Action result = () => SecureParameter.Create("1".MakeSecure());

            result.Should().Throw<ArgumentException>();
        }
        [Test]
        public void MakeSecureParam_MakeCaseIns_argumentexception()
        {
            Action result = () => SecureParameter.Create("1".MakeCaseIns());

            result.Should().Throw<ArgumentException>();
        }
        [Test]
        public void MakeCaseIns_MakeCaseIns_argumentexception()
        {
            Action result = () => CaseInsensitive.Create("1".MakeCaseIns());

            result.Should().Throw<ArgumentException>();
        }
        [Test]
        public void MakeCaseIns_null_DBValueNull()
        {
            var result = CaseInsensitive.Create(null);

            result.Parameter.Should().BeOfType(typeof(DBNull));
        }
        [Test]
        public void MakeSecure_null_DBValueNull()
        {
            var result = SecureParameter.Create(null);

            result.Parameter.Should().BeOfType(typeof(DBNull));
        }
    }
}