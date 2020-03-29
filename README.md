**************************************************MS.PACMAN**************************************************


Pac-Man is a video game created by Tōru Iwatani for the Japanese company Namco. Originally produced for Arcade in the early 1980s, it became one of the most played and popular games at the moment, having modern versions for various devices. The game's mechanics are simple: the player is a round head with a mouth that opens and closes, positioned in a simple maze filled with dots and 4 ghosts that chase him. The goal was to eat all the dots without being reached by the ghosts, in a progressive rhythm of difficulty.

Our game has a similar premise, but the main character is Ms.Pac-Man instead of the classic Pac-Man.

CONTROLS:

W - up
A - left
S - down
D - right

---------MOST IMPORTANT CLASSES----------

-> MS.PAC-MAN
The main character of the game. The "player".

The Player class is responsible for the designation of player controls, checking intersections (with walls, dots, power pellets, ghosts, etc.), assigning scores, modifying the highscore if the current score exceeds it, removing lives from the player, and test and present the "YOU WIN" and "GAME OVER" screens.


	->>>PLAYER MOVEMENT
	The player movement code is fairly simple. The .txt file that constitutes our map is made out of several symbols and the player is only allowed to move to the player's target 			position if that position is occupied by a symbol which does not represent an obstacle. 

	->>>COLLISION
	If the player collides with an enemy, there is a function that tests whether or not the player 	is under the power pellet's effect. If the player is under its effect, the ghost 		dies; If they are not, they die. If the player collides with the dots, the dots will disappear and they will gain the score. If he collides with the power pellets, the pellet will 
	disappear, the player will get the score and gain the ability to "eat" the ghosts.

	->>>SCORE & HIGHSCORE
	The player gets 10 points for each dot he eats and 50 points for each power pellet he eats. If the current score surpasses the last saved highscore, the displayed highscore will 		be the current score. The current highscore only saves if the player finishes the game (by winning or losing). 



-> GHOSTS

The ghost class is responsible for controlling the ghosts' different movement patterns, checking the ghost's intersections and assigning scores for when ghosts get eaten

Blinky: is a red ghost who, in the original arcade game, follows behind Pac-Man. He is considered the leader of the ghosts.

Pinky: she is a pink female ghost who, in the original arcade game, uses ambush tactics to position herself in front of Pac-Man to surround him.

Inky:is a cyan ghost who, in the original arcade game, has a fickle mood. He can be unpredictable. Sometimes he chases Pac-Man aggressively like Blinky; other times he jumps ahead of Pac-Man as Pinky would. He might even wander off like Clyde on occasion.

Clyde: is an orange ghost who, in the original arcade game, acts stupid. He will chase after Pac-Man in Blinky's manner, but will wander off to his home corner when he gets too close.

	
	->>>GHOST MOVEMENT
	The movement of the ghosts works similarly to the player movement but it is not controlled by the player. Each ghost has a different movement pattern based on the descriptions 		above. 

	->>>GHOST SCORE
	Each ghost the player eats is worth twice as much as the one he previously ate. For instance, the first ghost eaten is always worth 200pts, the second ghost will be worth 400pts, 		the third one 800pts, and so forth.




----------COLLECTIBLES----------

-> DOTS (10PTS)
Scattered across the map, the player must eat all the dots to win.

-> PELLETS (50PTS)
Allows the player to eat the ghosts after consuming it.

-> CHERRY (100PTS)
Appears on the map when the player has reached a score of 1000.

-> STRAWBERRY (300PTS)
Appears on the map when the player has reached a score of 2000.

-> LIVES (+1 life)
Appears on the map when the player has reached a score of 10000.


----------SOUNDTRACK----------


We used several different soundtracks from the original game, which are used at times like:

-> when you start the game
-> when Ms.Pac-Man's mouth opens and closes
-> when the player dies
-> when the player eats fruit
-> when the player eats a ghost


----------SUCCESSFULLY IMPLEMENTED----------

-> tile based implementation
-> Game Over concept
-> possible to win
-> use of animations
-> use of sound


----------THINGS THAT AREN'T WORKING QUITE RIGHT :(----------

-> enemy autonomous movement
-> more than one level


