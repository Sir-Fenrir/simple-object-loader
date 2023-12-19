# Simple Object Loader

Made to make the creation of new furniture etc. easier, as long as no complex/new behaviors are required.

## Setup
When creating new objects to load using this mod, there are a few steps that are required:

1. Create a new directory for your mod.
2. In your mod directory, create a file _ending_ in '.simple.json', for example: 'mymod.simple.json'.
3. In your mod directory, create a directory called 'Localization'.
4. In the Localization directory, create at least _one_ file, with the name corresponding to a shorthand for a language. For example 'en.json' for English.

That's the basic setup, easy, right? Well, it gets a bit more difficult from here, after all, we do need some information to load your custom objects.

## JSON
To create mods for the Simple Object Loader, you'll need to use _JSON_, a standard for storing data. I'm not going to fully explain JSON in this README, but here is a start at [W3Schools](https://www.w3schools.com/whatis/whatis_json.asp).

If you still have questions, there are many resources available online, just a Google-search away.

## .simple.json
The file you made contains the configuration for your objects. You can either have one JSON object, or an array of JSON objects for multiple new objects.

All (currently) supported properties are as follows:

**modId**

Unique name for the mod. You can have multiple items with the same identifier, but the name needs to be unique within this mod.

**type**

This is for specific types of furniture, the possible types are listed under the <b>Derived</b> section on <see href="https://docs.tinylifegame.com/api/TinyLife.Objects.Furniture.html"/>.

**name**

The name of the item when registering it.
NOTE: This is not the name used in the game, for that we need the localization files.
This name is used to find the relevant entries in the localization files and the atlas file.

**price**

The price of the object, if relevant.

**categories**

The categories this object belongs to, if applicable. 
The possible categories are listed on https://docs.tinylifegame.com/api/TinyLife.Objects.ObjectCategory.html under the header **Fields**.

**tab**

For when you want to specify which tab the object appears in in the build menu.
Possible tabs are under the tag <b>Fields</b> on https://docs.tinylifegame.com/api/TinyLife.Tools.FurnitureTool.Tab.html.

**textureFile** *(Not yet used)*

If this object requires a specific texture (like wallpapers or clothing), we need the name of the file.

**atlas**

When adding furniture, we need an atlas file describing the orientation of the texture file of the same name.

**size**

If you're creating furniture, we need the size. It expects an array of two values, 
the first being the x, the second being the y, for example: \[1, 1]

**colorSchemes**

The ColorSchemes you want to use for the furniture, can be multiple.
For options, take a look under the header <b>Fields</b> on https://docs.tinylifegame.com/api/TinyLife.Utilities.ColorScheme.html.

**defaultRotation**

In case you want to set a default rotation for the object, for options see https://mlem.ellpeck.de/api/MLEM.Misc.Direction2.html.

**tableSpots**

If you're creating a table, you need some spots to sit. Generally this can be the same as Size.

**actionSpots**

All the places a Tiny can interact with/from with the furniture. I'll be honest, not 100% sure on how this works. Best understood by seeing this example mod: https://github.com/Sir-Fenrir/tiny-life-simple-expensive-chair

**colorMap**

Set the colors for the different layers, if applicable.
It is an array, with as many elements as there are layers in your textures. 
The numbers in it correspond to the given ColorSchemes. 
If you have three layers and two ColorSchemes, this could be \[1, 0, 1], with the 0 referencing the first ColorScheme, the 1 the second.

**needModifier**

When this value is set, it modifies the need restoration rate of the need for this type of furniture. It should be a decimal value using points, not comma's (i.e., 1.25).

### actionSpots
In the previous list we saw the property 'actionSpots', which in itself needs some properties (remember the JSON tutorials? You can nest things!).

These properties are as follows, but I have to admit, I'm not much of a texture-guy, so I'm not too sure myself on how these work exactly.

**vectorX**

**vectorY**

**yOffset**

**direction**

For options for this, look under the **Fields** section on https://mlem.ellpeck.de/api/MLEM.Misc.Direction2Helper.html.

**drawLayer**

Basically, they influence the position the Tiny can interact with the object and it's position relative to the object when using it.

# Atlas files
Atlas files are too complicated to fully explain here, but Ellpeck has it documented here: https://mlem.ellpeck.de/api/MLEM.Data.DataTextureAtlas.html
Simply put, it describes which parts of the texture file with the _same name_ should be used for which piece of furniture and in which orientation.

Let's use an example:
 
File: ExpensiveChair.atlas
```
SimpleObjectLoader.ExpensiveChair.ExpensiveChairLeft
loc 0 0 32 32
piv 16 25

SimpleObjectLoader.ExpensiveChair.ExpensiveChairUp
loc 0 32 32 32
piv 16 57

SimpleObjectLoader.ExpensiveChair.ExpensiveChairRight
loc 0 64 32 32
piv 16 89

SimpleObjectLoader.ExpensiveChair.ExpensiveChairDown
loc 0 96 32 32
piv 16 121
```

(Made by McChicky from the Ellpeck Games Discord, modified by me)

This file (which can be seen in https://github.com/Sir-Fenrir/tiny-life-simple-expensive-chair) tells the game which parts of the texture file of the same name (ExpensiveChair.png) 
should be used for the furniture, depending on the camera orientation.

It's very important to point out that the names you see all start with _SimpleObjectLoader_. The second part, _ExpensiveChair_ is the ModId. 
The last part _before_ Left, Up, Right and Down is the _name_ of the object. The _SimpleObjectLoader_ part is very important, 
because this chair is loaded using the Simple Object Loader mod, which means that in the eyes of the game, my mod is the one responsible for your new furniture, instead of you. 
That also means it will use the id of _my_ mod to load things like textures and localization files. So if you omit the 'SimpleObjectLoader' part, your mod will fail to load.

# Localization files
Localization files are required for supplying names for your new objects. You can create a file per language you want to support. For example:

File: Localization/en.json
```json
{
    "BuildMode": {
        "SimpleObjectLoader.ExpensiveChair.ExpensiveChair": "An Expensive Chair"
    }
}

```

Here you can see the file en.json in the directory Localization. This gives the English names for your objects. As we are adding objects for in BuildMode, we need to put the key-value pair in that part.
Again, you see _'SimpleObjectLoader'_ as part of the name. It's important you don't forget that, otherwise your names won't load. It also has the ModId and the Name, same as in the Atlas file.

# Multiple objects
You can put multiple objects in one mod, just put more items in your .simple.json or create more .simple.json files.

# Testing your mod
You only need to put your mod directory in the mod directory of Tiny Life. On Windows, the default location is `C:\Users\\[YOUR USERNAME]\AppData\Local\Tiny Life\Mods`. So you'd have something like
`C:\Users\\[YOUR USERNAME]\AppData\Local\Tiny Life\Mods\MyCustomFurniture`, containing your .simple.json, your Atlas file, your texture, and your Localization directory with the langauges you want to support.
If you're not sure about the location of the Mods directory, you can also open it from the ingame options menu, on the 'Custom Content' tab.

# Packaging your mod
Simply put the contents of your mod directory in a .zip file.

# Publishing your mod on Steam
This takes a few steps:

1. Start the game
2. Copy your .zip file to the Tiny Life mods directory (`C:\Users\\[YOUR USERNAME]\AppData\Local\Tiny Life\Mods`)
3. Go to the Custom Content tab in the game options.
4. Press 'Share Mod on Steam Workshop'
5. Select your .zip file and follow the steps ingame.

Congratulations! You have created some new type of furniture without having to write code!

## Adding a dependency on Simple Object Loader
A useful feature on the Steam Workshop is the ability to add mods your mod depends on. On the Steam Workshop page for your mod, you can click on 'Add/Remove Required Items' on the right hand side after scrolling down a tiny bit. There you can look for 'Simple Object Loader' and add it! This way, people can subscribe directly to your mod without subscribing to mine directly.


# Help! My mod does not show up in the ingame list of mods!
Yes, that is correct. Basically, the game spots _my_ mod, but not _your_ mod, because your mod doesn't have any code associated with it. My mod aims to load your mod anyway, but it can't make the game aware that there are more mods. This means that, from the perspective of the game, all furniture loaded in this manner belong to Simple Object Loader, meaning, I'm stealing all your credit! Ha!

...I don't want to steal your credit. I'm happy enough with this mod. In the future I will add an user interface element displaying all the mods that have been loaded, but right now I'm focussing on the basic functionalities. 

# Supported items
Frankly, not sure. I have tested chairs and tables, but I haven't tested more types of items, like fridges or beds. I will work on this, but it's a hobby project and I am probably straining my RSI arms too much already :')
If you have tested a different kind of item and it didn't work, please open an issue here on GitHub!

# Example mods
There are two right now:

https://github.com/Sir-Fenrir/tiny-life-simple-expensive-chair
 
https://github.com/Sir-Fenrir/tiny-life-simple-custom-table

# Future Features
I have some plans for upcoming features:

- Support for clothes, hair etc.
- Support for wallpapers and tiles.
- Icon support to show which mod an item is from.
- User interface element showing all the loaded mods.