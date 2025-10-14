using Raylib_cs;

namespace Enceladus.Core.Services
{
    public interface IWindowManager
    {
        void CreateWindow(int? width, int? height);
        int Width { get; }
        int Height { get; }
        bool IsResized { get; }
    }
    public class WindowManager : IWindowManager
    {
        const int defaultWindowWidth = 1920;
        const int defaultWindowHeight = 1080;
        const string title = "Enceladus";
        const int targetFps = 60;

        public int Width => Raylib.GetScreenWidth();
        public int Height => Raylib.GetScreenHeight();
        public bool IsResized => Raylib.IsWindowResized();

        public void CreateWindow(int? width, int? height)
        {
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.InitWindow(width ?? defaultWindowWidth, height ?? defaultWindowHeight, title);
            Raylib.SetTargetFPS(targetFps);
        }
    }
}
