using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace RobotCleaner.Test
{
    public class CleaningSessionTests
    {
        private Robot _robot;
        private CleaningSession _cleaningSession;

        public CleaningSessionTests()
        {
            _robot = new Robot();
            _robot.SetRobotStartLocation(0, 0);

            _cleaningSession = new CleaningSession(_robot);
        }
        
        private List<Command> getCommandsFromString(string commandsString)
        {
            var commands = new List<Command>();
            var commandsArrays = commandsString.Split(',').ToArray();
            foreach (var commandString in commandsArrays)
            {
                var command = commandString.Split(' ').ToArray();
                commands.Add(new Command(Convert.ToChar(command[0]), Convert.ToInt32(command[1])));
            }

            return commands;
        }

        [Theory]
        [InlineData(0, 0, "E 9,W 9", 10)]
        [InlineData(0, 0, "N 9,S 9", 10)]
        public void TestLocationsCountOnParalellOverlap(int startX, int startY, string commandString, int expected)
        {
            var commands = getCommandsFromString(commandString);

            _cleaningSession.RunCleaningSession(new int[] { startX, startY }, commands);

            Assert.Equal(expected, _robot.UniqueLocationsCount);
        }

        [Theory]
        [InlineData(0, 0, "E 3,N 3", 7)]
        [InlineData(0, 0, "W 3,S 3", 7)]
        public void TestLocationsCountOnOrtogonalOverlap(int startX, int startY, string commandString, int expected)
        {
            var commands = getCommandsFromString(commandString);

            _cleaningSession.RunCleaningSession(new int[] { startX, startY }, commands);

            Assert.Equal(expected, _robot.UniqueLocationsCount);
        }

        [Theory]
        [InlineData(0, 0, "N 4,E 2,S 3,E 2,N 2,W 4,S 3", 16)]
        [InlineData(0, 0, "E 49,W 49,S 50,N 50", 100)]
        public void TestLocationsCountOnRoute(int startX, int startY, string commandString, int expected)
        {
            var commands = getCommandsFromString(commandString);

            _cleaningSession.RunCleaningSession(new int[] { startX, startY }, commands);

            Assert.Equal(expected, _robot.UniqueLocationsCount);
        }

        [Fact]
        public void RunBigCleaningSession()
        {
            var commands = new List<Command>();

            for (int i = 0; i < 10000; ++i)
            {
                switch (i % 4)
                {
                    case 0:
                        commands.Add(new Command('E', 100000));
                        break;
                    case 2:
                        commands.Add(new Command('W', 100000));
                        break;
                    default:
                        commands.Add(new Command('N', 1));
                        break;
                }
            }

            _cleaningSession.RunCleaningSession(new int[] { 0, -5000 }, commands);

            var endPoint = new Tuple<int, int>(0, 0);

            Assert.Equal(500005001, _robot.UniqueLocationsCount);
            Assert.Equal(new Tuple<int, int>(_robot.CurrentLocation.X, _robot.CurrentLocation.Y), endPoint);
        }
    }
}