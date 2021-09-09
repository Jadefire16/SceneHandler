# SceneHandler
A small tool developed for Unity to assist beginner programmers in additive and "Asyncronous" scene loading features provided by Unity.

Developed in Unity Version: 2019.4.28

## Getting Started:
#
#### Download Unity:
- Download a stable Unity version. Note that this tool was built in Unity Version 2019.4.28 And will likely be suported for a fairly long time passed this version however I cannot gaurantee easy porting beyond 2019.4 LTS.

#### Clone repository:
- Clone repository -> See here https://github.com/github-for-unity/Unity/blob/master/docs/using/getting-started.md
- Setup environment as specified in provided link

#### Package Manager:
- Download package from /Packages folder.
- Open Unity
- Nagivate to Assets -> Import Package -> Custom Package
- Select downloaded package
- Select files you wish to install -> Core files are needed for scenehandler to run.


## Usage
#
### Core
The SceneHandler can be placed on an object, it is a Singleton and thus should only ever be placed on one object. To use it simply call SceneHandler.Manager and it will either fetch the current active instance.

There are a few different functions and properties you can choose from and a few options for each. Let's go over them.

##### LoadSceneAsync
This method will allow you to load a method either using the name of the scene or the scene's buildIndex, preferably the latter, in an "asyncronous" like fashion. However this is not to be confused with actual C# async methods and does not need to be awaited.
```
Args:
string -> key 
or
int -> index;

LoadSceneMode -> (Single/Additive) //Do you want to load the scene additively or not?
```

##### UnloadSceneAsync
This method will allow you to unload a method either using the name of the scene or the scene's buildIndex, preferably the latter, in an "asyncronous" like fashion. However this is not to be confused with actual C# async methods and does not need to be awaited. 
```
Args:
string -> key 
or
int -> index;
```

##### Events
There are 6 events fired at different points during a scene load cycle, they are:
- SceneLoadStart -> When the scene loading has started
- SceneLoading -> Will by fired every frame containing the current percentage of scene loading
- SceneLoaded -> When the scene loading has finished containing an object which holds the name & index of the scene
- SceneUnloading -> Will by fired every frame containing the current percentage of scene unloading
- SceneLoaded -> fired when scene unloading has finished, contains an object which holds the name & index of the scene

##### Properties
Unfortunately there isn't much functionality here yet, the one being:
- MainScene -> The scene in which the SceneHandler object was originally instantiated


### Addons
#
##### SceneLoadTrigger
this simple script simply shows how you can leverage some of the functionality of the SceneHandler.
When dragged onto an object this trigger will generate a trigger collider and provide the specified object a few functionalities. These include the ability to trigger scene loading and unloading when an object enters the trigger. Object filtering to ensure unwanted objects don't fire the trigger (using Layermasks). and a reset or destroy functionality when the trigger has been activated, allowing you to either reset the trigger after a set time or simply destroy the object.