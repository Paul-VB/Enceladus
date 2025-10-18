# Enceladus - Gameplay Design Document

## I. Core Concept & Setting

**Genre**: 2D Submersible Action/Mining/Survival

**Setting**: The lawless, deep subsurface ocean of Saturn's moon Enceladus.

**Theme**: Morally gray resource exploitation and combat against rival mining companies.

**Physics**: Low gravity and near-neutral buoyancy create a unique "floaty flying" movement. The submersible behaves more like a spacecraft than a traditional submarine, giving combat and traversal a distinctive feel.

## II. The Submersible & Control Modes

The submersible is a heavy-duty mining rig whose systems are centered on two core modes:

### 1. COARSE MODE (Traversal)
- **Purpose**: High-speed movement, dodging, and fleeing in open water
- **Movement**: WASD translates the sub. Sub auto-levels for maximum thrust
- **Combat**: Uses 360-degree Aft Weapons (LMB) for defense while moving
- **Use Case**: Navigating between mining sites, escaping dangerous situations, combat while retreating

### 2. FINE MODE (Precision)
- **Purpose**: Drilling, careful navigation, and targeted combat
- **Movement**: WASD slews the sub slowly while holding a fixed attitude
- **Combat**: Rotates the sub with the mouse to aim the fixed-arc Mining Lasers (RMB)
- **Use Case**: Extracting resources, precise maneuvering in tight spaces, stationary combat

### Mode Switching
Instantly toggled with a single key press (e.g., SHIFT). Mastering when to switch between modes is a core skill.

## III. Progression & Customization

### Equipment Systems

Equipment (Weapons, Power Cores, Melt-Beam) is governed by two independent systems:

#### Mark System
Defines base power and efficiency (Mark 1, Mark 2, Mark 3...).
- Higher marks = more powerful/efficient base stats
- Linear progression path

#### Quality System
Defines the number of Augment Slots. The better the quality, the more slots
- Common (white) 
- Uncommon (green)
- Rare (blue)
- Very Rare (purple)
- Epic/Legendary (orange) - we might not have this high of quality. undecided if we want this many tiers

This creates interesting choices: a high-mark common item vs. a low-mark rare item with more customization potential.

#### Augment Extraction
Allows a player to destroy an item to salvage its augment and apply it to a preferred item. This prevents players from being locked into suboptimal gear and encourages experimentation.

### Key Augments

#### Weapon Augments
- **Rage Rounds**: Turn enemies against each other (crowd control)
- **Plasma Lense**: Damage over Time effect
- **Cryo-Projector**: Slows targets (useful for kiting or escape)

#### Power Core Augments
- **Nanite Repair**: Passive hull regeneration
- **Phase Shift**: Adds dodge chance (reduces incoming damage)
- **Heat Sink Array**: Slows tunnel refreeze rate (extends safe exploration time)

## IV. Environment & Hazards

### Iceologic Instability
Drilling too much in one sector causes temporary instability, increasing tunnel collapse risk. This forces the player to:
- Move to a new area when instability is high
- Return to previously-mined sectors after they've "cooled down"
- Plan mining routes strategically

### Tunnel Refreeze 
This feature is not confirmed if we want it in the game. it might be annoying for players
Mined tunnels slowly freeze over, creating time pressure for:
- Deep dives (need to get back out before tunnels seal)
- Escape routes (previously safe paths may become blocked)
- Resource extraction (can't stay in one place indefinitely)

### Biomes & Loot

Different sectors feature unique hostile life and high-value loot:

#### Minerals
Rare elements embedded in the ice, extracted by the Melt-Beam:
- Used for equipment upgrades and trading
- Different biomes contain different mineral types
- Deeper sectors = rarer minerals

#### Organics (Goo)
Specialized compounds harvested from unique alien life forms:
- **Catalyst Gel**: Harvested from swarm creatures
- **Carapace Resins**: Harvested from armored predators
- Used for high-tier augments and rare upgrades

## V. Economy & Meta-Game

### Hostile Rivals
Competing mining subs attack to steal your loot:
- Destroyed rivals yield scrap, unique upgrades, and their haul
- Creates risk/reward: fight for extra loot or flee to preserve your haul
- Different rival types with varying difficulty and rewards

### The Base
Always accessible location for:
- **Selling haul**: Convert minerals and organics into currency
- **Repairs**: Fix hull damage
- **Refueling**: Energy is required for drilling and thrusters
- Safe haven from environmental hazards

### Orbital Trader
A high-value vendor who is only accessible at the base for a few minutes each "day":
- Sells rare, high-tier weapons and core augments
- Time-limited availability creates urgency
- Expensive but offers items not available elsewhere
- Adds meta-game planning: "Should I do one more dive or rush back for the trader?"

## VI. Core Gameplay Loop

1. **Prepare**: At base, repair, refuel, upgrade equipment
2. **Dive**: Travel to mining sector, extract resources
3. **Navigate hazards**: Manage tunnel refreeze, avoid instability, fight/flee from rivals
4. **Extract**: Mine minerals, harvest organics from hostile life
5. **Return**: Race back before tunnels seal or energy runs out
6. **Trade**: Sell haul, check for Orbital Trader, upgrade equipment
7. **Repeat**: Venture deeper/farther for better loot

## VII. Design Pillars

1. **Meaningful Choices**: Dual control modes, equipment customization, risk/reward decisions
2. **Time Pressure**: Tunnel refreeze, Orbital Trader, energy management
3. **Spatial Awareness**: Managing refreeze, instability zones, escape routes
4. **Build Diversity**: Mark/Quality/Augment systems allow many viable builds
5. **Skill Expression**: Control mode mastery, combat tactics, route planning
