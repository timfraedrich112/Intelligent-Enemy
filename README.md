# Intelligent-Enemy

This game demonstrates a prototype agent that uses artificial intelligence to collect data on how the player fights the enemy, and changes its response accordingly

- The agent uses a complex behavior tree. Vision sensors watch the player and keep track of different statistics such as:
  - The players distance from the agent.
  - How often the players uses ranged attacks vs. melee attacks.
  - How often each of the agents attacks successfully land or miss.
  - Whether or not the enemies attacks miss due to being shielded or dodged.
- The agent has 5 distinct and unique attacks. Where to move and aim, as well as which attack to use is decided by the behavior tree.
- As a huge fan of Super Smash Bros, this project was inspired by how two real players would fight one another by learning habits and playstyles of their opponent in order to counter them.

### Demo Video: https://www.youtube.com/watch?v=2Ho-tiwPyu4&

#### Assets used from Unity Assets Store:

Behavior Bricks: https://assetstore.unity.com/packages/tools/visual-scripting/behavior-bricks-74816
