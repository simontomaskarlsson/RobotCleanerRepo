namespace RobotCleaner
{
    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Location(int xCoordinate, int yCoordinate)
        {
            X = xCoordinate;
            Y = yCoordinate;
        }

    }
}