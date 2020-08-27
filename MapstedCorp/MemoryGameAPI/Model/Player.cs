using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryGameAPI.Model
{
    public class Player
    {
        public string Difficulty { get; set; }

        public DateTime ElapsedTime { get; set; }

        public int MatchedCount { get; set; }

        public int ErrorScore { get; set; }
    }
}
