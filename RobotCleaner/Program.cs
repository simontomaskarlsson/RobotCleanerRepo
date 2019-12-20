using System;

namespace RobotCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandReader commandReader = new CommandReader();
            Robot robot = new Robot();
            CleaningSession cleaningSession = new CleaningSession(robot);

            var nrOfCommands = commandReader.ReadNrOfCommands();
            var startingCoordinates = commandReader.ReadStartingCoordinates();
            var commands = commandReader.ReadAllInstructions(nrOfCommands);

            cleaningSession.RunCleaningSession(startingCoordinates, commands);

            Console.WriteLine($"=> Cleaned: {robot.UniqueLocationsCount}");
            Console.WriteLine($"End location (remove) {robot.CurrentLocation.X}, {robot.CurrentLocation.Y}");
        }
    }
}
