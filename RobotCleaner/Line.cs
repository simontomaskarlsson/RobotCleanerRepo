using System;
using System.Linq;

namespace RobotCleaner
{
    public class Line
    {
        public Location StartingLocation { get; private set; }
        public Command Command { get; private set; }

        public Line(Location startingLocation, Command command)
        {
            StartingLocation = startingLocation;
            Command = command;
        }

        public Tuple<int, int>[] GetAllLineLocations()
        {
            var locations = new Tuple<int, int>[Command.NrOfSteps + 1];

            switch (Command.Direction)
            {
                case 'E':
                    for (var i = 0; i < locations.Length; i++)
                    {
                        locations[i] = new Tuple<int, int>(StartingLocation.X + i, StartingLocation.Y);
                    }
                    return locations;

                case 'W':
                    for (var i = 0; i < locations.Length; i++)
                    {
                        locations[i] = new Tuple<int, int>(StartingLocation.X - i, StartingLocation.Y);
                    }
                    return locations;

                case 'N':
                    for (var i = 0; i < locations.Length; i++)
                    {
                        locations[i] = new Tuple<int, int>(StartingLocation.X, StartingLocation.Y + i);
                    }
                    return locations;

                case 'S':
                    for (var i = 0; i < locations.Length; i++)
                    {
                        locations[i] = new Tuple<int, int>(StartingLocation.X, StartingLocation.Y - i);
                    }
                    return locations;

                default:
                    return new Tuple<int, int>[] { };
            }
        }

        public Location GetEndLocation()
        {
            switch (Command.Direction)
            {
                case 'E':
                    return new Location(StartingLocation.X + Command.NrOfSteps, StartingLocation.Y);
                case 'W':
                    return new Location(StartingLocation.X - Command.NrOfSteps, StartingLocation.Y);
                case 'N':
                    return new Location(StartingLocation.X, StartingLocation.Y + Command.NrOfSteps);
                case 'S':
                    return new Location(StartingLocation.X, StartingLocation.Y - Command.NrOfSteps);

                default:
                    return new Location(0, 0);
            }
        }

        public bool CheckIfParallell(Line l)
        {
            return ((Command.Direction == 'E' || Command.Direction == 'W') &&
                    (l.Command.Direction == 'E' || l.Command.Direction == 'W'))
                    || ((Command.Direction == 'N' || Command.Direction == 'S') &&
                    (l.Command.Direction == 'N' || l.Command.Direction == 'S'));
        }

        public Location[] GetOrtogonalOverlap(Line l)
        {
            var horizontalLine = Command.Direction == 'E' || Command.Direction == 'W' ? this : l;
            var verticalLine = Command.Direction == 'N' || Command.Direction == 'S' ? this : l;
            if ((horizontalLine.StartingLocation.X, horizontalLine.StartingLocation.Y) == (verticalLine.GetEndLocation().X, verticalLine.GetEndLocation().Y))
            {
                return new Location[] { new Location(horizontalLine.StartingLocation.X, horizontalLine.StartingLocation.Y) };
            }
            else if ((verticalLine.StartingLocation.X, verticalLine.StartingLocation.Y) == (horizontalLine.GetEndLocation().X, horizontalLine.GetEndLocation().Y))
            {
                return new Location[] { new Location(verticalLine.StartingLocation.X, verticalLine.StartingLocation.Y) };
            }
            else if ((verticalLine.StartingLocation.X < Math.Min(horizontalLine.StartingLocation.X, horizontalLine.GetEndLocation().X))
            || (verticalLine.StartingLocation.X > Math.Max(horizontalLine.StartingLocation.X, horizontalLine.GetEndLocation().X)))
            {
                return new Location[] { };
            }
            else if ((horizontalLine.StartingLocation.Y < Math.Min(verticalLine.StartingLocation.Y, verticalLine.GetEndLocation().Y))
            || (horizontalLine.StartingLocation.Y > Math.Max(verticalLine.StartingLocation.Y, verticalLine.GetEndLocation().Y)))
            {
                return new Location[] { };
            }
            return GetOverlaps(l);
        }

        public Location[] GetParallellOverlaps(Line l)
        {
            if (Command.Direction == 'E' || Command.Direction == 'W')
            {
                if (StartingLocation.Y == l.StartingLocation.Y)
                {
                    return GetHorizontalParallellOverlaps(l);
                }
                return new Location[] { };
            }
            if (StartingLocation.X == l.StartingLocation.X)
            {
                return GetVerticalParallellOverlaps(l);
            }

            return new Location[] { };
        }

        private Location[] GetHorizontalParallellOverlaps(Line l)
        {
            if ((Math.Max(StartingLocation.X, GetEndLocation().X) < Math.Min(l.StartingLocation.X, l.GetEndLocation().X))
                || (Math.Min(StartingLocation.X, GetEndLocation().X) > Math.Max(l.StartingLocation.X, l.GetEndLocation().X)))
            {
                return new Location[] { };
            }

            return GetOverlaps(l);
        }

        private Location[] GetVerticalParallellOverlaps(Line l)
        {
            if ((Math.Max(StartingLocation.Y, GetEndLocation().Y) < Math.Min(l.StartingLocation.Y, l.GetEndLocation().Y))
                || (Math.Min(StartingLocation.Y, GetEndLocation().Y) > Math.Max(l.StartingLocation.Y, l.GetEndLocation().Y)))
            {
                return new Location[] { };
            }

            return GetOverlaps(l);
        }

        public Location[] GetOverlaps(Line l2)
        {
            var lineLocations1 = this.GetAllLineLocations().ToList();
            var lineLocations2 = l2.GetAllLineLocations().ToList();

            var overlaps = lineLocations1.Intersect(lineLocations2);

            return overlaps.Select(ol => new Location(ol.Item1, ol.Item2)).ToArray();
        }
    }
}