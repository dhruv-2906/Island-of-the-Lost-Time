# Mistvale — Prototype

This repository contains a Unity WebGL prototype for a third-person mounted-combat demo based on the provided script.

## Goals
- Third-person player controller (WASD) with mouse orbit camera
- Mount/dismount horse (E), Shift to gallop
- Melee attack (left-click): overlap-sphere deals 25 damage and spawns hit VFX
- Rally squad with horn (press `H`) — squad follows player using NavMesh
- WebGL build with GitHub Actions for deployment to GitHub Pages

## Quick start
1. Open Unity Hub and add the project folder: `C:/3d game/Prototype`.
2. Recommended Unity: **2024 LTS** (URP recommended for WebGL). Set up WebGL build support.
3. Open `Assets/Scenes/Main.unity` (placeholder file created; create the scene in the Editor if needed).
4. Import your character images into `Assets/Art` and create models/textures or plan billboard sprites.

## CI / Deployment
The repository includes a GitHub Actions workflow (`.github/workflows/unity-build.yml`) that uses game-ci's Unity Builder to create a WebGL build and optionally deploy to GitHub Pages. You must add appropriate secrets (see workflow comments).

## License
MIT — see `LICENSE`.
