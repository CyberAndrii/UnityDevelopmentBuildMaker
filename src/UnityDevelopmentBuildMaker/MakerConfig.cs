using CommandLine;

namespace UnityDevelopmentBuildMaker;

public class MakerConfig
{
    private DirectoryInfo _editorPath = null!;

    [Option("game-path",
        Required = true,
        HelpText = "Root directory of the game (where the game's executable is located). ")]
    public DirectoryInfo GamePath { get; set; } = null!;

    [Option("is-x86",
        Required = false,
        Default = false,
        HelpText = "Is the game's arch x64 or x86 (x64 if not specified). The game may crash if you specify this wrong.")]
    public bool IsArchx86 { get; set; }

    public bool Is64Bit
    {
        get => !IsArchx86;
        set => IsArchx86 = !value;
    }

    [Option("editor-path",
        Required = true,
        HelpText = "Root directory of the editor (where `Unity.exe` is located). Must be the same version the game is using.")]
    public DirectoryInfo EditorPath
    {
        get => _editorPath;
        set
        {
            var editorDirectory = value.EnumerateDirectories().FirstOrDefault(dir => dir.Name == "Editor");
            _editorPath = editorDirectory ?? value;
        }
    }
}
