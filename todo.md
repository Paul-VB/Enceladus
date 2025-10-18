# Enceladus TODO List

## High Priority

### Collision System
- [ ] **Implement polygon decomposition algorithm** (concave → convex)
  - Location: `PolygonHitboxBuilder.cs`
  - Reason: Player submarine hitbox is concave, SAT only works with convex polygons
  - Related: Update `BuildFromPixelCoordinates` to use decomposition

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

- [ ] **Entity auto-registration or factory pattern**
  - Location: `EntityRegistry.cs:5`
  - Consider making entities register themselves on creation
  - Or use EntityFactory to enforce registration

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
  - Location: `SatCollisionDetector.cs:75, 76`
  - Why normal must point from OtherObject to Entity
  - Why we use center of vertices for direction check

- [ ] **Consider extracting vertex rotation logic**
  - Location: `VertexExtractor.cs:73`
  - Is this the only place we rotate vertices?
  - Extract to shared utility if used elsewhere

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
