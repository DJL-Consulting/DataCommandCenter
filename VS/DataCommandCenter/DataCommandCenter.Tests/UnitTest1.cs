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
        public async Task TestMethod1()
        {
            var context = new DataCommandCenterContext();

            var a = new DCCRepository(context);

            var b = await a.GetServers();

            var c = b;
        }
    }
}