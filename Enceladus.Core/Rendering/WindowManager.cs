using Enceladus.Core.Config;
using Raylib_cs;

namespace Enceladus.Core.Rendering
{
    public interface IWindowManager
    {
        void CreateWindow();
        int Width { get; }
        int Height { get; }
        bool IsResized { get; }
        void SetTitle(string title);
        int GetFps();
    }
    public class WindowManager : IWindowManager
    {
        //todo: (stretch goal) also i know we are currently putting the fps in the title. i think having a debug overlay that shows the fps (like how minecraft has the f3 overlay) would be cool. thats a strentch goal
        const string title = "Enceladus";

        private readonly IConfigService _configService;

        public WindowManager(IConfigService configService)
        {
            _configService = configService;
        }

        public int Width => Raylib.GetScreenWidth();
        public int Height => Raylib.GetScreenHeight();
        public bool IsResized => Raylib.IsWindowResized();

        public void CreateWindow()
        {
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.InitWindow(
                _configService.Config.Display.DefaultWindowWidth,
                _configService.Config.Display.DefaultWindowHeight,
                title);
            Raylib.SetTargetFPS(_configService.Config.Display.TargetFps);
        }

        public void SetTitle(string title)
        {
            Raylib.SetWindowTitle(title);
        }

        public int GetFps() => Raylib.GetFPS();
    }
}
