namespace System.Audio
{
    public interface IAudioSource
    {
        IAudioStream[] CreateStreams();

        bool IsLooped { get; }
    }
}