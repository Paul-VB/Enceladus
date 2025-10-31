using Enceladus.Core.Entities;
using Enceladus.Core.Entities.Weapons;
using Enceladus.Core.Time;
using Moq;
using System.Numerics;

namespace Enceladus.Core.Tests.Entities.Weapons
{
    public class ProjectileFactoryTestFixture
    {
        private readonly IProjectileFactory _projectileFactory;

        private readonly Mock<IScheduledActionService> _scheduledActionService = new Mock<IScheduledActionService>();
        private readonly Mock<IEntityRegistry> _entityRegistry = new Mock<IEntityRegistry>();


        private TestArmedEntity _testArmedEntity = null!;
        private Weapon _testWeapon = null!;

        public ProjectileFactoryTestFixture()
        {
            _projectileFactory = new ProjectileFactory(_scheduledActionService.Object, _entityRegistry.Object);
        }

        private void GivenTestArmedEntity(List<int> iffCodes)
        {
            _testArmedEntity = new TestArmedEntity
            {
                IffCodes = iffCodes
            };

            _testWeapon = new TestWeapon
            {
                Owner = _testArmedEntity,
                Position = Vector2.Zero,
                Rotation = 0f
            };
        }


        [Fact]
        public void GivenProjectileCreated_WhenOwnerIffCodesChange_ThenProjectileIffCodesUnchanged()
        {
            // Given - Create a mock owner with IFF codes
            GivenTestArmedEntity([1, 2]);

            // When - Create projectile (should snapshot IFF codes)
            var projectile = _projectileFactory.CreateProjectile(_testWeapon);
            var projectileIffCodesInitial = new List<int>(projectile.IffCodes); 

            _testArmedEntity.IffCodes.Add(666);

            Assert.Equal(projectileIffCodesInitial, projectile.IffCodes);
            Assert.DoesNotContain(666, projectile.IffCodes);
            Assert.Contains(666, _testArmedEntity.IffCodes); 
        }
    }
}
