using Xunit.Sdk;
using DataCommandCenter.DAL;
using DataCommandCenter.DAL.Services;
using DataCommandCenter.DAL.Models;

namespace DataCommandCenter.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var context = new DataCommandCenterContext();

            var a = new DCCRepository(context);

            var b = a.GetServers();

            var c = b;
        }
    }
}