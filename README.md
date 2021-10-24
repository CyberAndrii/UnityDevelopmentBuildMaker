# UnityDevelopmentBuildMaker

A tool to quickly convert Unity release build into development build.

If you want to attach a debugger, you should use Unity's `Attach Unity debugger` instead of `Attach to Process` in Visual Studio.

> *Note: At the moment this tool only works on Windows. MacOS and Linux support will be added later.*

> *Note: Only Unity 2017.2 or newer is supported.*

## Usage

### Example

```cmd
./UnityDevelopmentBuildMaker.exe --game-path "D:\SteamLibrary\steamapps\common\Unturned" --editor-path "D:\UnityEditors\2019.4.30f1\Editor"
```

### Supported Command Line Options

| Name        | Required           | Description                                                                                              |
| ----------- | ------------------ | -------------------------------------------------------------------------------------------------------- |
| game-path   | :heavy_check_mark: | Root directory of the game (where the game's executable is located).                                     |
| is-x86      | :x:                | Is the game's arch x64 or x86 (x64 if not specified). The game may crash if you specify this wrong.      |
| editor-path | :heavy_check_mark: | Root directory of the editor (where `Unity.exe` is located). Must be the same version the game is using. |
| help        | :x:                | Display supported arguments.                                                                             |
