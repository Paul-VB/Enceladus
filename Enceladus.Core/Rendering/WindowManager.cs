using Raylib_cs;

namespace Enceladus.Core.Rendering
{
    public interface IWindowManager
    {
        void CreateWindow(int? width = null, int? height = null);
        int Width { get; }
        int Height { get; }
        bool IsResized { get; }
        void SetTitle(string title);
        int GetFps();
    }
    public class WindowManager : IWindowManager
    {
        //todo. make default window dims be configurable, and make target fps also configurable.
        //todo: (stretch goal) also i know we are currently putting the fps in the title. i think having a debug overlay that shows the fps (like how minecraft has the f3 overlay) would be cool. thats a strentch goal
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

        public void SetTitle(string title)
        {
            Raylib.SetWindowTitle(title);
        }

        public int GetFps() => Raylib.GetFPS();
    }
}
