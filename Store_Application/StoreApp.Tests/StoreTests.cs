using Moq;
using StoreApp.App;
using StoreApp.DataInfrastructure;
using StoreApp.Logic;
using System;
using System.Collections.Generic;
using Xunit;

namespace StoreApp.Tests
{
    public class StoreTests
    {
        [Fact]
        public void GetLocationTest()
        {
            Mock<IRepository> mockRepo = new();
            List<Location> locations = new List<Location>()
            {
                new (1, "1551 3rd Ave, New York, NY 10128"),
                new (2, "367 Russell St, Hadley, MA 01035"),
                new (3, "613 Washington Blvd, Jersey City, NJ 07310")
            };
            mockRepo.Setup(x => x.GetLocationList()).Returns(locations);
            var store = new Store(mockRepo.Object);
            string result = store.GetLocations();
            var expected = "ID\tStore Location\r\n---------------------------------------------------------------\r\n1\t[1551 3rd Ave, New York, NY 10128]\r\n2\t[367 Russell St, Hadley, MA 01035]\r\n3\t[613 Washington Blvd, Jersey City, NJ 07310]\r\n---------------------------------------------------------------\r\n";
            Assert.Equal(expected, result);
        }
    }
}
