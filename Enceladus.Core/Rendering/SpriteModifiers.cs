using Raylib_cs;

namespace Enceladus.Core.Rendering
{
    public class SpriteModifiers
    {
        public Color Tint { get; set; } = Color.White;
        public byte Alpha { get; set; } = 255;
        public BlendMode BlendMode { get; set; } = BlendMode.Alpha;
        public bool FlipHorizontal { get; set; } = false;
        public bool FlipVertical { get; set; } = false;

        // Future expansion:
        // public Shader? CustomShader { get; set; }
        // public bool RenderOutline { get; set; }
        // public Color OutlineColor { get; set; }
    }
}
