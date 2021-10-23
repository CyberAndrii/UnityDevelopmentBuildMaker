using CommandLine;

namespace UnityDevelopmentBuildMaker;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var result = Parser.Default.ParseArguments<MakerConfig>(args);

        await result.WithParsedAsync(async config =>
        {
            try
            {
                var maker = new Maker(config);
                await maker.RunAsync();

                Console.WriteLine("Done!");
            }
            catch (MakerConfigValidationFailedException ex)
            {
                Console.Error.WriteLine(ex.Message);
                Environment.Exit(2);
            }
        });

        result.WithNotParsed(errors => Environment.Exit(1));
    }
}
