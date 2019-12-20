using System.Collections.Generic;

namespace RobotCleaner
{
    public class CleaningSession
    {
        private Robot _robot;
        public CleaningSession(Robot robot)
        {
            _robot = robot;
        }

        public void RunCleaningSession(int[] startingCoordinates, List<Command> commands)
        {
            _robot.SetRobotStartLocation(startingCoordinates[0], startingCoordinates[1]);
            foreach (var command in commands)
            {
                _robot.ExecuteCommand(command);
            }
        }
    }
}