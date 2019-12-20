namespace RobotCleaner
{
    public class Command
    {
        public char Direction { get; private set; }
        public int NrOfSteps { get; private set; }
        public Command(char direction, int nrOfSteps)
        {
            Direction = direction;
            NrOfSteps = nrOfSteps;
        }
    }
}