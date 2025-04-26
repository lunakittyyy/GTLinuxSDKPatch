# GTUnixSDKPatch
Small patch to fix Gorilla Tag map building on Unix-like operating systems (Linux and MacOS)

## Usage
Drop the ``Editor`` folder into the Assets folder of your project

## The Problem 
The **problem** is that the Gorilla Tag SDK is hardcoded to look for assetbundles ending in ``_Win64`` and ``_Android``, despite the fact that they export as ``_win64`` and ``_android`` on Unix-likes.

This is fine in Windows land, because Windows' file system, NTFS, is case insensitive. This means that ``file``, ``fIlE``, ``FILE``, so on and so forth are all treated as the same filename and cannot coexist.

This is **not** the case on virtually all Unix-like filesystems, such as Ext4 and Btrfs, so when the SDK tries to zip up the assetbundles it built, it cannot find them.

## The Solution
The **solution** is what is contained in this repository. It uses Harmony to patch the SDK in real-time when you open the Unity editor, similarly to [LinuxVRChatSDKPatch](https://github.com/BefuddledLabs/LinuxVRChatSDKPatch/). 

It makes the endings of the aforementioned assetbundles **lowercase** instead of uppercase in the builder script, therefore finding the right file, and all is well!

All it would take for Another Axiom to fix this problem upstream is make two strings lowercase instead of uppercase. That wouldn’t break anything on Windows, because NTFS is case-insensitive and therefore wouldn’t care about the capitalization not matching.

> Case-insensitive names are horribly wrong, and you shouldn't have done them at all. The problem wasn't the lack of testing, the problem was implementing it in the first place.

- Linus Torvalds
