# G3Randomizer
G3Randomizer is a Gothic 3 data randomizer written in C# utilizing cropped version of [G3Archive](https://github.com/gekonnn/G3Archive)

> [!IMPORTANT]
> G3Randomizer requires at least .NET 7.0 installed

# Installation
1. Download the [latest release](https://github.com/gekonnn/G3Randomizer/releases/) from Releases tab
2. Customize `ignored_extensions.txt` and `target_archives.txt` if needed (pretty self-explanatory)
3. Run the `.exe` file and specify the game path
4. Move all the folders from `Extracted` folder to `Data` folder in Gothic 3 installation directory

# Notes
- The program is in the very early beta version, meaning that it probably won't be fully functional.
- Please take note that the randomizing process may take a long time depending on the selected archives.
- Randomizing some archives such as `Templates.pak` might lead to game crashes or make the game unplayable (not fully tested yet)
- If the game keeps crashing on startup, try renaming or deleting some of the generated folders
- New files are stored in folders, meaning they don't overwrite any files and can be deleted anytime.