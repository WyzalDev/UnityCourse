# Second Part of [FuryLion Unity Course](https://furylion.net/courses/junior-unity-developer)
This part of course covered Unity Editor through 3 project - SkyRoads, IntoTheSpace, Match3.  
Briefly about these projects:  
# SkyRoads
A 3D runner game. You control a spaceship and navigate an infinitely generated path while dodging meteorites. The goal is to score as many points as possible.  

Tasks covered:
* Implementation of gameplay mechanics (ship movement, ship acceleration, meteorites spawn, high-score table, pause and end of game).
* Adaptive UI (main menu, pause, settings, records, loss, and victory windows, exit warning popup) and HUD
* Creating a singleton systems for windows and popups management
* Development of a system for storing/set up/playing the volume of sounds and music.
* Creating VFX for a game using Particle System.
* Creating ship tilt and acceleration animations.
* Optimizing textures for responsive UI using 9-slice.
* Bug fixes in the Unity editor, C# code, and assets.

# IntoTheSpace
A 2D game - a Space Invaders clone. Control a spaceship and shoot at alien ships. The goal is to score as many points as possible.  

Tasks covered:
* Development of gameplay mechanics (ship/enemy movement, ship shooting and enemy hitting, enemy spawning, ultimate ability, ship selection, records, pause and end of game).
* Implementations of tasks like in SkyRoads (UI, HUD, Music and Sounds, VFX, functional menus, bugfixes).
* Adaptive UI - ship choosing window.
* VFX - postprocessing effect for taking damage.
* Optimizing Projectile in game using ObjectPool.

# Match3
A 2D Match3 game with a drawing mechanic. By creating combinations by swiping your finger across tokens to create chains, you must complete one of the level's objectives (score a required number of points, match/break all gems/obstacles of a certain type).  

Tasks covered:
* Development of gameplay mechanics (creating/matching gem chains, activating board bonuses, level goals, moves limit, level selection, pause, and game overs).
* Implementations of tasks like in IntoTheSpace (UI, HUD, Music and Sounds, VFX, functional menus, bugfixes).
* Creating a Custom Level Editor using EditorWindow.
* Optimizing gem drop animations in the game using ObjectPool.
* Creation of a gem-chains management system.
* Managing the gameplay loop using the Finite State Machine pattern.

