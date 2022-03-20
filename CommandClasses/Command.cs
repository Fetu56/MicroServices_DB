using System.Collections.Generic;

namespace WebApplication2
{
    public class Command
    {
        public List<Player> Players { get; set; }
        public string Name { get; set; }
        public Command()
        {
            Players = new List<Player>();
            Name = "";
        }
        public Command(string name)
        {
            Players = new List<Player>();
            Name = name;
        }
        public Command(List<Player> players, string name)
        {
            Players = players;
            Name = name;
        }
    }
}
