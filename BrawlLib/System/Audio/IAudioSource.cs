namespace System.Audio
{
    public interface IAudioSource
    {
        IAudioStream[] CreateStreams();
    }
}
