# Unity-Audio-Scripts
Hello,

This is a collection scripts I use for implementing audio in Unity. I routinely use these scripts in my work and I think other people might also find it useful. Also this is pretty much an ever going work in progress, with me constantly tweaking and addding scripts as the need arises.

The **"Unity Native Audio"** folder contains scripts for generic dynamic audio systems using the native audio engine from Unity. It's separated in two other folders: 

**Scriptable Objects Systems** - a collection of Monobehaviours systems that makes use of the AudioClipCueSO and AudioConfigurationSO Scriptable Objects to hold data for audio clips and audio source settings. These scripts offer the implementation of generic systems to do things like play one shots with pitch randomization, play audio clips intermittently between random times, concatenate random audio clips with sample accuracy, and loop music with reverb tails. **Almost all of these scripts call functions from the AudioUtility script**, so you should either copy that class to your project or refactor my scripts if you already have a similar class.

**General Scripts** - offers the same types of systems as the Scriptable Objects Systems but all references to data are stored in the Monobehaviours themselves. Albeit less modular and a little less robust, these scripts are more straightforward to use than their counterparts.

The **"FMOD** folder contains scripts for doing simple implementation when using FMOD, like playing one shots, start and stopping events, changing parameter values, and some basic bus control.

**Feel free to use any of these scripts in any project, commercial or not.**
