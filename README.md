# Blue Abyss

Dive into the enigmatic depths of the ocean in **Blue Abyss** – an immersive underwater exploration game where you'll embark on a captivating journey to uncover the mysteries of the deep sea. Explore the breathtaking underwater world, hunt mesmerizing marine life, and trade your treasures as you navigate the aquatic realm. Experience the challenges and wonders of underwater exploration as you venture into the boundless Blue Abyss.

![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/95a6208e-5733-4599-adf5-187b6dbc3a1b)


<br>

## Table of Contents
1. [Gameplay](#gameplay)
2. [Development Journey](#development-journey)
3. [Fish Behaviour Script](#fish-behaviour-script)
    - [Example Code](#example-code)
4. [Screenshots](#screenshots)
5. [How to Play](#how-to-play)
6. [Credits](#credits)

<br>

## Gameplay

**Blue Abyss** offers a thrilling underwater adventure where you take on the role of an intrepid explorer delving into the ocean's secrets. Immerse yourself in a dynamic and engaging gameplay experience with the following features:

- **Hunt and Gather:** Venture into the depths to hunt exotic marine life. Use your skills to catch a variety of fish species and reap rewards.

- **Trade and Profit:** Visit the local butcher to sell your hard-earned catch. Accumulate wealth to upgrade your equipment and unlock new opportunities.

- **Discover Hidden Treasures:** Explore the underwater world and search for collectibles that hold valuable secrets. Uncover the mysteries that lie beneath the surface.

- **Upgrade Your Arsenal:** Enhance your gear by purchasing submarines and vehicles, allowing you to explore deeper and more dangerous parts of the ocean.

- **Embark on Quests:** Engage with unique characters and undertake quests that unravel the story. Complete missions to earn the trust of a renowned professor who can build you a specialized vessel for deeper exploration.

- **Unlock the Abyss:** Conquer challenges, solve puzzles, and meet the professor's criteria to unlock the ultimate goal—a state-of-the-art vessel that grants you access to the abyss.

With a focus on quest-driven progression and a rich underwater world to explore, **Blue Abyss** provides an immersive gameplay experience that combines action, strategy, and discovery.

![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/3691770f-906b-488e-8ec0-6be6d8a94d8b)

<br>

## Development Journey

During the development journey of the game, I dedicated my evenings to working on this project after completing my daytime internship work. Over the course of two months, I dedicated approximately 5 hours each day to creating an immersive underwater exploration experience.

As the project's scope expanded, I delved deep into mastering the Unity engine. My focus shifted toward achieving optimal performance and integrating captivating features. The project's growth prompted a meticulous approach to code organization, ensuring its modularity for seamless expansion.

Amidst this journey, I ventured into the realm of 3D modeling and animation, acquiring proficiency in Blender to create complex 3D models and breathe life into them through animation. This comprehensive approach enabled me to incorporate great visuals and engaging interactions into the game, elevating the player's immersion in the mysterious depths of the ocean.

![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/a8e74b17-8cf9-4daf-888e-01a941baad6a)

Throughout, I used Trello to maintain organization and structure, streamlining the development process. Moreover, optimizing performance became a significant challenge. I invested substantial effort to ensure the open-world game runs smoothly on a wide range of hardware, delivering an enjoyable experience to players across different machines.

At every point, I worked hard to come up with the best ways to add new features. It wasn't just about making a great game; it was also important to keep the code organized and clear, so that I can easily make improvements and changes later on.

<br>

## Fish Behaviour Script

### Example Code:
- [Link to Project Scripts Folder](Assets/Scripts/)
- [Link to Fish Behaviour Script](Assets/Scripts/Fish/EnemyPatrol.cs)


The **Fish Behaviour** script plays a pivotal role in bringing the underwater world of "Blue Abyss" to life. One of the key decisions behind its design was the implementation of a transform-based movement system. This approach prioritizes using transforms over physics rigidbodies, resulting in enhanced performance, particularly when a multitude of fish inhabit the expansive ocean environment. With the potential for hundreds of fish in the game, this decision greatly contributes to ensuring a seamlessly fluid and immersive gameplay experience.

Central to the fish AI is the dynamic checkpoint system, offering a nuanced array of behaviors that breathe life into each aquatic inhabitant. Parameters such as turning radius, movement speed, and aggression levels dictate the unique characteristics of each fish. This results in a mesmerizing diversity of swimming patterns and interactions as the underwater ecosystem comes to life.

A notable feature of the fish AI is its sophisticated obstacle avoidance mechanism. By projecting three lines from the front of each fish, obstacles are detected in real-time. If an obstacle is encountered, the fish gracefully adjusts its course by rotating until a clear path is ensured, enriching the realism and authenticity of their underwater navigation.

![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/ddaa6009-4e13-4f0a-85df-932356cb4f44)

Moreover, the fish AI system demonstrates intelligent optimization strategies. Fish are dynamically despawned when they venture far from the player's vicinity, ensuring computational resources are allocated where they matter most. Furthermore, animations are selectively disabled for fish that are at a considerable distance from the player, maintaining a harmonious balance between performance and visual fidelity.

<br>

## Screenshots
![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/1a91265c-1a93-40a2-bb3a-42fd62e1388b)
![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/80dc9d7f-a850-4713-8252-b11a606aad0d)
![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/ab180dbb-3197-4f9e-9a6e-55885649fe10)
![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/e2897351-4255-4dc4-9842-1d0bf5b0ff6c)
![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/53067cb2-191a-4dac-b505-072a31bc1543)
![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/b215326b-7eb3-4c8d-ba07-4bf12b6d8463)
![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/56702926-c9d9-4f00-a84c-11109d853a86)
![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/25e6b6be-33ea-4d36-9b6d-51f9be16f672)
![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/f8a47546-0729-4a91-8159-d5a10fdcee12)

<br>

## How to Play
1. Download the game from [itch.io](https://micnasr.itch.io/blue-abyss).
2. Extract the downloaded file.
3. Run the executable to launch the game.

<br>

## Credits

- Developed by: **Michael Nasr**
