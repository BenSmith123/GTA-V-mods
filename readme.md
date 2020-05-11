# GTA V Mods

.NET / C# scripts for modding Grand Theft Auto V on PC

Each folder is a different project

## Published mods
Instant vehicle repair: https://www.gta5-mods.com/scripts/instant-vehicle-repair 

![Instant vehicle repair](references/img/published-instant-vehicle-repair.png)

Default vehicle weapon: https://www.gta5-mods.com/scripts/default-vehicle-weapon

![Instant vehicle repair](references/img/published-default-vehicle-weapon.png)

## Set up

### IDE
- Visual Studio 2019 (Community edition)
- Open any of the `.csproj` project files is Visual Studio

### Requirements:
- Reference these in Visual Studio project `(in references/)`
	- ScriptHookVDotNet.dll (NOTE: Do not use ScriptHookVDotNet2 or ScriptHookVDotNet3 even though they are the latest)
	- NativeUI.dll
- Ensure the GTA V folder also has the two files above in it (NativeUI.dll must go in the /scripts folder)

### Build/Deploy
- CTRL + SHIFT + B to build .cs file into .dll, this will also automatically deploy to my 'GTAV folder/scripts'
