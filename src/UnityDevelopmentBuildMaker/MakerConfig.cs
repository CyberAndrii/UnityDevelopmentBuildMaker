using CommandLine;

namespace UnityDevelopmentBuildMaker;

public class MakerConfig
{
    private DirectoryInfo _editorPath = null!;

    [Option("gamePath", Required = true, HelpText = "Root directory of the game.")]
    public DirectoryInfo GamePath { get; set; } = null!;

    [Option("is64Bit", Required = true, HelpText = "Is the game 64 or 32 bit.")]
    public bool Is64Bit { get; set; }

    [Option("editorPath", Required = true, HelpText = "Root directory of the editor.")]
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
