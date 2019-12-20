using System.Collections.Generic;
using System.Linq;

namespace RobotCleaner
{
    public class Robot
    {
        private readonly List<Line> _previousLines;
        public int UniqueLocationsCount { get; private set; }
        public Location CurrentLocation { get; private set; }

        public Robot()
        {
            _previousLines = new List<Line>();
        }

        public void SetRobotStartLocation(int coordinateX, int coordinateY)
        {
            CurrentLocation = new Location(coordinateX, coordinateY);
        }

        private void MoveRobot(Command command)
        {
            switch (command.Direction)
            {
                case 'E':
                    CurrentLocation.X = CurrentLocation.X + command.NrOfSteps;
                    break;
                case 'W':
                    CurrentLocation.X = CurrentLocation.X - command.NrOfSteps;
                    break;
                case 'N':
                    CurrentLocation.Y = CurrentLocation.Y + command.NrOfSteps;
                    break;
                case 'S':
                    CurrentLocation.Y = CurrentLocation.Y - command.NrOfSteps;
                    break;
            }
        }

        public void ExecuteCommand(Command command)
        {
            var newLine = new Line(new Location(CurrentLocation.X, CurrentLocation.Y), new Command(command.Direction, command.NrOfSteps));
            var overlappingLocations = new List<Location>();

            foreach (var line in _previousLines)
            {
                if (line.CheckIfParallell(newLine))
                {
                    var overlaps = line.GetParallellOverlaps(newLine);
                    overlappingLocations.AddRange(overlaps);
                }
                else
                {
                    overlappingLocations.AddRange(line.GetOrtogonalOverlap(newLine));
                }
            }

            _previousLines.Add(newLine);
            MoveRobot(command);
            UniqueLocationsCount += command.NrOfSteps + 1 - overlappingLocations.Select(ol => (ol.X, ol.Y)).Distinct().ToList().Count;
        }
    }
}