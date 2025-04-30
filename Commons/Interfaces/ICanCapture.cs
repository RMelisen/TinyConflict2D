namespace TinyConflict2D.Commons.Interfaces;

public interface ICanCapture
{
    void CaptureBuilding();
    void CancelCapture();
    int GetCaptureProgress();
}