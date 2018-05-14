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
