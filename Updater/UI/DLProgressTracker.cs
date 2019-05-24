namespace System.Windows.Forms
{
    public interface DLProgressTracker
    {
        void Update(float value);
        void Begin(float min, float max, float current);
        void Finish();
        void Cancel();
        
        bool Cancelled { get; set; }
    }
}
