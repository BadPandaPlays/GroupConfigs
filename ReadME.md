This is my very first operational plugin, and very first time working in C#.  

This plugin allows creating Group Config files.  The plugin only creates groups, and is incomplete.
This plugin requires TShock for Terraria.
https://github.com/Pryaxis/TShock

To install, simply download and place GroupConfig.dll in your $/Terraria/ServerPlugins folder.
Run your server once, and it will create a default config "Groups.txt" in /Terraria/tshock/Groups/ folder.
To add additional Groups simply copy and paste the Groups file.  The group name will be the filename without the .txt extention.
The first line of the file will be the color in byte notation (255,255,255)
the second line will be the parent.  The group must have a parent currently. (bug)
each additional line you can fill with your TShock permissions.  Do not leave any blank spaces.