namespace UnityDevelopmentBuildMaker;

public class MakerConfigValidationFailedException : Exception
{
    public MakerConfigValidationFailedException(string? message) : base(message)
    {
    }
}
