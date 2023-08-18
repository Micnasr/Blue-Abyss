# Blue Abyss

Dive into the enigmatic depths of the ocean in **Blue Abyss** – an immersive underwater exploration game where you'll embark on a captivating journey to uncover the mysteries of the deep sea. Explore the breathtaking underwater world, hunt mesmerizing marine life, and trade your treasures as you navigate the aquatic realm. Experience the challenges and wonders of underwater exploration as you venture into the boundless Blue Abyss.

![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/a191f685-f692-4560-8ff7-f27d5cc90244)

<br>

## Table of Contents
1. [Gameplay](#gameplay)
2. [Development Journey](#development-journey)
3. [Fish Behaviour Script](#fish-behaviour-script)
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

![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/b6cb9906-9631-40ef-9849-7ccb56618049)

<br>

## Development Journey

During the development journey of the game, I dedicated my evenings to working on this project after completing my daytime internship work. Over the course of two months, I dedicated approximately 5 hours each day to creating an immersive underwater exploration experience.

As the project's scope expanded, I delved deep into mastering the Unity engine. My focus shifted toward achieving optimal performance and integrating captivating features. The project's growth prompted a meticulous approach to code organization, ensuring its modularity for seamless expansion.

Amidst this journey, I ventured into the realm of 3D modeling and animation, acquiring proficiency in Blender to create complex 3D models and breathe life into them through animation. This comprehensive approach enabled me to incorporate great visuals and engaging interactions into the game, elevating the player's immersion in the mysterious depths of the ocean.

![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/8e18c422-7ee6-4bdd-acaf-fd14c6c43aa8)

Throughout, I used Trello to maintain organization and structure, streamlining the development process. Moreover, optimizing performance became a significant challenge. I invested substantial effort to ensure the open-world game runs smoothly on a wide range of hardware, delivering an enjoyable experience to players across different machines.

![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/c7113b15-d3c0-489f-bfc0-0265b585be4d)

At every point, I worked hard to come up with the best ways to add new features. It wasn't just about making a great game; it was also important to keep the code organized and clear, so that I can easily make improvements and changes later on.

<br>

## Fish Behaviour Script

### Example Code:
- [Link to Project Scripts Folder](Assets/Scripts/)
- [Link to Fish Behaviour Script](Assets/Scripts/Fish/EnemyPatrol.cs)


The **Fish Behaviour** script plays a pivotal role in bringing the underwater world of "Blue Abyss" to life. One of the key decisions behind its design was the implementation of a transform-based movement system. This approach prioritizes using transforms over physics rigidbodies, resulting in enhanced performance, particularly when a multitude of fish inhabit the expansive ocean environment. With the potential for hundreds of fish in the game, this decision greatly contributes to ensuring a seamlessly fluid and immersive gameplay experience.

Central to the fish AI is the dynamic checkpoint system, offering a nuanced array of behaviors that breathe life into each aquatic inhabitant. Parameters such as turning radius, movement speed, and aggression levels dictate the unique characteristics of each fish. This results in a mesmerizing diversity of swimming patterns and interactions as the underwater ecosystem comes to life.

A notable feature of the fish AI is its sophisticated obstacle avoidance mechanism. By projecting three lines from the front of each fish, obstacles are detected in real-time. If an obstacle is encountered, the fish gracefully adjusts its course by rotating until a clear path is ensured, enriching the realism and authenticity of their underwater navigation.

![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/356dfca6-439a-4d2e-8e88-728316cee034)

Moreover, the fish AI system demonstrates intelligent optimization strategies. Fish are dynamically despawned when they venture far from the player's vicinity, ensuring computational resources are allocated where they matter most. Furthermore, animations are selectively disabled for fish that are at a considerable distance from the player, maintaining a harmonious balance between performance and visual fidelity.

<br>

## Screenshots
![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/7ec32c25-e9b0-4cd3-adb3-f5b4eadfe0d5)
![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/c6d46769-2479-4a8b-875c-51224251224f)
![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/12facedf-78c4-45c8-b95d-ba1425283998)
![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/f376dff1-edfb-4c95-ad64-14aa6bcd6063)
![image](https://github.com/Micnasr/Blue-Abyss/assets/44876651/d4a31bb4-cb91-4258-9847-62f230f900a8)

<br>

## How to Play
1. Download the game from [itch.io](https://micnasr.itch.io/blue-abyss).
2. Extract the downloaded file.
3. Run the executable to launch the game.

<br>

## Credits

- Developed by: **Michael Nasr**
