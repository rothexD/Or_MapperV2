using System;
using System.Collections.Generic;
using System.Data;
using FakeItEasy;
using FluentAssertions;
using Npgsql;
using NUnit.Framework;
using ORMapper;
using ORMapper.Models;

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
        [Test]
        public void OrmMapping_ParseTables_Valid_SqlStringParams()
        {
            List<string> results = new();
            Type[] tableArray = new[] {typeof(Class)};
            
            
            OrmMapping.ParseTables(results,tableArray);

            results.Should().HaveCount(5);
            results.Should().Contain("Create Table IF NOT EXISTS CLASSES (ID text  not null  primary key , Name text  not null , KTEACHER text not null ,kstudent text not null );");
            results.Should().Contain("ALTER TABLE CLASSES DROP CONSTRAINT IF EXISTS \"fk_CLASSES_KTEACHER_CLASSES_ID\";");
            results.Should().Contain("Alter table CLASSES ADD Constraint \"fk_CLASSES_KTEACHER_CLASSES_ID\" Foreign Key (KTEACHER) references TEACHERS(ID) on update cascade on delete cascade;");
            results.Should().Contain("ALTER TABLE STUDENT_COURSES DROP CONSTRAINT IF EXISTS \"fk_CLASSES_kstudent_STUDENT_COURSES_ID\";");
            results.Should().Contain("Alter table STUDENT_COURSES ADD Constraint \"fk_CLASSES_kstudent_STUDENT_COURSES_ID\" Foreign Key (kstudent) references CLASSES(ID) on update cascade on delete cascade;");
        }
        
        [Test]
        public void OrmMapping_Map_Valid_SqlStringParams()
        {
            List<string> results = new();
            Type[] tableArray = new[] {typeof(Class),typeof(MyTestEnum)};
            
            
            results = OrmMapping.Map(tableArray,false);
            results.Should().HaveCount(6);
        }
        
        [Test]
        public void OrmModels_InsertIntoDb_ValidSql_string()
        {
            List<string> sqlstrings = new();
            var dbConnMock = A.Fake<IDbConnection>();
            var commandMock = A.Fake<IDbCommand>();
            A.CallTo(() => dbConnMock.Open()).WithAnyArguments().DoesNothing();
            A.CallTo(() => dbConnMock.Close()).WithAnyArguments().DoesNothing();
            A.CallTo(() => dbConnMock.CreateCommand()).WithAnyArguments().Returns(commandMock);
            A.CallTo(() => commandMock.Dispose()).WithAnyArguments().DoesNothing();

            sqlstrings.Add("1");
            sqlstrings.Add("2");

            OrmMapping._InsertIntoDb(sqlstrings,dbConnMock);

            A.CallTo(() => commandMock.ExecuteNonQuery()).WithAnyArguments().MustHaveHappenedTwiceExactly();
        }
        
        [Test]
        public void OrmMapping_ManyToMany()
        {
            var sqlCommands = new List<string>();
            Type[] tables = new[] {typeof(STUDENT_COURSES)};

            OrmMapping.ParseTables(sqlCommands,tables);

            sqlCommands.Count.Should().Be(1);
        }
    }
}