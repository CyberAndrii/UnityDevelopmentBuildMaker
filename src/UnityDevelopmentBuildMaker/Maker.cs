using System.Diagnostics;

namespace UnityDevelopmentBuildMaker;

public class Maker
{
    private const string UnityPlayerDllFileName = "UnityPlayer.dll";
    private const string UnityExeFileName = "Unity.exe";
    private const string BootConfigFileName = "boot.config";

    private readonly MakerConfig _config;

    private static readonly (string Name, bool IsRequired)[] _filesToReplace = new[]
    {
        ( "UnityPlayer.dll", true ),
        ( "WinPixEventRuntime.dll", false ),
    };

    private static readonly string[] _directoriesToReplace = new[]
    {
        "MonoBleedingEdge",
    };

    public Maker(MakerConfig config)
    {
        _config = config;
    }

    private string backupDirectory => Path.Combine(_config.GamePath.FullName, "UnityDevelopmentBuildMaker Backup");

    public async Task RunAsync()
    {
        Validate();
        CreateBackupDirectory();
        Copy();
        await ConfigureBootConfigAsync();
    }

    private void Validate()
    {
        static void Throw(string message) => throw new MakerConfigValidationFailedException(message);
        static string? GetFileVersion(params string[] paths) => FileVersionInfo.GetVersionInfo(Path.Combine(paths)).FileVersion;

        _config.GamePath.Refresh();
        _config.EditorPath.Refresh();

        if (!_config.GamePath.Exists)
        {
            Throw("Game directory doesn't exists.");
        }

        if (!_config.GamePath.ContainsFile(UnityPlayerDllFileName))
        {
            Throw("Invalid game directory, game is not Mono-based, or game editor version is not supported.");
        }

        if (!_config.EditorPath.Exists)
        {
            Throw("Editor directory doesn't exists.");
        }

        if (!_config.EditorPath.ContainsFile(UnityExeFileName))
        {
            Throw("Invalid editor directory or unsupported game editor version.");
        }

        var gameEditorVersion = GetFileVersion(_config.GamePath.FullName, UnityPlayerDllFileName);
        var editorVersion = GetFileVersion(_config.EditorPath.FullName, UnityExeFileName);

        if (gameEditorVersion != editorVersion)
        {
            Throw($"Game is using a different editor version ({gameEditorVersion}) than provided ({editorVersion}).");
        }

        if (!Directory.Exists(GetVariationPath()))
        {
            Throw("Unity Editor build support for your platform is not installed.");
        }
    }

    private void Copy()
    {
        var fromDirectory = GetVariationPath();
        var toDirectory = _config.GamePath.FullName;

        foreach (var file in _filesToReplace)
        {
            var fromPath = Path.Combine(fromDirectory, file.Name);
            var toPath = Path.Combine(toDirectory, file.Name);

            if (!file.IsRequired && !File.Exists(fromPath))
            {
                continue;
            }

            if (File.Exists(toPath))
            {
                File.Move(toPath, Path.Combine(backupDirectory, file.Name));
            }

            File.Copy(fromPath, toPath);
        }

        foreach (var directory in _directoriesToReplace)
        {
            var fromPath = Path.Combine(fromDirectory, directory);
            var toPath = Path.Combine(toDirectory, directory);

            if (Directory.Exists(toPath))
            {
                Directory.Move(toPath, Path.Combine(backupDirectory, directory));
            }

            DirectoryUtils.CopyFilesRecursively(fromPath, toPath);
        }
    }

    private string GetVariationPath()
    {
        // todo: linux/macos support

        var variationName = _config.Is64Bit ? "win64_development_mono" : "win32_development_mono";

        return Path.Combine(_config.EditorPath.FullName, @"Data\PlaybackEngines\windowsstandalonesupport\Variations", variationName);
    }

    private async Task ConfigureBootConfigAsync()
    {
        var bootConfigPath = _config.GamePath
            .EnumerateDirectories("*_Data")
            .Where(dir => dir.ContainsFile(BootConfigFileName))
            .Select(dir => Path.Combine(dir.FullName, BootConfigFileName))
            .FirstOrDefault();

        if (bootConfigPath == null)
        {
            Console.Error.WriteLine($"Failed to configure {BootConfigFileName}: Not found");
            return;
        }

        var dataDirectoryName = new FileInfo(bootConfigPath).Directory!.Name;
        var backupDataDirectory = Path.Combine(backupDirectory, dataDirectoryName);

        if (!Directory.Exists(backupDataDirectory))
        {
            Directory.CreateDirectory(backupDataDirectory);
        }

        File.Copy(bootConfigPath, Path.Combine(backupDataDirectory, BootConfigFileName));

        var keyValueFile = new KeyValueFile(bootConfigPath, '=');
        await keyValueFile.LoadAsync();

        keyValueFile["wait-for-managed-debugger"] = "1";
        keyValueFile["player-connection-debug"] = "1";

        await keyValueFile.SaveAsync();
    }

    private string CreateBackupDirectory()
    {
        if (Directory.Exists(backupDirectory))
        {
            Directory.Delete(backupDirectory, true);
        }

        Directory.CreateDirectory(backupDirectory);

        return backupDirectory;
    }
}
