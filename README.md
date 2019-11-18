# Reverse Angry Bird

## Tools & Version
Unity Version: 2019.1.8f1: WebGL module \
Burst Version 1.1.1 \
Additional package might needed: \
Nuget: Now cannot directly download from the assets store. Can download the package at [Nuget Gallery](https://www.nuget.org/packages/Unity/)\
ProtoBuf.Net Serialized data from .Net code to protobuf buffer serialization format by Google. More info at protobuf-net github. Can install from Nuget package or import from [Protobuf-net github](https://github.com/protobuf-net/protobuf-net) \
Firebase CLI: Firebase App and firebase storage should be install in the google cloud shell. [Firebase Hosting tutorial](https://firebase.google.com/docs/hosting/?gclid=Cj0KCQjwivbsBRDsARIsADyISJ_ZQI5y0gWzIHM76_x8tL4W4Q1FrZMr2HjFOXkfifxjHSwitOKde3waAvnKEALw_wcB)

## Important note
The project is set up for the WebGL. Enable to run in the Unity editor have to disable the authentication set up
Go to Landing Scene => Set Authentication to not active and set Canvas to active \
![](https://imgur.com/QmCxUKj.png)

## How to use
```install correct Unity version with WebGL module``` \
```git clone ~ ``` \
```open Unity hub ```\
```use Unity hub to open the folder```\ 
```compile with WebGL platform```\
```deploy at firebase hosting```\

## Outline
Reverse Angry Bird contain five different Scene:

- Robotic Arm Landing Scene \
Opening Scene. Contain GameMaster that control the entir game \
lead to Build Scene / Replay Scene 

- Robotic Arm Build Scene \
Main scene for user control. Two robot to build houses 

- Robotic Arm Replay Scene \
Replay Scene. Control the replay process 

- Robotic Arm Disaster Scene \
Disaster automatic trigger earthquake or hail 

- Robotic Arm Game Over \
Last Scene. Send feedback and trjectory to the bucket 

## Docs
[Data transfer on WebGL](https://github.com/JCTGY/rabshare/blob/master/Data%20Transfer%20WebGL%20to%20Google%20Cloud.pdf)
