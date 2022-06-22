# Elympics Web3Kit

This is a complete toolset for creating games on web3 technology stack, using Elympics SDK for skill-based, play & earn multiplayer. It comes ready with UI, blockchain integration, smart contract integration, metamask authentication, error handling and more. 

# Installation

Add this package in UPM using git repository link: `https://github.com/Elympics/Unity-Web3-Kit.git`

You will be presented with a first time setup wizard. Begin the setup as prompted. This will copy necessary files from the package directory (which is immutable for you) to your project assets, where you can edit scriptable objects and prefabs.

After a successful import, the wizard will ask for additional settings:

1. Elympics Room API uri - this can be left blank if you don't use this feature
2. Blockchain integration - you can leave this unchecked and the game will run in a "play for free" mode - without any user authentication and without smart contract features like betting.
3. Smart contract address & Chain address - if you turn on blockchain integration you need to specify these addresses
4. Manage games in Elympics - this is the next step required to work with Elympics. You don't have to do this right away, but it is necessary to deploy and start testing your game.

# Use

Two main features of Web3Kit are:

## Title screen

Drag & drop TitleScreenController prefab to the first scene in your game. This will handle user authentication, wallet chain and account validation and token approval. If blockchain integrations is turned off, this screen will only contain a play button

## Main Menu

Drag & drop MainMenuController prefab to your second scene. Here you will find betting menu, challenge a friend popup, play for free as well as the matchmaking screen and error handling. If blockchain integration is turned off, "play for free" will be the only available option.

Additionaly, in main menu you will see a "Debug Options" panel, where you can play locally or using half remote. Check docs.elympics.cc for more details.

## Advanced editing

Prefabs and scriptable objects are copied to Assets/Web3Kit. You can edit all these files there and change the looks of the menus, provide different texts and styles and edit the configuration. Remember to preserve the directory structure in Resources.

A good practice is to make prefab variants of the original prefabs. This will make package updates easier.
