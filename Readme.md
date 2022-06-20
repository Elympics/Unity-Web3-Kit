# Elympics Web3Kit

TODO

# Installation

Add this repository as a unitypackage in UPM. TODO: address

You will be presented with a first time setup wizard. Begin the setup as prompted. This will copy necessary files from the package directory (which is immutable for you) to your project assets, where you can edit scriptable objects and prefabs.

After a successful import, the wizard will ask for additional settings:

1. Elympics Room API uri - this can be left blank if you don't use this feature | TODO: do we have a default api?
2. Blockchain integration - you can leave this unchecked and the game will run in a "play for free" mode - without any user authentication and without smart contract features like betting.
3. Smart contract address & Chain address - if you turn on blockchain integration you need to specify these addresses
4. Manage games in Elympics - this is the next step required to work with Elympics. You don't have to do this right away, but it is necessary to deploy and start testing your game.

# Use

Two main features of Web3Kit are:

## Title screen

Drag & drop TitleScreenController prefab to the first scene in your game. This will handle user authentication, wallet chain and account validation and token approval. If blockchain integrations is turned off, this screen will only contain a play button

## Main Menu

Drag & drop MainMenuController prefab to your second scene. Here you will find betting menu, challenge a friend popup, play for free as well as the matchmaking screen and error handling. If blockchain integration is turned off, "play for free" will be the only available option.

## Advanced editing

Prefabs and scriptable objects are copied to Assets/Web3Kit. You can edit all these files there and change the looks of the menus, provide different texts and styles and edit the configuration. Remember to preserve the directory structure in Resources.

A good practice is to make prefab variants of the original prefabs. This will make package updates easier.
