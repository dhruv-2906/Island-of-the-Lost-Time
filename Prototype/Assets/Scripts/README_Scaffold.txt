Prefabs to create in the Unity Editor:

- Player: add CharacterController, PlayerController, MeleeAttack, Health, and assign Camera target.
- Horse: add Rigidbody, HorseController, mountPoint (empty child transform).
- SquadMembers: add NavMeshAgent, SquadAI, Health.

Notes:
- Bake a NavMesh in the scene for squad following.
- Assign layers for `hitMask` in MeleeAttack (e.g., 'Enemy').
- Create a simple VFX prefab for hitVFX and assign it to MeleeAttack.
