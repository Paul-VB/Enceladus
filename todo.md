# Enceladus TODO List

## High Priority

### Collision System
- [x] **Implement polygon decomposition algorithm** (concave → convex)
  - Location: `EarClippingTriangulationSlicer.cs`, `ConcavePolygonHitbox.cs`
  - Implemented ear clipping triangulation (placeholder algorithm)
  - Todo: Replace with more performant algorithm later
  - SAT updated with CheckCollision2 to support concave polygons

- [ ] **Implement circle collision detection**
  - Location: `CollisionChecker.cs:50, 111`
  - Need circle-to-anything collision for both entity-to-cell and entity-to-entity
  - Currently using placeholder that returns zero penetration

- [ ] **Test with Player submarine hitbox**
  - Once polygon decomposition is implemented, test with actual submarine shape
  - Verify collision feels correct with concave hull

### Architecture Consistency
- [ ] **Consider injecting IEntityRegistry into CollisionChecker**
  - Location: `CollisionService.cs:33`, `CollisionChecker.cs`
  - Currently CollisionService injects it, CollisionChecker receives as parameter
  - Inconsistent pattern - decide on one approach

## Medium Priority

### Entity System
- [ ] **Review MovableComponent necessity**
  - Location: `MovableComponent.cs:7`
  - Currently only used by MovableEntity - is the extraction worth it?
  - Decision: Keep for future component-based architecture or inline the logic?

- [x] **Entity auto-registration or factory pattern**
  - Location: `EntityFactory.cs`
  - Implemented IEntityFactory and EntityFactory
  - All entities created through factory with automatic registration
  - Factory handles hitbox building and positioning

- [ ] **Refactor Cell to use Vector2 Position instead of X, Y**
  - Location: `Cell.cs:13`
  - Would unify coordinate handling across entities and cells
  - Update all usages (MapRenderer, ChunkMath, VertexExtractor)

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
- [ ] **Sprite atlas system**
  - Location: `SpriteService.cs:11`, `Cell.cs:31`
  - One big texture is more efficient than individual files
  - Would require updating CellType sprite references

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
- [ ] **ChunkSize configuration**
  - Location: `ChunkMath.cs:8`
  - Currently hardcoded - is it worth making configurable?
  - Probably not - no benefit to changing it

- [ ] **PolygonHitboxBuilder API**
  - Location: `PolygonHitboxBuilder.cs:30`
  - Add BuildFromVertices for already-world-scale vectors?
  - Or is PolygonHitbox constructor enough?

## Completed
- [x] Consolidate TestEntity creation to use EntityHelpers.CreateTestEntity
- [x] Fix collision bug - Cell-specific coordinate handling in VertexExtractor
- [x] Add unit tests for VertexExtractor (prevent coordinate regression)
- [x] Component-based architecture with MovableComponent delegation
- [x] Rename IMoveableEntity → IMovable
- [x] Remove redundant interfaces (IEntity, ICollidableEntity, IPlayer)
- [x] EntityRegistry pre-filtered lists optimization
- [x] CollisionService simplification (inject IEntityRegistry)
