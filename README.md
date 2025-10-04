# UCL_Draw Code

#### Access Code
Open commandline and CD into directory of choice, and run "git clone https://github.com/PeterG18/UCL_Draw.git"

You'll then be able to access all files
You can then open this from command line for windows by running "code ." and that will open vscode in the directory you're in and you'll then be able to access all the code

## Debug
####  Had errors where when a team follows the rules I end up running out of available team ensuring the rules are met, and I couldn't seem to get around this even with checking, if a match is scheduled could all teams still complete their schedule 

## Each Methods and File Use
#### Files
- Club.cs is just the design of the club object, we track the team name, country, schedule, etc
- UCLDraw.cs is what I use to test and print the scedules, you can test by running "dotnet run" in vscode terminal, you will get errors
- Scheduler.cs this is the heart of the code where the schedule generation is done

#### Methods for Scheduler Class
- EligibleMatches is a method which return a list of eligible matches a club has to play, and the matches removed which aren't eligble for debug purposes
- PotsRemaining reutrns a dictionary which pot 1 through 4 are the keys(string) and the value is the matches remaining in each pot for said club
- CanFinishSchedule is where I attempt to make sure if I select a match randomly for a team, each club in the UCL draw who have not gotten their schedule drawn can still finish their schedule while seeing through the requirements each team must follow
- AddMatch method just adds the match to the team being drawn schedule and the opponents schedule
- RemoveMatch method undoes this in case adding a match causes another team to be unable to finish schedule
- MatchCreate combines all of this into making sure match creation is legal
- CheckSchedule returns a dictionary of pots and countries a team has played
- ScheduleGenerate does the full auto generation of scheduling for a team

## UCL Rules
- Can't play yourself
- Can't play team from same country
- Can't play same team twice
- HAVE to play exactly 2 teams from each pot
- Can't play more than 2 teams from same country


## How to debug
You can use run and debug in VSCode and use a breakpoint to start where you want to begin debugging


