using System.Collections.Generic;
using System.Data;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using OrMapper.Helpers;

namespace OrMapper.Tests
{
    public class ExtentionTest
    {
        [Test]
        public void Dbextentions_connection_open()
        {
            var dbConnMock = A.Fake<IDbConnection>();
            A.CallTo(() => dbConnMock.Open()).WithAnyArguments().DoesNothing();
            
            Helpers.extentions.DbExtentions.Open(dbConnMock);
            A.CallTo(() => dbConnMock.Open()).MustHaveHappenedOnceExactly();
        }
        [Test]
        public void Dbextentions_connection_close()
        {
            var dbConnMock = A.Fake<IDbConnection>();
            A.CallTo(() => dbConnMock.Close()).WithAnyArguments().DoesNothing();

            Helpers.extentions.DbExtentions.Close(dbConnMock);
            A.CallTo(() => dbConnMock.Close()).MustHaveHappenedOnceExactly();
        }
        [Test]
        public void Dbextentions_command_open()
        {
            var commandMock = A.Fake<IDbCommand>();
            A.CallTo(() => commandMock.ExecuteNonQuery()).WithAnyArguments().Returns(0);

            Helpers.extentions.DbExtentions.ExecuteNonQuery(commandMock);
            A.CallTo(() => commandMock.ExecuteNonQuery()).WithAnyArguments().MustHaveHappenedOnceExactly();
        }
        [Test]
        public void Dbextentions_command_close()
        {
            var commandMock = A.Fake<IDbCommand>();
            A.CallTo(() => commandMock.ExecuteReader()).WithAnyArguments().Returns(null);
            
            Helpers.extentions.DbExtentions.ExecuteReader(commandMock);
            A.CallTo(() => commandMock.ExecuteReader()).WithAnyArguments().MustHaveHappenedOnceExactly();
        }
        
        [Test]
        public void Dbextentions_command_parahelper()
        {
            var commandMock = A.Fake<IDbCommand>();
            
            Helpers.extentions.Parameterhelper.HelpWithParameter(commandMock,"1","2");

            A.CallTo(() => commandMock.Parameters.Add(null)).WithAnyArguments().MustHaveHappenedOnceExactly();
        }
    }
}