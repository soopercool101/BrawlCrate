namespace BrawlLib.Internal.Audio
{
    public interface IAudioSource
    {
        IAudioStream[] CreateStreams();

        bool IsLooped { get; }
    }
}