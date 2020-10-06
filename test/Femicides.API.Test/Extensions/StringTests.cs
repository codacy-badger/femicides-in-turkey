using Xunit;
using System.Collections.Generic;
using Femicides.API.Extensions;
using Microsoft.Extensions.Primitives;

namespace Femicides.API.Test.Extensions
{
    public class StringTests
    {
        #region -
        [Fact]
        public void AreThereNecessaryQueries()
        {
            var scenario = new List<KeyValuePair<string, StringValues>>();

            var resultOne = scenario.ToArray().AreThereNecessaryQueries();
            Assert.Equal(false, resultOne);

            scenario.Add(new KeyValuePair<string, StringValues>("page","1"));
            var resultTwo = scenario.ToArray().AreThereNecessaryQueries();
            Assert.Equal(false, resultTwo);
            scenario.Clear();

            scenario.Add(new KeyValuePair<string, StringValues>("page","1"));
            scenario.Add(new KeyValuePair<string, StringValues>("paramname","query"));
            var resultThree = scenario.ToArray().AreThereNecessaryQueries();
            Assert.Equal(true, resultThree);
        }

        [Fact]
        public void ToStringWithOutPageParam()
        {
            var scenario = new List<KeyValuePair<string, StringValues>>();
            var resultOne = scenario.ToArray().ToStringWithOutPageParam();
            Assert.Equal(null, resultOne);
            scenario.Clear();

            scenario.Add(new KeyValuePair<string, StringValues>("page","1"));
            var resultTwo = scenario.ToArray().ToStringWithOutPageParam();
            Assert.Equal(null, resultTwo);
            scenario.Clear();

            scenario.Add(new KeyValuePair<string, StringValues>("param1","query"));
            scenario.Add(new KeyValuePair<string, StringValues>("param2","query"));
            var resultThree = scenario.ToArray().ToStringWithOutPageParam();
            Assert.Equal("?param1=query&param2=query", resultThree);
            scenario.Clear();

            scenario.Add(new KeyValuePair<string, StringValues>("page","1"));
            scenario.Add(new KeyValuePair<string, StringValues>("param1","query"));
            scenario.Add(new KeyValuePair<string, StringValues>("param2","query"));
            var resultFour = scenario.ToArray().ToStringWithOutPageParam();
            Assert.Equal("?param1=query&param2=query", resultFour);
            scenario.Clear();

            scenario.Add(new KeyValuePair<string, StringValues>("param1","query"));
            scenario.Add(new KeyValuePair<string, StringValues>("param2","query"));
            scenario.Add(new KeyValuePair<string, StringValues>("page","1"));
            var resultFive = scenario.ToArray().ToStringWithOutPageParam();
            Assert.Equal("?param1=query&param2=query", resultFive);
            scenario.Clear();

            scenario.Add(new KeyValuePair<string, StringValues>("param1","query"));
            scenario.Add(new KeyValuePair<string, StringValues>("page","1"));
            scenario.Add(new KeyValuePair<string, StringValues>("param2","query"));
            var resultSix = scenario.ToArray().ToStringWithOutPageParam();
            Assert.Equal("?param1=query&param2=query", resultSix);
            scenario.Clear();
        }
        [Fact]
        public void PaginateWithParams()
        {
            string s = "https://femicidesinturkey.com/api/victim";
            var scenario = new List<KeyValuePair<string, StringValues>>();
            scenario.Add(new KeyValuePair<string, StringValues>("param1","query"));
            scenario.Add(new KeyValuePair<string, StringValues>("param2","query"));
            scenario.Add(new KeyValuePair<string, StringValues>("page", ""));

            var selectedPage = 1;
            var jump = -1;

            var resultOne = s.PaginateWithParams(totalPage: 150, selectedPage, scenario.ToArray().ToStringWithOutPageParam(), jump);   //prev
            Assert.Equal(null, resultOne);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            selectedPage = 100;
            jump = 1;
            var resultTwo = s.PaginateWithParams(totalPage: 150, selectedPage, scenario.ToArray().ToStringWithOutPageParam(), jump);   //next
            Assert.Equal(s + "?param1=query&param2=query&page=" + (selectedPage + jump), resultTwo);
            jump = -1;
            var resultThree = s.PaginateWithParams(totalPage: 150, selectedPage, scenario.ToArray().ToStringWithOutPageParam(), jump); //prev
            Assert.Equal(s + "?param1=query&param2=query&page=" + (selectedPage + jump), resultThree);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            selectedPage = 150;
            jump = 1;
            var resultFour = s.PaginateWithParams(totalPage: 150, selectedPage, scenario.ToArray().ToStringWithOutPageParam(), jump);  //next
            Assert.Equal(null, resultFour);
            jump = -1;
            var resultFive = s.PaginateWithParams(totalPage: 150, selectedPage, scenario.ToArray().ToStringWithOutPageParam(), jump);  //prev
            Assert.Equal(s + "?param1=query&param2=query&page=" + (selectedPage + jump), resultFive);
        }
        #endregion
    }
}
