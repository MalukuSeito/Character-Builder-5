# Character-Builder-5
A Character Builder for D&D 5th Edition.
The Builder is designed to be extendable with your own content (as I am not allowed to put in all the books), 
fast (Only make choices you need to make), and to be both used as a play aid or PDF creator.

The main version is for Windows, but there are versions for Android, iOS and UWP as well (using Xamarin). 
The App versions are currently being worked on.

## Download

Latest version (including SRD 5.1) for Windows can be found under Releases.

## Internals

There are 2 parts to a character in this builder: First the Data Files, that define what classes, subclasses, races, subraces, features, backgrounds, ...
and so on exist. Each Source Book is its own folder in the programs directory. 
Most of these Data Files are a collections of features that give the character ability score increases, proficiencies, spellcasting systems, and many more things.

The second part is the character file, which is a catalogue of choices made during creation. 
Choices like abilities, class(es), race, background, name, picture, backstory, inventory are always logged. 
The Features in the Data Files can also create more choices (spells, equipment, fighting styles, metamagic,...) which are saved in the character file as well.
The program recreates the final character on every change using those saved choices. 

Choices that are no longer needed will be saved for the future, thus allowing you to create a character up to level 20 and then put it back to level 1, with the choices coming back as you level up normally.

## Programs

### Character Builder
The main part of the program, allows you to build characters and export them as PDF.
#### Select Race, Class(es), Ability Scores, Background and set Personal Info
![Race Selection](https://user-images.githubusercontent.com/17147116/39996777-f41bcbe4-5780-11e8-8d37-b155d74ef283.png)
![Class Selection](https://user-images.githubusercontent.com/17147116/39996775-f3fe1e64-5780-11e8-864b-5b016f33ac43.png)
![Ability Scores](https://user-images.githubusercontent.com/17147116/39996774-f3e28dc0-5780-11e8-859e-4d4562571f91.png)
![Background](https://user-images.githubusercontent.com/17147116/39996773-f3c3dc0e-5780-11e8-90ea-719106c25b80.png)
![Personal Info](https://user-images.githubusercontent.com/17147116/39996772-f3a6f382-5780-11e8-9aee-4b356082a7fa.png)

#### Spell Selection for each of your classes
![Spell Selection](https://user-images.githubusercontent.com/17147116/39996771-f383641c-5780-11e8-8587-9675b9e1bb20.png)

#### Manage your summons and polymorph forms
![Forms](https://user-images.githubusercontent.com/17147116/39996770-f3686996-5780-11e8-8db2-30621f5e8ed3.png)

#### Track your progress
![Journal](https://user-images.githubusercontent.com/17147116/39996769-f34c948c-5780-11e8-9bb7-ca8147f304b6.png)

#### Buy/Add items
![Shop](https://user-images.githubusercontent.com/17147116/39996768-f32e7c2c-5780-11e8-8dc3-6d3782e2159a.png)

#### Manage your inventory and customize items
![Inventory](https://user-images.githubusercontent.com/17147116/39996767-f312f3ee-5780-11e8-9c61-cffc70e98989.png)

#### Use the Builder to run your character
The In Play Screen gives you all your needed information at once.
![In Play Screen](https://user-images.githubusercontent.com/17147116/39996766-f2f3f818-5780-11e8-8470-72d8d64a5d73.png)


### Data Browser
Search and Display everything using full text search.
![Data Browser](https://user-images.githubusercontent.com/17147116/39996764-f2b4591a-5780-11e8-99c7-ea023c7390aa.png)

### Character Builder Builder
Add more classes, races, and everything else.
![Character builder builder](https://user-images.githubusercontent.com/17147116/39996765-f2d62694-5780-11e8-8424-b6c960cee504.png)
