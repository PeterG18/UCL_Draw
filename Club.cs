using System;

namespace FootballTeam
{
    class Club
    {
        //Properties of the club
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Pot { get; set; }

        //Club Scheduling
        //Schedule is initially 8 empty slots, and will be adjusted as scheduled
        public Club?[] Schedule { get; set; } = new Club?[8];

        // Home and away games will be added at the end each time
        public List<Club> HomeGames = new List<Club>();
        public List<Club> AwayGames = new List<Club>();
    }
    
}