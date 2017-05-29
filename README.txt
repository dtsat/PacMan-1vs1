Online Multiplayer Pacman
David Tsatsoulis

Standalone available in the Builds folder. Simply run and enjoy!

This game is an online multiplayer version of the classic game Pacman.

CONTROLS
- Enter your name and click the "Connect and Play" button to begin your game.
- Use arrow keys to move up/down/left/right
- Exit to main screen using the "Leave Game" button

Game functions
- This game supports 1 or 2 players. 
- If only 1 player joins, then the game will proceed with 1 player until a 2nd player joins.
- Once the 2nd player joins, the game starts over again with 2 players.
- Regardless of the number of players, the goal remains the same: eat all the pac dots located in the arena.
- Eating a larger, super pac dot increases your speed temporarily
- Ghosts patrol the arena. If you are hit by a ghost, you return to your starting position. There is no other penalty for being hit by a ghost.
- Ghosts cannot be destroyed.
- The arena is a 2.5D maze modeled after the original Pac Man game. There are 2 exit tunnels that wrap around to each other.
- The winner is the player that eats the most pac dots by the time they are all gone. 

Implementation

Multiplayer - 
- Multiplayer support is implemented using the Photon Unity Network tool.
- There are 2 scenes, depending on whether it is 1 or 2 players. When a player joins or leaves the game, the appropriate scene is loaded (using the number of players as a check)
- The ghosts and pac dots are instantiated on the Master Client and communicated to the connected client.
- There is one single script used for both pacman characters, and movement is distinguished using the photonView.isMine attribute (as well as client-side sounds)
- Calls to destroy a pac dot and add to the score are done via RPC to the MasterClient. This ensures that all pac dots and scores are synced up correctly on both sides.
- Only the MasterClient controls the pac dot locations and scoreboard.
- Pacman characters and ghosts communicate their position to all parties using the Photon View component, and use Lerp to interpolate their movement so it appears smooth.

Arena/movement
- The arena/maze is built using a tile based graph and a node/edge system similar to Assignment 2
- Movement of pacman characters works by keeping tack of the "current tile" the pacman character is on. When a direction key is pressed, it checks to see if the current tile allows for such movement.
- If movement is allowed, the character is moved to the next tile, and the current tile is updated. Movement works via linear interpolation, with a constant amount of time needed to move from tile a to tile b.
- This ensures that pacman is only allowed to move where he is supposed to, and never comes into contact with any walls.

Enemies
- Ghost enemies move via navmesh overlayed onto the maze. Ghosts cannot touch the walls as the navmesh is structured so that they are restricted to the same movement as pacman.
- Each ghost has a different movement type. They will always react by chasing the pacman character closest to them.
- The Pink Ghost moves slowly and methodically, going to straight to the nearest pacman.
- The Orange Ghost is "stupid" and moves slowly, often out of position. However, if you get too close, it will accellerate quickly and easily reach you. The ghost will slam on the breaks when it is close to you.
- The Red Ghost moves similarly to the pink ghost, but will never reach you. Instead, he chooses to stop when he is near you in an attempt to trap you in a corner so that his friends can reach you from the other side.
- The Blue Ghost is the most dangerous, he will move very quickly towards your location and will easily catch you in any straight-away corridor. To avoid him, you must out-maneuver him as he turns slowly and has trouble slowing down.


