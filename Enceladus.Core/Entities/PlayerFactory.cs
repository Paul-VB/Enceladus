using Enceladus.Core.Config;
using Enceladus.Core.Physics.Hitboxes.Helpers;
using Enceladus.Core.Rendering;
using System.Numerics;

namespace Enceladus.Core.Entities
{
    public interface IPlayerFactory
    {
        Player CreatePlayer(Vector2 position);
    }

    public class PlayerFactory : IPlayerFactory
    {
        private readonly IConfigService _configService;
        private readonly IPolygonHitboxBuilder _polygonHitboxBuilder;

        public PlayerFactory(IConfigService configService,IPolygonHitboxBuilder polygonHitboxBuilder)
        {
            _configService = configService;
            _polygonHitboxBuilder = polygonHitboxBuilder;
        }

        public Player CreatePlayer(Vector2 position)
        {
            var player = new Player();

            // Player physics config
            var config = _configService.Config.Player;
            player.Mass = config.Mass;
            player.Drag = config.Drag;
            player.AngularDrag = config.AngularDrag;

            player.Hitbox = _polygonHitboxBuilder.BuildFromPixelCoordinates(
                (int)SpriteDefinitions.Entities.PlayerSubRight.SourceRegion.Width,
                (int)SpriteDefinitions.Entities.PlayerSubRight.SourceRegion.Height,
                Player.PixelVertices);
            player.Position = position;
            return player;
        }
    }
}
