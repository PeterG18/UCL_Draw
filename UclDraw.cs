using System;
using System.Collections.Generic;
using FootballTeam;
using ScheduleManager;

namespace UclDraw
{
    class UclDraw
    {
        // Pot 1
        static Club PSG = new Club { Name = "Paris Saint-Germain", Country = "France", Pot = "1" };
        static Club RealMadrid = new Club { Name = "Real Madrid", Country = "Spain", Pot = "1" };
        static Club ManCity = new Club { Name = "Manchester City", Country = "England", Pot = "1" };
        static Club Bayern = new Club { Name = "Bayern München", Country = "Germany", Pot = "1" };
        static Club Liverpool = new Club { Name = "Liverpool", Country = "England", Pot = "1" };
        static Club Inter = new Club { Name = "Inter", Country = "Italy", Pot = "1" };
        static Club Chelsea = new Club { Name = "Chelsea", Country = "England", Pot = "1" };
        static Club Dortmund = new Club { Name = "Borussia Dortmund", Country = "Germany", Pot = "1" };
        static Club Barcelona = new Club { Name = "Barcelona", Country = "Spain", Pot = "1" };

        // Pot 2
        static Club Arsenal = new Club { Name = "Arsenal", Country = "England", Pot = "2" };
        static Club Leverkusen = new Club { Name = "Bayer Leverkusen", Country = "Germany", Pot = "2" };
        static Club Atletico = new Club { Name = "Atlético Madrid", Country = "Spain", Pot = "2" };
        static Club Benfica = new Club { Name = "Benfica", Country = "Portugal", Pot = "2" };
        static Club Atalanta = new Club { Name = "Atalanta", Country = "Italy", Pot = "2" };
        static Club Villarreal = new Club { Name = "Villarreal", Country = "Spain", Pot = "2" };
        static Club Juventus = new Club { Name = "Juventus", Country = "Italy", Pot = "2" };
        static Club Frankfurt = new Club { Name = "Eintracht Frankfurt", Country = "Germany", Pot = "2" };
        static Club Brugge = new Club { Name = "Club Brugge", Country = "Belgium", Pot = "2" };

        // Pot 3
        static Club Tottenham = new Club { Name = "Tottenham", Country = "England", Pot = "3" };
        static Club PSV = new Club { Name = "PSV Eindhoven", Country = "Netherlands", Pot = "3" };
        static Club Ajax = new Club { Name = "Ajax", Country = "Netherlands", Pot = "3" };
        static Club Napoli = new Club { Name = "Napoli", Country = "Italy", Pot = "3" };
        static Club Sporting = new Club { Name = "Sporting CP", Country = "Portugal", Pot = "3" };
        static Club Olympiacos = new Club { Name = "Olympiacos", Country = "Greece", Pot = "3" };
        static Club Slavia = new Club { Name = "Slavia Praha", Country = "Czechia", Pot = "3" };
        static Club BodoGlimt = new Club { Name = "Bodø/Glimt", Country = "Norway", Pot = "3" };
        static Club Marseille = new Club { Name = "Marseille", Country = "France", Pot = "3" };

        // Pot 4
        static Club Copenhagen = new Club { Name = "Copenhagen", Country = "Denmark", Pot = "4" };
        static Club Monaco = new Club { Name = "Monaco", Country = "France", Pot = "4" };
        static Club Galatasaray = new Club { Name = "Galatasaray", Country = "Turkey", Pot = "4" };
        static Club UnionSG = new Club { Name = "Union SG", Country = "Belgium", Pot = "4" };
        static Club Qarabag = new Club { Name = "Qarabağ", Country = "Azerbaijan", Pot = "4" };
        static Club Athletic = new Club { Name = "Athletic Club", Country = "Spain", Pot = "4" };
        static Club Newcastle = new Club { Name = "Newcastle United", Country = "England", Pot = "4" };
        static Club Pafos = new Club { Name = "Pafos", Country = "Cyprus", Pot = "4" };
        static Club Kairat = new Club { Name = "Kairat", Country = "Kazakhstan", Pot = "4" };

        static void Main(string[] args)
        {
            Draw();
        }
        public static void Draw()
        {
            List<Club> allClubs = new List<Club>
            {
                PSG, RealMadrid, ManCity, Bayern, Liverpool, Inter, Chelsea, Dortmund, Barcelona,
                Arsenal, Leverkusen, Atletico, Benfica, Atalanta, Villarreal, Juventus, Frankfurt, Brugge,
                Tottenham, PSV, Ajax, Napoli, Sporting, Olympiacos, Slavia, BodoGlimt, Marseille,
                Copenhagen, Monaco, Galatasaray, UnionSG, Qarabag, Athletic, Newcastle, Pafos, Kairat
            };

            List<Club> c_allClubs = new List<Club>(allClubs);
            List<Club> pot1Clubs = allClubs.Where(c => c.Pot == "1").ToList();

            foreach (var team in c_allClubs)
            {
                Scheduler.ScheduleGenerate(team, allClubs);

                // Example: print PSG's schedule after generation
                Console.WriteLine($"{team.Name} Schedule:");
                foreach (var match in team.Schedule)
                {
                    if (match != null)
                        Console.WriteLine($"{team.Name} vs {match.Name}");
                }
                Console.WriteLine($"-----------------------------------------------------------------");
            }
        }
    }
}
