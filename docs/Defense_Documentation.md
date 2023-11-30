# Foreign Language Immersion Virtual Reality (FLIVR) Defense Documentation

**Student: Michael Nicholson**

**Graduation Date: December 2023**

**Major & Degree: Cybersecurity**

**Project Advisor: Dr. Sean Hayes**

## Statement of Purpose:

There are two primary approaches to foreign language acquisition: traditional or academic learning and immersive learning. Academic learning is geared towards attaining proficiency, which involves achieving a systematic understanding of vocabulary, grammar, writing, and practical application. Fluency in a language is a sign of how comfortable and easy it is for a person to utilize the proficiency they possess. Immersive language learning’s main byproduct is fluency and is what everyone experiences learning their native language. Immersive language learning inherently yields fluency, mirroring the natural progression observed in acquiring one's native language.

According to former CEO Michael Schutzler of LiveMocha, a language learning social media site later bought by Rossetta Stone, traditional classroom-based language training results in a 99.5% failure rate of achieving basic conversational fluency (Schutzler). I believe this failure rate is due to methodology of the teaching and a misunderstanding of how the brain chooses to retain and recall information. Foreign Language Immersion Virtual Reality (FLIVR) aims have the opposite approach of traditional methodology which focuses on using only declarative (explicit) memory, towards one that focuses more on engaging a person’s procedural (implicit) memory.

## Research & Background:

The conventional perspective on language learning often stems from experiences in mandatory high school classes, where the emphasis is placed on continuous and rigorous study of languages such as Spanish or French. While dedicated studying can certainly expedite progress, it is insufficient for achieving fluency. Merely engaging explicit memory without addressing other cognitive processes essential for natural language use hinders the desired outcome of language acquisition.

A telling illustration of this challenge surfaced in advice I received from my high school Spanish teacher, who cautioned against relying on translating Spanish words into English. This practice adds an unnecessary step absent in native language usage. Unlike the effortless recognition of an apple as an apple in one's native language, attempting to translate unfamiliar words creates a cognitive barrier. Many language learners, unsuccessful in their attempts, often grapple with translating in their heads rather than establishing direct associations between words and their corresponding concepts.

Foreign Language Immersion Virtual Reality attempts to address this issue by prioritizing object discovery and providing a visual and auditory context for vocabulary. Unlike traditional flashcard-based approaches, FLIVR encourages learners to associate words with tangible objects, mirroring the implicit connections made when remembering names in day-to-day life. Just as a person's significance influences the remembrance of their name, the context in which words are encountered affects language retention.

An example of a practical application of this approach could be demonstrated by a chef seeking to learn ingredient and tool names in a different language. Beyond conventional flashcards, the chef engages with recipes written in the target language, labeling ingredients and tools accordingly. This immersive exposure ensures repeated subconscious encounters with the language, prompting the brain to adapt and form associations without reliance on the native language. Gradually, individuals adopting this methodology develop a "second brain," enabling a seamless transition between their native language and the target language, eliminating the constant need for translation.

## Project Languages, Software, and Hardware:

**Project Language: C#**

**Data Structure: JSON (JavaScript Object Notation)**
 - Visual Studio Code (version 1.84.2)

**Integrated Development Environment (IDE): Unity Personal
Version: LTS 2021.3.26f1**

 - Source Code Editor: Visual Studio Community 2022 (version 17.8.1)

**Hardware:**

 - Lenovo Legion Y-530
 - CPU: Intel(R) Core(TM) i7-8750H CPU @ 2.20GHz
 - GPU: NVIDIA GeForce GTX 1050Ti 4.0 GB
 - RAM: 15.9 GB

**Oculus Quest 2:**

 - Processor: Qualcomm Snapdragon XR2

 - RAM: 6 GB

 - Storage: 256 GB

**Operating System: Windows 11 Home**

 - Version: 22H2

 - Build: 22621.2715

## Project Requirements: [Requirements](https://github.com/Mick7028/CSU-Senior-Project/blob/ad047f2edbd79d43e1cad58ae43027efb46fcc01/docs/Requirements%20Document%20Fall%202023.pdf/)

## Project Implementation Description & Explanation:
[**Source Code**](https://github.com/Mick7028/CSU-Senior-Project/tree/master/src)
[**Unity Scripts**](https://github.com/Mick7028/CSU-Senior-Project/tree/master/src/Scripts)

### Main Menu

When FLIVR starts, the Main Menu is instantiated and the player has three main options to choose from: Start Game, High Scores, and the Game Instructions. Selecting Start Game uninstantiates the Main Menu, selecting High Scores shows the top 10 Multiple Choice Activity scores. Selecting Game Instructions brings you to a buffer menu that allows you to select either the Game Controls Menu or the Game Instructions Menu (future feature that will include video tutorials of the activities).

![screenshot](/../master/media/Menu/Main_Menu.png)
**Fig 1. Main Menu**

![screenshot](/../master/media/Menu/High_Scores.png)
**Fig 2. High Scores**

![screenshot](/../master/media/Menu/Game_Controls_Instructions.png)
**Fig 3. Game Controls and Instructions**

![screenshot](/../master/media/Menu/Game_Controls.png)
**Fig 4. Game Controls**

### Game Play and Save Functionality

The backbone of almost every feature in FLIVR is the function "OnHover" located in Hoverable.cs. Hovering over an object displays the name of the object, allows the player to press A to hear it's pronunciation which adds it to the "InteractedObjects" array in the SaveData.json, and allows for the player to perform the actions needed in both the FlowerQuest and the Multiple Choice Activity.  

![screenshot](/../master/media/Gameplay/Hover_Name.jpg)
**Fig 5. Player hovers ray interactor over a tree to display it's name. If A button is pressed, player will hear its pronunciation and it will be added to database array "InteractedObjects".**

![screenshot](/../master/media/Menu/SaveData_Top.png)
/**Fig 6. Save Data: Amount of Times Played for Multiple Choice Activity and Flower Quest. Objects that have been interacted with (pressed A button while hovering) and last saved player position on map (the game saves player postition data after completing an activity and every time they open the main menu).**

![screenshot](/../master/media/Gameplay/Flower_Quest_Start.jpg)
**Fig 7. Player hovers over NPC before starting Flower Quest**

![screenshot](/../master/media/Gameplay/Before_Flower_Press.jpg)
**Fig 8. Before picking flower, Heads Up Display(HUD) shows how many of each flower needs to be picked.**

![screenshot](/../master/media/Gameplay/After_Flower_Press.jpg)
**Fig 9. Player presses B button to pick flower, HUD displays updated inventory of flowers.**

![screenshot](/../master/media/Gameplay/Finished_flower_quest.jpg)
**Fig 10. Player presses B button on NPC to hand over the flowers and finish their quest.**

![screenshot](/../master/media/Menu/SaveData_Middle.png)
**Fig 11. Save Data Continued: List of game objects in Multiple Choice Activity which is ordered top-down from lowest average score to highest average score.**

![screenshot](/../master/media/Gameplay/Multiplayer_Start.jpg)
**Fig 12. Player hovers over NPC before starting Multiple Choice Activity.**

![screenshot](/../master/media/Gameplay/Before_Multi_Press.jpg)
**Fig 13. HUD displays name of the object that needs to be chosen by player.**

![screenshot](/../master/media/Gameplay/After_Multi_Press.jpg)
**Fig 14. HUD displays new object that needs to be chosen by player, correctly chosen objects become uninstantiated.**

![screenshot](/../master/media/Gameplay/Multiplayer_Highscore.jpg)
**Fig 15. Displays if player achieves a top 10 score. Player can enter their initials (up to 3 letters).**

![screenshot](/../master/media/Gameplay/Updated_High_Score.jpg)
**Fig 16. Updated high scores (new score is under initials AAA).**

![screenshot](/../master/media/Menu/SaveData_Bottom.png)
**Fig 17. Save Data continued: List of fastest total times (in miliseconds) of the Multiple Choice Activity and the high scores for it.**

## Test Plan: (Test Plan Link)

## Test Results: (Test Results)

## Challenges Overcome:

Undertaking the development of a virtual reality game for my senior project was a significant learning experience; one I approach with a sense of reflection and growth. Embarking on this journey without any previous game development experience, I naively chose to create a virtual reality experience, with the vision of seamlessly incorporating language immersion into a realistic city setting. Over the course of 2 ½ years, the journey took unforeseeable turns which included transitioning from Unreal Engine to Unity, acquiring proficiency in C#, and navigating the intricacies of managing a JSON database. Despite facing challenges that extended the project timeline and led to a final product diverging from my initial vision, the valuable insights gained from this ambitious undertaking outweigh the mental anguish I felt all throughout the construction.

I encountered a copious number of technical hurdles, ranging from everyday bugs in the GameManager script to unpredictable crashes in both Unity and QuestLink. Moreover, encounters with unresolvable Unity and Oculus bugs necessitated the development of alternative strategies to circumvent these obstacles. Notably, a persistent issue surfaced during the implementation of high score functionality within the multiple-choice activity. Prompting users to enter their initials upon achieving a new high score posed an unforeseen challenge—Oculus' virtual keyboard failed to appear both in the editor and post-build to the headset. I later discovered that this dilemma, linked to the use of XR Interaction Toolkit instead of Oculus Integration, had persisted over the years, despite community grievances on forums. Unfortunately, the only solution available involved hardcoding a keyboard into the game, an endeavor that could have been mitigated had I opted for Oculus Integration, where a virtual keyboard seamlessly appears upon selecting the input field.

## Future Enhancements:

Many enhancements can be made to this program including voice recognition to help the user practice their pronunciation, more activities and game objects, and improved ways for users to track their progress. With the recent release of the Oculus Quest 3 my plans for this project have shifted to researching the capabilities of augmented reality (AR) instead.

The Quest 3 features color passthrough and has a much wider field of view than the Quest 2. I believe that with the help of artificial intelligence (AI) and object recognition software a person’s entire home could become its own language immersion environment. As technology gets smaller and better, people might wear these devices while doing everyday things like cleaning, cooking, or playing games, and they can pick up a language’s vocabulary without really trying. With the Quest 3 AR, users can also put virtual objects in their space, making it feel more like a home that matches the culture of the language they are learning.

This technology could lead to more sophisticated active learning opportunities, incorporating real-life objects and activities in one's home. AI could dynamically change any text observed in real-time into the target language, facilitating activities such as reading books entirely in a different language, or the headset could replace specific vocabulary and phrases, providing an effective method for gradual language script adaptation while allowing users to enjoy their preferred content.

## Defense Presentation Slides: (Presentation Link)


## References:

Brito, A. C. (2017). _Effects of Language Immersion versus Classroom Exposure on Advanced French Learners: An ERP Study_. Pursuit - The Journal of Undergraduate Research at The University of Tennessee. [https://trace.tennessee.edu/cgi/viewcontent.cgi?article=1358&context=pursuit](https://trace.tennessee.edu/cgi/viewcontent.cgi?article=1358&context=pursuit)

Schutzler, M. (2013, March 7). _Michael Schutzler's answer to "What is the success rate of learning a foreign language in the world, measured by the number of people who learned a foreign language for 3 years or more and who can express their thoughts in a foreign language?"._ Quora. [https://bit.ly/31UDEq9](https://bit.ly/31UDEq9).

The Human Memory. (2022, May 20). _Declarative memory and procedural memory: Explicit & implicit_. The Human Memory. [https://human-memory.net/explicit-implicit-memory/#Introduction_to_Declarative_and_Procedural_Memory](https://human-memory.net/explicit-implicit-memory/#Introduction_to_Declarative_and_Procedural_Memory)