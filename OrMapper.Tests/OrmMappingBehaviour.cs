using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using ORMapper;

namespace OrMapper.Tests
{
    public class Tests
    {
        private enum MyTestEnum
        {
            Test,
            Cookie
        }
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void OrmMapping_ToDataBaseType_Int_integer()
        {
            string result;

            result = OrmMapping.ToDatabaseType(typeof(int));

            result.ToLower().Should().Be("integer");
        }
        [Test]
        public void OrmMapping_ToDataBaseType_Short_smallint()
        {
            string result;

            result = OrmMapping.ToDatabaseType(typeof(short));

            result.ToLower().Should().Be("smallint");
        }
        [Test]
        public void OrmMapping_ToDataBaseType_Long_bigint()
        {
            string result;

            result = OrmMapping.ToDatabaseType(typeof(long));

            result.ToLower().Should().Be("bigint");
        }
        [Test]
        public void OrmMapping_ToDataBaseType_FLoat_Numeric()
        {
            string result;

            result = OrmMapping.ToDatabaseType(typeof(float));

            result.ToLower().Should().Be("numeric");
        }
        [Test]
        public void OrmMapping_ToDataBaseType_Double_Numeric()
        {
            string result;

            result = OrmMapping.ToDatabaseType(typeof(double));

            result.ToLower().Should().Be("numeric");
        }
        [Test]
        public void OrmMapping_ToDataBaseType_String_Text()
        {
            string result;

            result = OrmMapping.ToDatabaseType(typeof(string));

            result.ToLower().Should().Be("text");
        }
        [Test]
        public void OrmMapping_ToDataBaseType_Char_text()
        {
            string result;

            result = OrmMapping.ToDatabaseType(typeof(char));

            result.ToLower().Should().Be("text");
        }
        [Test]
        public void OrmMapping_ToDataBaseType_DateTime_timestamp()
        {
            string result;

            result = OrmMapping.ToDatabaseType(typeof(DateTime));

            result.ToLower().Should().Be("timestamp");
        }
        [Test]
        public void OrmMapping_ToDataBaseType_InValid_exception()
        {
            Action act;
            
            act = () => OrmMapping.ToDatabaseType(typeof(SystemException));

            act.Should().Throw<Exception>();
        }
        [Test]
        public void OrmMapping_ParseEnum_ValidEnum_SqlStringEnum()
        {
            List<string> results = new();
            Type[] enumArray = new[] {typeof(MyTestEnum)};
            
            
            OrmMapping.ParseEnums(results,enumArray);

            results.Should().HaveCount(1);
            results.Should().Contain("DO $$ BEGIN  CREATE TYPE MyTestEnum AS Enum ('Test','Cookie');  EXCEPTION  WHEN duplicate_object THEN null;  END $$;");
        }
    }
}