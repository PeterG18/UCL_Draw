using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using FootballTeam;
using Microsoft.VisualBasic;

namespace ScheduleManager;

class Scheduler
{

    public static (List<Club>, List<Club>) EligibleMatches(Club club, List<Club> allteams)
    {
        List<Club> possibleTeams = new List<Club>(allteams);
        // Remove matches where teams already met there country or pot limit

        //debug
        List<Club> removedTeams = new List<Club>();

        var (main_countries_played, main_pots_played) = CheckSchedule(club);

        // select countries that have been played at least twice and turn into a list
        var countriesWithTwoOrMore = main_countries_played.Where(countrykey => countrykey.Value >= 2)
        .Select(kv => kv.Key)
        .ToList();
        
        foreach (var team in allteams)
        {

            // if club has played a country twice make sure opponent isnt eligible to play this team
            if (countriesWithTwoOrMore.Contains(team.Country))
            {
                possibleTeams.Remove(team);
                removedTeams.Add(team);
            }
            
            // Team can't play against itself
            if (team.Name == club.Name)
            {
                possibleTeams.Remove(team);
                removedTeams.Add(team);
                continue;
            }

            // Don't play against team from same country
            if (team.Country == club.Country)
            {
                possibleTeams.Remove(team);
                removedTeams.Add(team);
                continue;
            }

            // Match sure match is also eligible on opponent side
            // Making sure the opponents schedule doesnt have a country played more than twice before matching with club from that country
            var (countries_played, pots_played) = CheckSchedule(team); // previous teams played info
            foreach (var country in countries_played)
            {
                if (country.Key == club.Country)
                {
                    if (country.Value >= 2)
                    {
                        possibleTeams.Remove(team);
                        removedTeams.Add(team);
                        continue;
                    }
                }
            }
            foreach (var teampot in pots_played)
            {
                if (teampot.Key == club.Pot)
                {
                    if (teampot.Value >= 2)
                    {
                        possibleTeams.Remove(team);
                        removedTeams.Add(team);
                    }
                }
            }
        }
        return (possibleTeams,removedTeams);
    }

    public static Dictionary<string, int> PotsRemaining(Club club)
    {
        Dictionary<string, int> potsremain = new Dictionary<string, int>
        {
            { "1", 2 },
            { "2", 2 },
            { "3", 2 },
            { "4", 2 }
        };

        var (countries_played, pots_played) = CheckSchedule(club);
        foreach (var pot in pots_played)
        {
            potsremain[pot.Key] -= pot.Value;
        }
        return potsremain;
    }

    public static bool CanFinishSchedule(List<Club> allteams, Club debugteam)
    {
        bool canFinish = true;
        foreach (Club team in allteams)
        {
            Dictionary<string, List<Club>> potTeamsAvailable = new Dictionary<string, List<Club>>();
            var (countries_played, pots_played) = CheckSchedule(team);
            var remainingPots = PotsRemaining(team);
            var (teamsAvailable, removedTeams) = EligibleMatches(team, allteams);

            // Get eligible teams for each pot
            foreach (var possMatch in teamsAvailable)
            {
                if (!potTeamsAvailable.ContainsKey(possMatch.Pot!))
                {
                    potTeamsAvailable[possMatch.Pot!] = new List<Club>();
                }
                potTeamsAvailable[possMatch.Pot!].Add(possMatch);
            }

            foreach (var potTeamNeeded in remainingPots)
            {
                var pot = potTeamNeeded.Key;
                var teamsNeeded = potTeamNeeded.Value;
                if (teamsNeeded == 0)
                {
                    // go to next pot because they already met requirements for this pot and next line would cause error
                    continue;
                }
                if (!potTeamsAvailable.ContainsKey(pot) || potTeamsAvailable[pot].Count < teamsNeeded)
                    {
                        canFinish = false;
                        return canFinish;
                    }
            }

        }

        return canFinish;
    }

    public static void AddMatch(Club club, Club match, int index)
    {
        // Add match to schedule for team and opponent. Mod 0 is home 1 away
            if (index % 2 == 0)
            {
                club.HomeGames.Add(match);
                club.Schedule[index] = match;

                //Add match to schedule for opponent as away game for them
                for (int i = 1; i < match.Schedule.Length; i += 2)
                {
                    if (match.Schedule[i] == null)
                    {
                        match.Schedule[i] = club;
                        match.AwayGames.Add(club);
                        break;
                    }
                }
            }
            else
            {
                club.AwayGames.Add(match);
                club.Schedule[index] = match;

                //Add match to schedule for opponent as away game for them
                for (int i = 0; i < match.Schedule.Length; i += 2)
                {
                    if (match.Schedule[i] == null)
                    {
                        match.Schedule[i] = club;
                        match.HomeGames.Add(club);
                        break;
                    }
                }
            }
    }

    public static void RemoveMatch(Club club, Club match, int index)
    {
        if (index % 2 == 0) // club was home
        {
            // Remove from club
            club.HomeGames.Remove(match);
            club.Schedule[index] = null;

            // Remove from opponent (they were away)
            for (int i = 1; i < match.Schedule.Length; i += 2)
            {
               if (match.Schedule[i] == club)
                {
                    match.Schedule[i] = null;
                    match.AwayGames.Remove(club);
                    break;
                }
            }
        }
        else // club was away
        {
            // Remove from club
            club.AwayGames.Remove(match);
            club.Schedule[index] = null;

            // Remove from opponent (they were home)
            for (int i = 0; i < match.Schedule.Length; i += 2)
            {
                if (match.Schedule[i] == club)
                {
                    match.Schedule[i] = null;
                    match.HomeGames.Remove(club);
                    break;
                }
            }
        }
    }

    
    private static Random rand = new Random();

    public static void MatchCreate(Club club, string pot, int index, List<Club> allteams)
    {
        // decide on random match
        // All clubs where Pot == input of pot number
        var (possibleMatches,removedTeams) = EligibleMatches(club, allteams);
        List<Club> potClubs = possibleMatches.Where(c => c.Pot == pot).ToList();
        foreach (Club? opponent in club.Schedule)
        {
            if (opponent != null  && potClubs.Contains(opponent))
            {
                potClubs.Remove(opponent);
            }
        }
        bool canFinish = false;
        while (canFinish == false)
        {
            int randomIndex = rand.Next(potClubs.Count);
            var match = potClubs[randomIndex];
            AddMatch(club, match, index);
            var canEntireScheduleFinsh = CanFinishSchedule(allteams,club);
            if (canEntireScheduleFinsh == true)
            {
                break;
            }
            else
            {
                RemoveMatch(club, match, index);
                potClubs.Remove(match);
                randomIndex = rand.Next(potClubs.Count);
                match = potClubs[randomIndex];
            }
        }

    }


    public static (Dictionary<string, int> CountriesPlayed, Dictionary<string, int> PotsPlayed) CheckSchedule(Club club)
    {
        var countries_played = new Dictionary<string, int>();
        var pots_played = new Dictionary<string, int>();

        foreach (Club? opponent in club.Schedule)
        {
            if (opponent != null)
            {
                if (countries_played.ContainsKey(opponent.Country!))
                {
                    countries_played[opponent.Country!] += 1;
                }
                else
                {
                    countries_played[opponent.Country!] = 1;
                }
                
                if (pots_played.ContainsKey(opponent.Pot!))
                {
                    pots_played[opponent.Pot!] += 1;
                }
                else
                {
                    pots_played[opponent.Pot!] = 1;
                }
            }
        }
        return (countries_played, pots_played);
    }


    // Rules are:
    // - Can't play team from same country
    // - Can't play more than 2 teams from same country
    // - Have to play 2 teams from each pot
    public static void ScheduleGenerate(Club club, List<Club> allteams)
    {    
        var c_allteams = new List<Club>(allteams);
        int index = 0;
        foreach (Club? opponent in club.Schedule)
        {
            // update countries played and pots played each time a match may have been made
            var (countries_played, pots_played) = CheckSchedule(club); // previous teams played info

            // Console.WriteLine($"{club.Name} team name <- {countries_played} country play {pots_played} pot play");
            if (opponent == null)
            {
                if (!pots_played.ContainsKey("1") || pots_played["1"] < 2)
                {
                    MatchCreate(club, "1", index, c_allteams);
                }

                else if (!pots_played.ContainsKey("2") || pots_played["2"] < 2)
                {
                    MatchCreate(club, "2", index, c_allteams);
                }

                else if (!pots_played.ContainsKey("3") || pots_played["3"] < 2)
                {
                    MatchCreate(club, "3", index, c_allteams);
                }

                else if (!pots_played.ContainsKey("4") || pots_played["4"] < 2)
                {
                    MatchCreate(club, "4", index, c_allteams);
                }
            }
            index++;
        }

        allteams.Remove(club);
    }
}
