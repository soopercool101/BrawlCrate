using BrawlLib.Internal.Audio;
using System.Drawing;

namespace BrawlLib.Imaging
{
    public interface IImageSource
    {
        int ImageCount { get; }
        Bitmap GetImage(int index);
    }

    public interface IVideo : IImageSource
    {
        bool Loop { get; }
        uint NumFrames { get; }
        float FrameRate { get; }
        int GetImageIndexAtFrame(int frame);

        IAudioStream Audio { get; }
        uint Frequency { get; }
    }
}