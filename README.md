
[![Discord](https://discord.com/assets/e4923594e694a21542a489471ecffa50.svg)](https://discord.gg/bYSaT74KzT)

# ![Logo](./Images/logo.png)

![Everyone](./Images/Everything.png)
An Among Us mod that adds a lot of mods and relevant game settings



There are 10 roles
- [Mayor](#mayor)
- [Jester](#jester)
- [Sheriff](#sheriff)
- [Lovers](#lovers)
- [Mafia](#mafia)
  - Godfather
  - Janitor
  - Mafioso
- [Engineer](#engineer)
- [Swapper](#swapper)
- [Shifter](#shifter)
- [Investigator](#investigator)
- [Time Master](#time-master)

Roles might be added in the future

# Releases :
| Among Us - Version| Mod Version | Link |
|----------|-------------|-----------------|
| 2020.12.19s | v1.0.3 | [Download](https://github.com/slushiegoose/Town-Of-Us/releases/download/v1.0.3/TownOfUs-v1.0.3.zip) |
| 2020.12.19s | v1.0.2 | [Download](https://github.com/slushiegoose/Town-Of-Us/releases/download/v1.0.2/TownOfUs-v1.0.2.zip) |
| 2020.12.19s | v1.0.1 | [Download](https://github.com/slushiegoose/Town-Of-Us/releases/download/v1.0.1/TownOfUs-v1.0.1.zip) |
| 2020.12.19s | v1.0.0 | [Download](https://github.com/slushiegoose/Town-Of-Us/releases/download/v1.0.0/TownOfUs-v1.0.0.zip) |

<details>
  <summary> Changelog </summary>
  <h3> <v> v1.0.3 </b> </h3>
  <p> Fixes: </p>
  <ul>
    <li> Engineer being able to Fix every sabotage </li>
    <li> The Investigator being able to revive people voted out (and instead made it an option) </li>
    <li> Footprints not appearing if a person walks over old footprints </li>
    <li> Graphical glitch where Jester and Lovers simultaneously win </li>
    <li> Players spawning back into positions from the last game when time is rewound right at the beginning of a new game </li>
    <li> Graphical Bug of Time Master's name not being blue in Meetings </li>
    <li> Fixed Medbay Scan not working for custom colours </li>
  </ul>
  <hr>
  <h3> <b> v1.0.2 </b> </h3>
  <p> Fixes: </p>
  <ul>
    <li>  The Swapper being unable to vote </li>
    Swapper unable to vote
    <li> Mayor Vote Bank ending up negative </li>
    <li> Anonymous Voting not working when Mayor is in a game </li>
    <li> Chats not working in the lobby </li>
    <li> Sheriff desync (I think) </li>
    <li> Buttons being able to be used during meetings </li>
  </ul>
  <hr>
  <h3> <b> v1.0.1 </b> </h3>
  <p> Fixes errors of not being able to launch. </p>
</details>


# Installation
**Download the zip file on the right side of Github.**  
1. Find the folder of your game, for steams players you can right click in steam, on the game, a menu will appear proposing you to go to the folders.
2. Make a copy of your game (not required but recommended)
3. Drag or extract the files from the zip into your game, at the .exe level.
4. Turn on the game.
5. Play the game.

![Install](https://i.imgur.com/pvBAyZN.png)

# Roles
## Mayor
### **Team: Crewmates**
The Mayor is a role that can vote multiple times.\
The Mayor has a Vote Bank, which is the number of times they can vote.\
Every time they don't vote in a meeting, one vote is added to the Vote Bank.\
During a meeting, so long as not everyone has voted, the Mayor can use a vote or multiple votes from their Vote Bank to vote multiple times.\
However, to compensate for this, the Mayor cannot fix Lights or Comms.

### Game Options
| Name | Description | Type | Default |
|----------|:-------------:|:------:|:------:|
| Mayor | The percentage probability of the Mayor appearing | Number | 0% |
| Mayor Vote Bank | The number of votes in the Vote Bank to start with. | Number | 1 |
| Mayor Vision | The vision level of the Mayor | Number | 1x |
-----------------------
## Jester
### **Team: Neutral**
The Jester has zero tasks and their aim is to be voted out.\
If they are voted out then the game finishes and they win.\
However, the Jester does not win if the Crewmates or Impostor wins.

### Game Options
| Name | Description | Type | Default |
|----------|:-------------:|:------:|:------:|
| Jester | The percentage probability of the Jester appearing | Number | 0% |
-----------------------
## Sheriff
### **Team: Crewmates**
The Sheriff is able to kill and their aim is to kill Impostors.\
However, if they attempt to kill a Crewmate or a Neutral character, they instead die themselves.

### Game Options
| Name | Description | Type | Default |
|----------|:-------------:|:------:|:------:|
| Sheriff | The percentage probability of the Sheriff appearing | Number | 0% |
| Show Sheriff | Whether everybody can see who the Sheriff is | Toggle | False |
| Sheriff Kill Cooldown | The cooldown of the Sheriff's kill | Number | 25s
-----------------------
## Lovers
### **Team: Either Crewmates or Impostors**
The Lovers are two players who are linked together.\
They gain the primary objective to stay alive together.\
If  they are both among the last 3 players, they win.\
However, they can also win with their respective team and hence the Lovers do not know the role of the other lover.\
There is a 66.7% chance that both Lovers are Crewmates.\
There is a 33.3% chance that one Lover is a Crewmate and one is an Impostor.

### Game Options
| Name | Description | Type | Default |
|----------|:-------------:|:------:|:------:|
| Lovers | The percentage probability of the Lovers appearing | Number | 0% |
| Both Lovers Die | Whether the other lover automatically suicides if the other dies | Toggle | True |
-----------------------
## Mafia
### **Team: Impostors**
The Mafia are a group of three Impostors.\
The Godfather is a normal impostor and can sabotage and kill.\
The Janitor is an impostor who cannot kill but can instead clean up bodies so that they can't be seen nor reported.\
The Mafioso is an impostor who cannot sabotage nor kill until the Godfather is dead.\
**NOTE** - The Mafia only appears in games of 3+ impostors, even when set to a 100% chance.

### Game Options
| Name | Description | Type | Default |
|----------|:-------------:|:------:|:------:|
| Mafia | The percentage probability of the Mafia appearing | Number | 0% |
| Janitor Clean Cooldown | The cooldown of the Janitor's cleaning | Number | 25s |
-----------------------
## Engineer
### **Team: Crewmates**
The Engineer can fix one sabotage per round from anywhere on the map.\
However, once fixed, the impostors receive an arrow pointing to the Engineer, giving up their spot.\
If an Engineer has used a fix for that round, the button cannot be pressed by *anybody* and meetings can only happen from reporting bodies.

### Game Options
| Name | Description | Type | Default |
|----------|:-------------:|:------:|:------:|
| Engineer | The percentage probability of the Engineer appearing | Number | 0% |
| Engineer Vision | The vision level of the Engineer | Number | 1x |
-----------------------
## Shifter
### **Team: Neutral**
The Shifter is a roleless character.\
They have zero tasks and zero win conditions.\
However they can swap roles with people.\
Swapping roles with a custom role gives the Shifter the role and their tasks and turns the other player into a Shifter.\
Swapping roles with a Crewmate gives the Shifter their tasks and turns the Crewmate into a Shifter.\
Swapping roles with an Impostor fails and kills the Shifter.


### Game Options
| Name | Description | Type | Default |
|----------|:-------------:|:------:|:------:|
| Shifter | The percentage probability of the Shifter appearing | Number | 0% |
| Shifter Cooldown | The cooldown of the Shifter's Shifting | Number | 25s |
-----------------------
## Swapper
### **Team: Crewmates**
The Swapper can swap two people's votes during a Meeting.\
Everyone who voted for the first person will instead be counted for the second and vice versa.\
This could easily change the outcome of a vote and perhaps vote off a different person.\
To counteract this, The Swapper cannot fix Lights or Comms and cannot push the Emergency Button.

### Game Options
| Name | Description | Type | Default |
|----------|:-------------:|:------:|:------:|
| Swapper | The percentage probability of the Swapper appearing | Number | 0% |
-----------------------
## Investigator
### **Team: Crewmates**
The Investigator can see the movement of players.\
Every player leaves a footprint that only the Investigator can see, which disappears after some time.\

### Game Options
| Name | Description | Type | Default |
|----------|:-------------:|:------:|:------:|
| Investigator | The percentage probability of the Investigator appearing | Number | 0% |
| Footprint Size | The size of the footprint on a scale of 1 to 10 | Number | 4 |
| Footprint Interval | The time between two footprints | Number | 1s |
| Footprint Duration | The amount of time that the footprint stays on the ground | Number | 10s |
| Anonymous Footprint | When enabled, all footprints are grey rather than the player color | Toogle | False
| Footprint Vent Visible | Whether footprints near vents are shown | Toggle | False
-----------------------
## Time Master
### **Team: Crewmates**
The Time Master can rewind time and all plaers will reverse.\
If enabled, any players killed during this time will be revived.\
Nothing but movements and kills are affected (for now).

### Game Options
| Name | Description | Type | Default |
|----------|:-------------:|:------:|:------:|
| Time Master | The percentage probability of the Time Master appearing | Number | 0% |
| Revive During Rewind | Whether to revive dead players when rewinding | Toggle | False |
| Rewind Duration | How far to rewind back time | Number | 3s |
| Rewind Cooldown | The cooldown of the Time Master's Rewind | Number | 25s |
| Revive Voted Out | Whether people voted out are revived if time <br> is rewound just after a meeting finishes | Toggle | False |
-----------------------
## Extras
### New Colors!
New colors are added for crewmates so that they can be more colors
### Rainbow Color!
A rainbow color has also been added. Anyone who equips this color will constantly switch between the colors of the rainbow.




# Bug / Suggestions
If you have any bugs or any need to contact me, join the [Discord server](https://discord.gg/bYSaT74KzT) or create a ticket on GitHub

# Credits & Resources
[Reactor](https://github.com/NuclearPowered/Reactor) - The framework of the mod\
[BepInEx](https://github.com/BepInEx) - For hooking game functions\
[Essentials](https://github.com/DorCoMaNdO/Reactor-Essentials) - For created custom game options.\
[Among-Us-Sheriff-Mod](https://github.com/Woodi-dev/Among-Us-Sheriff-Mod) - For the Sheriff role.\
[Among-Us-Love-Couple-Mod](https://github.com/Woodi-dev/Among-Us-Love-Couple-Mod) - For the inspiration of Lovers role.\
[ExtraRolesAmongUs](https://github.com/NotHunter101/ExtraRolesAmongUs) - For the Engineer role.\
[TooManyRolesMods](https://github.com/Hardel-DW/TooManyRolesMods) - For the Investigator & Time Master roles.\
[XtraCube](https://github.com/XtraCube) for the RainbowMod.

# License
This software is distributed under the GNU GPLv3 License. BepInEx is distributed under LGPL-2.1 License.
