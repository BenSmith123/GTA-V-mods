# GTA V Mods

.NET / C# scripts for modding Grand Theft Auto V on PC

### IDE
- Visual Studio 2019 (Community edition)

### Requirements:
- Reference these in Visual Studio project `(in references/)`
	- ScriptHookVDotNet.dll (NOTE: Do not use ScriptHookVDotNet2 or ScriptHookVDotNet3 even though they are the latest)
	- NativeUI.dll
- Ensure the GTA V folder also has the two files above in it

### Build/Deploy
- CTRL + SHIFT + B to build .cs file into .dll, this will also automatically deploy to my 'GTAV folder/scripts'

## Description: 
Instantly repair / clean your vehicle!

- (Keys configurable)
	- X = Repair vehicle
	- C = Clean vehicle

- Options:
	- Partial fix: Only fix the damage to the body SHAPE - leave broken windows/lights and scratches
	- Leave front windows down - this means the character won't have to break the window to shoot after every repair
	- Only fix vehicle cosmetics, not engine health
	- Clean on repair

- Installation:
	- Put the files into the /scripts folder

- Requirements:
	- ScriptHookV (http://www.dev-c.com/gtav/scripthookv/)
	- Community Script Hook V .NET (https://github.com/crosire/scripthookvdotnet/releases)
