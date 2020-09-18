# SoDataDumpExtractor

I made this to quickly extract all the Stack Overflow Data dump 7z files.

It will tie up the CPU on your system entirely (I guess as a Todo it shouldn't do this); but it's made to use all your cores.

Usage:

`SoDataDumpExtractor.exe <full-path-to-where-7zs-are>`

Example:

`SoDataDumpExtractor.exe e:\stackexchange`

Which would result in each Stack Exchange site being extracted to:

`e:\stackexchange\<file-name-without-7z>\<contents-of-7z-file>`

I use it to extract the Stack Exchange Data Dump into workable directories.
