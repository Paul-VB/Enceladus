# Enceladus TODO List

## High Priority

### Rendering
- [ ] **Implement render layers/z-index for proper sprite ordering**
  - Currently relies on entity registration order (fragile and implicit)
  - Weapons render in front of player only because they're registered after player
  - Need explicit render layer system: RenderLayer property, ZIndex, or separate entity lists
  - This is a critical architectural issue that will cause bugs as entities are added

### Collision System
- [x] **Implement polygon decomposition algorithm** (concave → convex)
  - Location: `EarClippingTriangulationSlicer.cs`, `ConcavePolygonHitbox.cs`
  - Implemented ear clipping triangulation (placeholder algorithm)
  - Todo: Replace with more performant algorithm later
  - SAT updated with CheckCollision2 to support concave polygons

- [x] **Implement circle collision detection**
  - Location: `CircleCollisionDetector.cs`, `CollisionChecker.cs`
  - Implemented circle-to-circle, circle-to-polygon, circle-to-cell collisions
  - Uses distance check for circles, closest point algorithm for polygons
  - Includes reversed cases (polygon-to-circle) with proper normal reversal

- [x] **Test with Player submarine hitbox**
  - Player is using PolygonHitbox built from pixel coordinates
  - Collision working correctly with submarine shape

- [ ] **Per-material restitution (bounciness)**
  - Add Restitution property to ICollidable or MovableEntity
  - Combine restitution values for collisions (geometric mean recommended)
  - Allow different materials (rubber, wood, metal) to have different bounce behavior
  - Update CollisionResolver to use combined restitution instead of global config

### Architecture Consistency
- [x] **Consider injecting IEntityRegistry into CollisionChecker**
  - Location: `CollisionService.cs`, `CollisionChecker.cs`
  - Injected both IEntityRegistry and IWorldService into CollisionChecker
  - Methods now parameterless - dependencies injected via constructor
  - CollisionService simplified - no longer passes dependencies as parameters

## Medium Priority

### Entity System
- [x] **Entity auto-registration or factory pattern**
  - Location: `EntityFactory.cs`
  - Implemented IEntityFactory and EntityFactory
  - All entities created through factory with automatic registration
  - Factory handles hitbox building and positioning

- [ ] **Refactor Cell to use Vector2 Position instead of X, Y**
  - Location: `Cell.cs:13`
  - Would unify coordinate handling across entities and cells
  - Update all usages (MapRenderer, ChunkMath, VertexExtractor)

### Player & Controls
- [x] **Implement sprite orientation hysteresis for Player**
  - Location: `Player.cs:90-108`
  - Uses 80°/280° and 100°/260° thresholds to prevent flip thrashing
  - Swaps between PlayerSubLeft and PlayerSubRight based on rotation
  - Implemented with _isFacingRight state to track current orientation
  - Prevents jarring back-and-forth flipping when rotating near threshold angles

- [ ] **Extract sprite orientation logic for reuse**
  - Location: `Player.cs:98`
  - The rotation-to-sprite-flip logic might be useful for other entities
  - Consider extracting to helper method or component

- [ ] **Replace magic numbers in sprite orientation**
  - Location: `Player.cs:103, 111`
  - Currently using hardcoded angles: 80°, 100°, 260°, 280°
  - Should be named constants for clarity

### Weapons & Combat
- [ ] **Implement weapon aiming strategies (Fixed, TrackTarget)**
  - Allow weapons to have different targeting behaviors
  - Fixed: weapon always points in same direction relative to owner
  - TrackTarget: weapon rotates to track nearest enemy/target

- [ ] **Implement mining beam weapon**
  - Mouse-aimable beam that can melt through ice and damage cells
  - Ray from player submarine to mouse cursor
  - Deal damage to cells over time (reduce health until destroyed)
  - Visual beam rendering
  - Consider heat/energy cost mechanics

- [ ] **Make bullet drag configurable in config file**
  - Location: `Bullet.cs:16`
  - Currently hardcoded to 0.1f
  - Should be configurable per projectile type

- [ ] **Add weapon.bulletSpawnOffset property**
  - Location: `ProjectileFactory.cs:52`
  - Currently projectiles spawn at weapon.Position
  - Need offset to spawn bullets from gun barrel instead of gun center

- [ ] **Decide if FireRate should be RPM or rounds/second**
  - Location: `Weapon.cs:9`
  - Currently using rounds/second
  - Consider if RPM would be more intuitive

- [ ] **Create proper weapon sprite (currently using default)**
  - TestGun and FastTestGun using placeholder sprites
  - Need actual gun sprite assets

- [ ] **Create proper bullet sprite (currently 8x8 test chunk)**
  - Current bullet sprite is test chunk texture
  - Need actual projectile sprite assets

### Collision Layer System
- [ ] **Implement collision layers/masks**
  - Needed for platforms (collide with player, not with cells/other platforms)
  - Add to ICollidable as simple properties (no component needed)
  - Update collision detection to check layer compatibility

### Component Architecture (Future)
- [ ] **Add collision properties to ICollidable when needed**
  - Location: `Entity.cs:8`
  - Restitution, CollisionLayer, CollisionMask, etc.
  - These are just properties - NO CollisionComponent needed

- [ ] **Add DamageComponent if complex damage logic needed**
  - For health, armor calculations, damage over time, etc.

- [ ] **Add InventoryComponent if complex inventory needed**
  - For item management, weight calculations, etc.

## Low Priority / Stretch Goals

### Testing
- [ ] **Refactor ChunkMathTests to separate test fixtures**
  - Location: `ChunkMathTests.cs:7`
  - One fixture per function being tested
  - Current tests work but could be better organized

- [ ] **Create test fixture for TestHelpers**
  - Location: `TestHelpers.cs:33`
  - Test the test helpers themselves

- [ ] **Verify AxesExtractor perpendicular axis logic**
  - Location: `AxesExtractor.cs:44`
  - Ensure we get perpendicular between final vertex and first vertex
  - Add test to verify correctness

- [ ] **Make PolygonHitboxBuilder testable**
  - Location: `PolygonHitboxBuilder.cs:9`
  - Extract to interface for easier testing

### World System
- [ ] **Decide on MapChunk struct vs class**
  - Location: `MapChunk.cs:3`
  - Research performance benefits of structs vs classes for this use case

- [ ] **Remove water cells or treat as empty**
  - Location: `CellTypes.cs:7`, `MapRenderer.cs:70`
  - Currently skipping water in rendering - maybe remove entirely?
  - Treat cell ID 0 as nothing (like Minecraft)

- [ ] **World saving and loading**
  - Location: `Map.cs:10`
  - Need entity persistence on maps
  - Handle player moving between sectors

- [ ] **Add map metadata**
  - Location: `Map.cs:12`
  - Biome type, resources, enemy spawns, instability level

- [ ] **Procedural world generation**
  - Location: `MapGenerator.cs:8`
  - Seed-based proc gen (learn how it works!)
  - Current random gen is just for testing

- [ ] **WorldService features**
  - Location: `WorldService.cs:5`
  - Orbital trader timing, sector events, etc.

### Rendering
- [x] **Sprite atlas system**
  - Location: `SpriteDefinitions.cs`, `SpriteService.cs`, `EntityRenderer.cs`, `MapRenderer.cs`
  - Implemented atlas-based rendering for both entities and cells
  - Organized into `SpriteDefinitions.Entities` and `SpriteDefinitions.Cells` nested classes
  - **Performance win: 115fps (with GC stuttering) → 1500+ fps** 🚀
  - Uses `ISpriteRendered` interface for sprite-based entities
  - Supports `SpriteModifiers` for tint, alpha, flip, and blend modes

- [ ] **Debug overlay (F3-style)**
  - Location: `WindowManager.cs:17`
  - Show FPS, position, velocity, collision info
  - Like Minecraft F3 overlay

### Performance
- [ ] **GPU-accelerated collision detection**
  - Location: `CollisionChecker.cs:28`
  - Extreme stretch goal - use GPU for broad phase?

### Code Documentation
- [ ] **Explain SAT normal direction logic**
  - Location: `SatCollisionDetector.cs:127, 128`
  - Why normal must point from OtherObject to Entity
  - Why we use center of vertices for direction check

- [x] **Consider extracting vertex rotation logic**
  - Location: `GeometryHelper.cs:12`
  - Extracted to GeometryHelper.TransformToWorldSpace()
  - Todo: Decide if it should be testable interface/service or stay as static utility
  - Used by AwfulGreenStar and other monsters

- [ ] **Combine AABB logic for convex and concave polygons**
  - Location: `AabbCalculator.cs:101`
  - Logic looks identical between CalculateAabbFromPolygon and CalculateAabbFromConcavePolygon
  - Consider if ConcavePolygonHitbox should inherit from PolygonHitbox
  - Wait until SAT implementation is stable before refactoring

- [ ] **XmlHelper API design discussion**
  - Location: `XmlHelper.cs:71`
  - Should it modify first xmlDoc and expect caller to clone?
  - Or keep current immutable approach?

### Optimization Ideas
- [x] **ChunkSize and PixelsPerWorldUnit constants**
  - Location: `Constants.cs`
  - Moved to centralized Constants.cs file
  - ChunkSize = 16, PixelsPerWorldUnit = 16f
  - These are fundamental game constants, no benefit to making them configurable

- [ ] **PolygonHitboxBuilder API**
  - Location: `PolygonHitboxBuilder.cs:30`
  - Add BuildFromVertices for already-world-scale vectors?
  - Or is PolygonHitbox constructor enough?

## Completed

### Recent (Latest Session - Weapon System)
- [x] Weapon system implementation (Weapon, TestGun, FastTestGun)
- [x] Projectile system with abstract Projectile base class (eliminates casting)
- [x] Generic CreateWeapon<T>() factory method for easy weapon creation
- [x] IFF (Identify Friend or Foe) system for collision filtering
- [x] WeaponControlService for mouse-based weapon aiming
- [x] TimeService for game time tracking
- [x] Dual weapon mounts on Player submarine
- [x] Constants.cs for centralized game constants (PixelsPerWorldUnit, ChunkSize)
- [x] Fixed sprite scaling to use PixelsPerWorldUnit instead of camera zoom
- [x] Deleted IProjectile interface (Projectile base class is superior)
- [x] Extracted IFF check in CollisionChecker to ShouldSkipCollisionDueToIff()
- [x] Fixed weapon render order (register player before weapons)

### Previous Session (Atlas Refactor)
- [x] Sprite atlas system implementation (115fps → 1500+ fps!)
- [x] ISpriteRendered and IGeometryRendered interfaces for flexible rendering
- [x] SpriteModifiers system (tint, alpha, flip, blend modes)
- [x] Organized sprite definitions into nested classes (Entities/Cells)
- [x] MapChunk changed to sparse List<Cell> storage (no null cells)
- [x] Removed CellTypes.Water (null = empty space, like Minecraft)
- [x] EntityRegistry pre-filtered lists for SpriteRendered/GeometryRendered entities
- [x] Injected IEntityRegistry and IWorldService into CollisionChecker

### Previous Session
- [x] Remove MovableComponent (was premature abstraction)
- [x] Refactor entity constructors - remove DI dependencies
- [x] Entity factory pattern with ApplyDefaults/RegisterEntity helpers
- [x] IControllable interface + InputService + ControllableRegistry
- [x] Rename IInputManager → IInputReader
- [x] Entity-to-cell bouncy collisions
- [x] CollisionResolver unit tests (mass distribution, momentum conservation)
- [x] Refactor CollisionResolver - extract helper methods (ShouldSkipCollision, CalculateImpulse)

### Previous Sessions
- [x] Consolidate TestEntity creation to use EntityHelpers.CreateTestEntity
- [x] Fix collision bug - Cell-specific coordinate handling in VertexExtractor
- [x] Add unit tests for VertexExtractor (prevent coordinate regression)
- [x] Component-based architecture with MovableComponent delegation (later removed)
- [x] Rename IMoveableEntity → IMovable
- [x] Remove redundant interfaces (IEntity, ICollidableEntity, IPlayer)
- [x] EntityRegistry pre-filtered lists optimization
- [x] CollisionService simplification (inject IEntityRegistry)
