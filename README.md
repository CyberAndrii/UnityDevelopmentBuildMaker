# UnityDevelopmentBuildMaker

A tool to quickly convert Unity release build into development build.

If you want to attach a debugger, you should use Unity's `Attach Unity debugger` instead of `Attach to Process` in Visual Studio.

> *Note: At the moment this tool only works on Windows. MacOS and Linux support will be added later.*

> *Note: Only Unity 2017.2 or newer is supported.*

## Usage

### Example

```cmd
./UnityDevelopmentBuildMaker.exe --gamePath "D:\SteamLibrary\steamapps\common\Unturned" --is64Bit --editorPath "D:\UnityEditors\2019.4.30f1\Editor"
```

### Supported Command Line Options

| Name          | Required           | Description                                                                                              |
| ------------- | ------------------ | -------------------------------------------------------------------------------------------------------- |
| gamePath      | :heavy_check_mark: | Root directory of the game (where the game's executable is located).                                     |
| is64Bit       | :heavy_check_mark: | Is the game 64 or 32 bit. The game may crash if you specify this wrong.                                  |
| editorPath    | :heavy_check_mark: | Root directory of the editor (where `Unity.exe` is located). Must be the same version the game is using. |
| help          | :x:                | Display supported arguments.                                                                             |
