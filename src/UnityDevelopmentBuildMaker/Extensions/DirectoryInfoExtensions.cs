namespace UnityDevelopmentBuildMaker.Extensions;

internal static class DirectoryInfoExtensions
{
    public static FileInfo GetFile(this DirectoryInfo directory, string name)
    {
        var path = Path.Combine(directory.FullName, name);

        if (!directory.ContainsFile(name))
        {
            throw new FileNotFoundException($"File \"{path}\" was not found.");
        }

        return new FileInfo(path);
    }

    public static bool ContainsFile(this DirectoryInfo directory, string name)
        => File.Exists(Path.Combine(directory.FullName, name));

    public static bool ContainsDirectory(this DirectoryInfo directory, string name)
        => Directory.Exists(Path.Combine(directory.FullName, name));
}
