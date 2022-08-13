# Army Clash Core
____
## About The Project
At the core of the engine I used structured code that can be extended by other programmers without big problems. The main work was carried out with the physics setup 
for the army. 

The design pattern **Object Pool** was chosen to improve performance, also I minimized use of Update functions. At the current point, the profiler 
showed good results. My main design patterns for working with code and Unity environment are **MVC** and **ECS**. 

During the integration of unit specifications I ran into a problem that the random 
(**SPHERE**, **SMALL**, **GREEN**), the **HEALTH** value became zero. The decision was to change the **GREEN** **HEALTH** reduction from **-100** to **-50**, so as not to limit the units in 
the random. 

I used an **Abstract Class** to simplify the further expansion of the types of units. It contains the main methods and variables of the current and future units.

As an **Additional Feature**, I added a few things: displaying the number of live units as player health, displaying damage to units, changing the color of enemy units.
It seemed necessary to me for a better understanding of the game.
