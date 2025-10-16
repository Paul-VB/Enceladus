using Enceladus.Core.Config;
using Enceladus.Core.Rendering;
using Enceladus.Core.Tests.Helpers;
using Moq;
using System.Numerics;

namespace Enceladus.Core.Tests.Rendering
{
    public class CameraManagerTestFixture
    {
        private readonly Mock<IWindowManager> _windowManager;
        private readonly Mock<IConfigService> _configService;
        private readonly ICameraManager _cameraManager;

        public CameraManagerTestFixture()
        {
            _windowManager = new Mock<IWindowManager>();
            _configService = new Mock<IConfigService>();

            // Setup default config
            var config = new Config.Config
            {
                Display = new DisplayConfig
                {
                    CameraZoom = 16f,
                    DefaultWindowWidth = 1920,
                    DefaultWindowHeight = 1080
                }
            };
            _configService.Setup(c => c.Config).Returns(config);

            // Setup default window size
            _windowManager.Setup(w => w.Width).Returns(1920);
            _windowManager.Setup(w => w.Height).Returns(1080);
            _windowManager.Setup(w => w.IsResized).Returns(false);

            _cameraManager = new CameraManager(_windowManager.Object, _configService.Object);
        }

        [Fact]
        public void InitCamera_SetsZoomFromConfig()
        {
            // Act
            _cameraManager.InitCamera();

            // Assert
            Assert.Equal(16f, _cameraManager.Camera.Zoom);
        }

        [Fact]
        public void InitCamera_CentersCamera()
        {
            // Act
            _cameraManager.InitCamera();

            // Assert - Offset should be half of window dimensions
            Assert.Equal(960f, _cameraManager.Camera.Offset.X); // 1920 / 2
            Assert.Equal(540f, _cameraManager.Camera.Offset.Y); // 1080 / 2
        }

        [Fact]
        public void TrackEntity_UpdateFollowsEntityPosition()
        {
            // Arrange
            var entity = EntityHelpers.CreateTestEntity(new Vector2(100, 200));
            _cameraManager.TrackEntity(entity);

            // Act
            _cameraManager.Update();

            // Assert
            Assert.Equal(100f, _cameraManager.Camera.Target.X);
            Assert.Equal(200f, _cameraManager.Camera.Target.Y);
        }

        [Fact]
        public void TrackEntity_UpdateFollowsEntityMovement()
        {
            // Arrange
            var entity = EntityHelpers.CreateTestEntity(new Vector2(100, 200));
            _cameraManager.TrackEntity(entity);
            _cameraManager.Update();

            // Move entity
            entity.Position = new Vector2(150, 250);

            // Act
            _cameraManager.Update();

            // Assert
            Assert.Equal(150f, _cameraManager.Camera.Target.X);
            Assert.Equal(250f, _cameraManager.Camera.Target.Y);
        }

        [Fact]
        public void StopTracking_StopsFollowingEntity()
        {
            // Arrange
            var entity = EntityHelpers.CreateTestEntity(new Vector2(100, 200));
            _cameraManager.TrackEntity(entity);
            _cameraManager.Update();

            // Act
            _cameraManager.StopTracking();
            entity.Position = new Vector2(500, 500);
            _cameraManager.Update();

            // Assert - Camera should still be at old position
            Assert.Equal(100f, _cameraManager.Camera.Target.X);
            Assert.Equal(200f, _cameraManager.Camera.Target.Y);
        }

        [Fact]
        public void SetTarget_StopsTrackingAndSetsPosition()
        {
            // Arrange
            var entity = EntityHelpers.CreateTestEntity(new Vector2(100, 200));
            _cameraManager.TrackEntity(entity);
            _cameraManager.Update();

            // Act
            _cameraManager.SetTarget(new Vector2(300, 400));
            entity.Position = new Vector2(999, 999);
            _cameraManager.Update();

            // Assert - Camera at set position, not following entity
            Assert.Equal(300f, _cameraManager.Camera.Target.X);
            Assert.Equal(400f, _cameraManager.Camera.Target.Y);
        }

        [Fact]
        public void Update_WindowResized_RecentersCamera()
        {
            // Arrange
            _windowManager.Setup(w => w.IsResized).Returns(true);
            _windowManager.Setup(w => w.Width).Returns(800);
            _windowManager.Setup(w => w.Height).Returns(600);

            // Act
            _cameraManager.Update();

            // Assert - Offset should be recalculated
            Assert.Equal(400f, _cameraManager.Camera.Offset.X); // 800 / 2
            Assert.Equal(300f, _cameraManager.Camera.Offset.Y); // 600 / 2
        }

        [Fact]
        public void GetVisibleChunks_ReturnsChunksInView()
        {
            // Arrange
            var map = MapHelpers.CreateMapWithChunks((0, 0), (1, 0), (0, 1), (1, 1), (5, 5));
            _cameraManager.SetTarget(new Vector2(16, 16)); // Center of 2x2 chunk grid

            // Act
            var result = _cameraManager.GetVisibleChunks(map);

            // Assert - Should see 4 center chunks, not the far away one
            Assert.Equal(4, result.Count);
            Assert.Contains(result, c => c.X == 0 && c.Y == 0);
            Assert.Contains(result, c => c.X == 1 && c.Y == 0);
            Assert.Contains(result, c => c.X == 0 && c.Y == 1);
            Assert.Contains(result, c => c.X == 1 && c.Y == 1);
            Assert.DoesNotContain(result, c => c.X == 5 && c.Y == 5);
        }

        [Fact]
        public void GetVisibleChunks_CameraAtOrigin_ReturnsNearbyChunks()
        {
            // Arrange
            var map = MapHelpers.CreateMapWithChunks((0, 0), (-1, -1), (-1, 0), (0, -1));
            _cameraManager.SetTarget(Vector2.Zero);

            // Act
            var result = _cameraManager.GetVisibleChunks(map);

            // Assert - Should see chunks around origin
            Assert.True(result.Count > 0);
            Assert.Contains(result, c => c.X == 0 && c.Y == 0);
        }

        [Fact]
        public void GetVisibleChunks_NegativeCoordinates_WorksCorrectly()
        {
            // Arrange
            var map = MapHelpers.CreateMapWithChunks((-2, -2), (-1, -1), (0, 0), (5, 5));
            _cameraManager.SetTarget(new Vector2(-24, -24)); // Center of negative chunks

            // Act
            var result = _cameraManager.GetVisibleChunks(map);

            // Assert - Should see negative chunks, not positive ones far away
            Assert.DoesNotContain(result, c => c.X == 5 && c.Y == 5);
            Assert.Contains(result, c => c.X == -2 && c.Y == -2);
        }
    }
}
