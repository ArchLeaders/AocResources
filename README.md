# AocResources

This repository contains a set of tools and resources to make extracting assets (models and sounds) from Age of Calamity much easier.

### Contents:

- **[Models/Textures Extracting Tutorial](#modelstextures-extracting-tutorial)**
- **[Sound/Music Extracting Tutorial](#soundmusic-extracting-tutorial)**
- **[Credits and Thanks](#credits-and-thanks)**

---

## Models/Textures Extracting Tutorial

The quick guide will take you through the proccess of extracting models and textures from your Age of Calamity game dump.

### Prerequisites

- Age of Calamity dumped to RomFS
- Age of Calamity DLC dumped to RomFS *(optional)*

> *[Video on extracting NSP/XCI to RomFS](https://www.youtube.com/watch?v=oUq0zecwHjY)*

### Extract (without DLC)

- Download the latest AocModelExtractor [release](./release).
- Place the `.exe` in a folder next to your game's `romfs` folder.

  <img width="400" src="https://user-images.githubusercontent.com/80713508/197467463-18328c1b-d713-473f-889e-c361aab73783.png">
- Open the command line in the folder by typing `cmd` into the path bar.
- From there, type `AocModelExtractor.exe .\romfs` and press enter.
- Wait for the process to finish. It can take up to an hour.

### Extract (with DLC)

- Download the latest AocModelExtractor [release](./release).
- Place the `.exe` in a folder next to your game dump.

  <img width="400" src="https://user-images.githubusercontent.com/80713508/197467307-57bd2b3f-811f-48a1-a91a-7fbb57955523.png">
- Open the command line in the folder by typing `cmd` into the path bar.
- From there, type `AocModelExtractor.exe .\` and press enter.
- Wait for the process to finish. It can take up to an hour.
- Finally, you are given the choice to install `Noesis`, this will be required to **export** and **view** the AoC models.

### After Extracting

Once you have extracted the files with the previous step, you will be left with a few new folders. Here's what they all are:

- `Cethleann` | This folder contains the Cethleann toolset which is used to extract the game files. You can delete it, or save it for future use if you desire.
- `Noesis` *\*<u>(only if you allowed the tool to install it at the end)*</u> | This folder contains Noesis, a model viewing and extracting program that can open **g1m** and **g1t** files (models and textures). This tool is required for the next step.
- `romfs` *\*<u>(only if you use the DLC)</u>* | This contains the base game merged with the DLCs found in that folder. This can also be safely deleted unless you have furthur use for it.
- `extracted-rdb` | This folder contains the extracted assets found in AoC. Do not delete this folder.

### Export as FBX

- Open `Noesis64.exe` from the `Noesis` folder, or your own installed location.
- Go to the `Tools` menu, and confirm `Display Plugin Tools` is checked.
- Next go to `Tools > Project G1M` and make sure the settings there match these:

  <img width="300" src="https://user-images.githubusercontent.com/80713508/197473185-319822c0-1ceb-41ba-8136-bd90c7a0d5c9.png">
- Once you have confirmed that, go to `File > Open File` and browse for the `extracted-rdb\merged` folder, open any one of the subfolders and select a `.g1m` file to open. Once you have done that, you can use the built-in browser to open more `.g1m` files easier.
- Find a model you want to export (you can use [this sheet](./FakeNames.yml) to help you), right click the `.g1m` file in `Noesis` and click `Export`. Leave everythign as default (unless you know what you are doing), and click `Export`.
- Use `Noesis` to open the export folder in File Explorer, and there you have your textures and FBX model.

<br>

> If you encounter any errors or need help, feel free to message me in my Discord: https://discord.gg/8Saj6tTkNB | @ArchLeaders

<br>

## Sound/Music Extracting Tutorial

### Prerequisites

- Age of Calamity dumped to RomFS
- Age of Calamity DLC dumped to RomFS *(optional)*

> *[Video on extracting NSP/XCI to RomFS](https://www.youtube.com/watch?v=oUq0zecwHjY)*

### Extract

- Download the latest AocSoundExtractor [release](./release).
- Place the `.exe` in a folder next to your game's (or DLC's) `romfs` folder.

  <img width="400" src="https://user-images.githubusercontent.com/80713508/197628795-43faa11b-3d30-449c-b43e-b79aa3d3ab30.png">
- Open the command line in the folder by typing `cmd` into the path bar.
- From there, type `AocSoundExtractor.exe .\romfs` and press enter.
- Wait for the process to finish.
- When the process completes, you will be left with a folder of `.ktss` files. These can be opened and exported using [foobar2000](https://www.foobar2000.org/) with the [vgstream](https://vgmstream.org/usage#foo-input-vgmstream-foobar2000-plugin) add-on.

<br>

> If you encounter any errors or need help, feel free to message me in my Discord: https://discord.gg/8Saj6tTkNB | @ArchLeaders

<br>

## Credits and Thanks

Thanks to all the developers and researchers who provided all the tools and info to parse KT's file formats.

### Tools

- **[yretenai](https://github.com/yretenai)** | Cethleann, data extraction and texture pairing.
- **[Joschuka](https://github.com/Joschuka)** | Project G1M, g1m, g1t support for Noesis.
- **[RichWhiteHouse](https://richwhitehouse.com)** | Noesis, model viewing and exporting.

### Resources

- **[Joschuka](https://github.com/Joschuka)** | Model/Texture hash list.
- **Anexenaumoon** | Sound file KTSS offsets.
- **[Moonling](https://github.com/VelouriasMoon)** | Extraction commands and misc info.
- **[banan039](https://github.com/banan039pl)** | Texture matching commands and misc info.
- **mg76** | Sound/music info.

### Credit

- **ArchLeaders** | C# automation software.