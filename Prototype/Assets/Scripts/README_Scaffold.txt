Prefabs to create in the Unity Editor:

- Player: add CharacterController, PlayerController, MeleeAttack, Health, and assign Camera target.
- Horse: add Rigidbody, HorseController, mountPoint (empty child transform).
- SquadMembers: add NavMeshAgent, SquadAI, Health.
- Mushroom: add Mushroom script, Collider (with isTrigger enabled) for eating interaction.
- Tree: add Tree script, create child transform for climbPosition.

Notes:
- Bake a NavMesh in the scene for squad following.
- Assign layers for `hitMask` in MeleeAttack (e.g., 'Enemy').
- Create a simple VFX prefab for hitVFX and assign it to MeleeAttack.

New Features:
- Mushrooms: Walk into mushrooms to eat them and gain health + attack power boost. Press Space near mushrooms to jump on them for a bounce effect.
- Trees: Press F near trees to climb and hide from enemies. While hidden, squad AI will stop following. Press F again to climb down.
- Controls: F = Climb/Descend Tree, Space = Jump on Mushroom, E = Mount/Dismount Horse, H = Rally Squad, Mouse Click = Attack
