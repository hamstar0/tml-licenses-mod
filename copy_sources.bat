rmdir /S /Q "C:\Users\Spinach\Documents\My Games\Terraria\ModLoader\Mod Sources\Licenses\"
xcopy . "C:\Users\Spinach\Documents\My Games\Terraria\ModLoader\Mod Sources\Licenses\" /S /EXCLUDE:.gitignore
del "C:\Users\Spinach\Documents\My Games\Terraria\ModLoader\Mod Sources\Licenses\*.csproj"
del "C:\Users\Spinach\Documents\My Games\Terraria\ModLoader\Mod Sources\Licenses\*.user"
del "C:\Users\Spinach\Documents\My Games\Terraria\ModLoader\Mod Sources\Licenses\*.sln"