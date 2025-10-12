using Raylib_cs;

namespace Enceladus.Core.Services
{
    public interface IWindowManager
    {
        void CreateWindow();
    }
    public class WindowManager : IWindowManager
    {
        const int screenWidth = 1920;
        const int screenHeight = 1080;
        const string title = "Enceladus";
        const int targetFps = 60;

        public void CreateWindow()
        {
            Raylib.InitWindow(screenWidth, screenHeight, title);
            Raylib.SetTargetFPS(targetFps);
        }
    }
}
