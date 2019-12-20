using System;
using Xunit;
using System.Linq;

namespace RobotCleaner.Test
{
    public class RobotTests
    {
        private Robot _robot;

        public RobotTests()
        {
            _robot = new Robot();
            _robot.SetRobotStartLocation(0, 0);
        }

        [Fact]
        public void CanSetRobotStartLocation()
        {
            var robot = new Robot();
            robot.SetRobotStartLocation(50, 100);
            Assert.Equal((robot.CurrentLocation.X, robot.CurrentLocation.Y), (50, 100));
        }

        [Theory]
        [InlineData('E', 1, "1 0")]
        [InlineData('W', 10, "-10 0")]
        [InlineData('N', 100, "0 100")]
        [InlineData('S', 1000, "0 -1000")]
        public void TestMoveRobot(char dir, int nrOfSteps, string expectedCoordinatesString)
        {
            _robot.ExecuteCommand(new Command(dir, nrOfSteps));

            var expectedCoordinates = expectedCoordinatesString.Split(' ').Select(c => Convert.ToInt32(c)).ToArray();

            Assert.Equal((_robot.CurrentLocation.X, _robot.CurrentLocation.Y), (expectedCoordinates[0], expectedCoordinates[1]));
        }
    }
}