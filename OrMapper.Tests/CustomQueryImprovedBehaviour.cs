using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using ORMapper;
using ORMapper.CustomQueryImproved;
using OrMapper.ExternalModels;
using OrMapper.Helpers.extentions;

namespace OrMapper.Tests
{
    public class CustomQueryImprovedBehaviour
    {
        [Test]
        public void CustomQueryImproved_functionalitytest1()
        {
            (string, List<(string,object)>) result;

            result = CustomQueryImproved.Create().OutputResult();

            result.Item1.Should().BeEmpty();
            result.Item2.Should().BeEmpty();
        }
        [Test]
        public void CustomQueryImproved_functionalitytest2()
        {
            (string, List<(string,object)>) result;

            result = CustomQueryImproved.Create().Where().Equals("1", "2").OutputResult();

            result.Item1.Should().Be(" WHERE  1 =  2 ");
            result.Item2.Count.Should().Be(0);
        }
        [Test]
        public void CustomQueryImproved_functionalitytest3()
        {
            (string, List<(string,object)>) result;

            result = CustomQueryImproved.Create().Where().Greater("1", "2").OutputResult();

            result.Item1.Should().Be(" WHERE  1 >  2 ");
            result.Item2.Count.Should().Be(0);
        }
        [Test]
        public void CustomQueryImproved_functionalitytest4()
        {
            (string, List<(string,object)>) result;

            result = CustomQueryImproved.Create().Where().Smaller("1", "2").OutputResult();

            result.Item1.Should().Be(" WHERE  1 <  2 ");
            result.Item2.Count.Should().Be(0);
        }
        [Test]
        public void CustomQueryImproved_functionalitytest5()
        {
            (string, List<(string,object)>) result;

            result = CustomQueryImproved.Create().Where().GreaterEquals("1", "2").OutputResult();

            result.Item1.Should().Be(" WHERE  1 >=  2 ");
            result.Item2.Count.Should().Be(0);
        }
        [Test]
        public void CustomQueryImproved_functionalitytest6()
        {
            (string, List<(string,object)>) result;

            result = CustomQueryImproved.Create().Where().SmallerEquals("1", "2").OutputResult();

            result.Item1.Should().Be(" WHERE  1 <=  2 ");
            result.Item2.Count.Should().Be(0);
        }
        [Test]
        public void CustomQueryImproved_functionalitytest7()
        {
            (string, List<(string,object)>) result;

            result = CustomQueryImproved.Create().Where().NotEquals("1", "2").OutputResult();

            result.Item1.Should().Be(" WHERE  1 !=  2 ");
            result.Item2.Count.Should().Be(0);
        }
        [Test]
        public void CustomQueryImproved_functionalitytest8()
        {
            (string, List<(string,object)>) result;

            result = CustomQueryImproved.Create().Where().Like("1", "2").OutputResult();

            result.Item1.Should().Be(" WHERE  1 LIKE  2 ");
            result.Item2.Count.Should().Be(0);
        }
        [Test]
        public void CustomQueryImproved_functionalitytest9()
        {
            (string, List<(string,object)>) result;

            result = CustomQueryImproved.Create().Where().NotLike("1", "2").OutputResult();

            result.Item1.Should().Be(" WHERE  1 NOT LIKE  2 ");
            result.Item2.Count.Should().Be(0);
        }
        [Test]
        public void CustomQueryImproved_functionalitytest10()
        {
            (string, List<(string,object)>) result;

            result = CustomQueryImproved.Create().Where().Equals("1", "2").And().Equals("1", "2").OutputResult();

            result.Item1.Should().Be(" WHERE  1 =  2  AND  1 =  2 ");
            result.Item2.Count.Should().Be(0);
        }
        [Test]
        public void CustomQueryImproved_functionalitytest11()
        {
            (string, List<(string,object)>) result;

            result = CustomQueryImproved.Create().Where().Equals("1", "2").Or().Equals("1", "2").OutputResult();

            result.Item1.Should().Be(" WHERE  1 =  2  OR  1 =  2 ");
            result.Item2.Count.Should().Be(0);
        }
        [Test]
        public void CustomQueryImproved_functionalitytest12()
        {
            (string, List<(string,object)>) result;

            result = CustomQueryImproved.Create().Where().BracketClose_().BracketOpen_().Equals("1","2").OutputResult();

            result.Item1.Should().Be(" WHERE )( 1 =  2 ");
            result.Item2.Count.Should().Be(0);
        }
        [Test]
        public void CustomQueryImproved_functionalitytest13()
        {
            (string, List<(string,object)>) result;

            result = CustomQueryImproved.Create().Where().Equals("1","2").BracketClose().BracketOpen().OutputResult();

            result.Item1.Should().Be(" WHERE  1 =  2 )(");
            result.Item2.Count.Should().Be(0);
        }
        [Test]
        public void CustomQueryImproved_functionalitytest14()
        {
            (string, List<(string,object)>) result;

            result = CustomQueryImproved.Create().Where().Equals("1".MakeSecure(),"2".MakeCaseIns()).OutputResult();

            result.Item1.Should().Be(" WHERE  :param0 =  LOWER( 2 )");
            result.Item2.Count.Should().Be(1);
            result.Item2[0].Item2.Should().Be("1");
        }
    }
}