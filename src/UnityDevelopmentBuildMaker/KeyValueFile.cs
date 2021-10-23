namespace UnityDevelopmentBuildMaker;

internal class KeyValueFile : Dictionary<string, string>
{
    private readonly string _path;
    private readonly char _separator;

    public KeyValueFile(string path, char separator)
    {
        _path = path;
        _separator = separator;
    }

    public async Task LoadAsync()
    {
        var lines = await File.ReadAllLinesAsync(_path);

        foreach (var line in lines)
        {
            var parts = line.Split(_separator);

            this[parts[0]] = string.Join(_separator, parts.Skip(1));
        }
    }

    public async Task SaveAsync()
    {
        var lines = new List<string>(Count);

        foreach (var pair in this)
        {
            lines.Add(pair.Key + _separator + pair.Value);
        }

        await File.WriteAllLinesAsync(_path, lines);
    }
}