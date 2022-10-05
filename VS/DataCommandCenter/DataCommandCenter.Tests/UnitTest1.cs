using Xunit.Sdk;
using DataCommandCenter.DAL;
using DataCommandCenter.DAL.Services;
using DataCommandCenter.DAL.Models;
using DataCommandCenter.DAL.DTO;
using AutoMapper;
using System.Reflection;
using DataCommandCenter.DAL.Profiles;

namespace DataCommandCenter.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var context = new DataCommandCenterContext();

            var myobj = context.ObjectSearches.Where(o => o.Id == 251).FirstOrDefault();

            var a = new DCCRepository(context);

            //var b = await a.GetLineageForObject(myobj);

            var retObj = new LineageDTO();

            var prf = new MetadataProfile();

            //var _mapper = new Mapper(prf.Conf );

            var (flows, nodes) = await a.GetLineageForObject(myobj);

            //retObj.Nodes = _mapper.Map<IEnumerable<LineageNode>>(nodes);
            //retObj.Flows = _mapper.Map<IEnumerable<LineageLink>>(flows);

            var c = nodes;
        }
    }
}