using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace RobotCleaner.Test
{
    public class LineTests
    {
        private Location _origo;

        public LineTests()
        {
            _origo = new Location(0, 0);
        }

        [Theory]
        [InlineData('E', 3, "0 0,1 0,2 0,3 0")]
        [InlineData('W', 3, "0 0,-1 0,-2 0,-3 0")]
        [InlineData('N', 3, "0 0,0 1,0 2,0 3")]
        [InlineData('S', 3, "0 0,0 -1,0 -2,0 -3")]
        public void CanGetAllLineLocations(char direction, int nrOfSteps, string expectedLocationsString)
        {
            var command = new Command(direction, nrOfSteps);
            var line = new Line(_origo, command);

            var locationsString = new List<string>();
            
            locationsString = expectedLocationsString.Split(',').ToList();

            var expectedLocations = new List<Tuple<int, int>>();

            foreach (var locationString in locationsString)
            {
                var location = locationString.Split(' ').Select(c => Convert.ToInt32(c)).ToArray();
                expectedLocations.Add(new Tuple<int, int>(location[0], location[1]));
            }

            var locations = line.GetAllLineLocations().Select(l => new Tuple<int, int>(l.Item1, l.Item2)).ToList();
            for (var i = 0; i < locations.Count; i++)
            {
                Assert.Equal(expectedLocations[i], locations[i]);
            }

            Assert.Equal(expectedLocations.Count, locations.Count);
        }

        [Theory]
        [InlineData('E', 1, 1, 0)]
        [InlineData('W', 1, -1, 0)]
        [InlineData('N', 1, 0, 1)]
        [InlineData('S', 1, 0, -1)]
        public void CanGetEndLocation(char direction, int nrOfSteps, int expectedX, int expectedY)
        {
            var line = new Line(_origo, new Command(direction, nrOfSteps));
            Assert.Equal((line.GetEndLocation().X, line.GetEndLocation().Y), (expectedX, expectedY));
        }

        [Theory]
        [InlineData(0, 0, 'E', 5, 0, 1, 'W', 5, true)]
        [InlineData(0, 0, 'E', 5, 0, 0, 'W', 5, true)]
        [InlineData(0, 0, 'E', 5, 0, 0, 'N', 5, false)]
        [InlineData(0, 0, 'S', 5, 0, 0, 'N', 5, true)]
        [InlineData(0, 0, 'S', 5, 1, 0, 'N', 5, true)]
        [InlineData(0, 0, 'W', 5, 1, 0, 'N', 5, false)]
        [InlineData(0, 0, 'W', 5, 1, 0, 'S', 5, false)]
        public void TestCheckIfParallell(int startX1, int startY1, char dir1, int nrOfSteps1,
                int startX2, int startY2, char dir2, int nrOfSteps2, bool expected)
        {
            var l1 = new Line(new Location(startX1, startY1), new Command(dir1, nrOfSteps1));
            var l2 = new Line(new Location(startX2, startY2), new Command(dir2, nrOfSteps2));

            Assert.Equal(l1.CheckIfParallell(l2), expected);
        }

        [Theory]
        [InlineData(0, 0, 'E', 3, 0, 0, 'N', 3, "0 0")]
        [InlineData(0, 0, 'E', 3, 0, 1, 'E', 3, "")]
        [InlineData(0, 0, 'E', 2, 2, 0, 'W', 2, "0 0,1 0,2 0")]
        [InlineData(1, 1, 'N', 2, 1, 3, 'S', 2, "1 1,1 2,1 3")]
        public void TestGetOverlaps(int startX1, int startY1, char dir1, int nrOfSteps1,
                int startX2, int startY2, char dir2, int nrOfSteps2, string overlapsString)
        {
            var l1 = new Line(new Location(startX1, startY1), new Command(dir1, nrOfSteps1));
            var l2 = new Line(new Location(startX2, startY2), new Command(dir2, nrOfSteps2));

            var pointsString = new List<string>();

            if (overlapsString != "")
                pointsString = overlapsString.Split(',').ToList();

            var expectedOverlaps = new List<Tuple<int, int>>();

            foreach (var pointString in pointsString)
            {
                var coordinates = pointString.Split(' ').Select(c => Convert.ToInt32(c)).ToArray();
                expectedOverlaps.Add(new Tuple<int, int>(coordinates[0], coordinates[1]));
            }

            var overlaps = l1.GetOverlaps(l2).Select(l => new Tuple<int, int>(l.X, l.Y)).ToList();

            for (var i = 0; i < overlaps.Count; i++)
            {
                Assert.Equal(expectedOverlaps[i], overlaps[i]);
            }

            Assert.Equal(expectedOverlaps.Count, overlaps.Count);
        }
    }
}