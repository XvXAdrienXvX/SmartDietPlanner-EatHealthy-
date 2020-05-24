using CoreDietPlan.Models;
using System;
using Xunit;

namespace TimeManagement
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            TestManagement k = new TestManagement();

            Assert.Equal(1, k.testing());
        }
    }
}
