using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotCleaner
{
    public class CommandReader
    {
        public int ReadNrOfCommands()
        {
            return Convert.ToInt32(Console.ReadLine());
        }

        public int[] ReadStartingCoordinates()
        {
            return Console.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
        }

        public List<Command> ReadAllInstructions(int nrOfCommands)
        {
            var instructions = new List<Command>();

            for (int i = 0; i < nrOfCommands; ++i)
            {
                string[] instruction = Console.ReadLine().Split(' ');
                instructions.Add(new Command(Convert.ToChar(instruction[0]), Convert.ToInt32(instruction[1])));
            }

            return instructions;
        }
    }
}