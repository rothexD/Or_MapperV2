using System.Collections.ObjectModel;
using FluentAssertions;
using NUnit.Framework;
using ORMapper;
using ORMapper.Models;
using ShowcaseOrm.Models;

namespace TestProject1
{
    public class OrmTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Orm_SearchInCache_Found_object()
        {
            object result;
            object obj1 = new Class()
            {
                ID = "1",
            };
            object obj2 = new Class()
            {
                ID = "2",
            };
            Collection<object> localcache = new Collection<object>();
            
            localcache.Add(obj1);
            localcache.Add(obj2);


            result = Orm.SearchInCache(typeof(Class), "1", localcache);

            result.Should().Be(obj1);
        }
        [Test]
        public void Orm_SearchInCache_NotFound_null()
        {
            object result;
            object obj1 = new Class()
            {
                ID = "1",
            };
            object obj2 = new Class()
            {
                ID = "2",
            };
            Collection<object> localcache = new Collection<object>();
            
            localcache.Add(obj1);
            localcache.Add(obj2);


            result = Orm.SearchInCache(typeof(Class), "3", localcache);

            result.Should().Be(null);
        }
        [Test]
        public void OrmModels_TableCtor_Table_CorrectlyParsedTable()
        {
            Table result;

            result = new Table(typeof(Class));

            result.TableName.Should().Be("CLASSES");
            result.Columns.Should().HaveCount(3);
            result.Member.Should().Be(typeof(Class));
            result.Internals.Should().HaveCount(3);

            result.Internals[0].ColumnName.Should().Be("ID");
            result.Internals[0].IsPrimaryKey.Should().BeTrue();
            result.Internals[0].IsNullable.Should().BeFalse();
            result.Internals[0].IsForeignKey.Should().BeFalse();
            result.Internals[0].IsExternal.Should().BeFalse();
            result.Internals[0].IsManyToMany.Should().BeFalse();
            result.Internals[0].ColumnType.Should().Be(typeof(string));

            result.Internals[1].ColumnName.Should().Be("Name");
            result.Internals[1].IsPrimaryKey.Should().BeFalse();
            result.Internals[1].IsNullable.Should().BeFalse();
            result.Internals[1].IsForeignKey.Should().BeFalse();
            result.Internals[1].IsExternal.Should().BeFalse();
            result.Internals[1].IsManyToMany.Should().BeFalse();
            result.Internals[1].ColumnType.Should().Be(typeof(string));
            
            result.Internals[2].ColumnName.Should().Be("KTEACHER");
            result.Internals[2].IsPrimaryKey.Should().BeFalse();
            result.Internals[2].IsNullable.Should().BeFalse();
            result.Internals[2].IsForeignKey.Should().BeTrue();
            result.Internals[2].IsExternal.Should().BeFalse();
            result.Internals[2].IsManyToMany.Should().BeFalse();
            result.Internals[2].ColumnType.Should().Be(typeof(Teacher));
        }
        [Test]
        public void OrmModels_GetSqlForTable_ValidSql_string()
        {
            var obj = new Table(typeof(Class));
            string sql;
            
            sql = obj.GetSelectSql("");

            sql.Should().Be("Select ID, Name, KTEACHER From CLASSES");
        }
    }
}