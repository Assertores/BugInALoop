using BIAL.Runtime.DataStorage;
using BIAL.Runtime.Interfaces;
using BIAL.Runtime.Singletons;
using UnityEngine;
using UnityEngine.AI;

namespace BIAL.Runtime.Entities
{
	public class Bug : Entity, IPoolableObject<Bug>
	{
		private const float REACHED_THRESHOLD = 0.01f;
		private const int SEGMENT_COLLISION_LAYER = 1 << 9;

		private static GameObject HVanishPoint = null;
		public ObjectPool<Bug> PoolOrigin { get; set; }
		public int CurrentHealth;
		private bool leftSpawnZone;

		public override void Ready()
		{
			leftSpawnZone = false;
			Pen.s_changedBlocker += CheckOnHealthCheck;
		}

		protected sealed override void OnEntityInitialised()
		{
			if (Config is BugConfig bugConfig)
			{
				CurrentHealth = bugConfig.Health;
			}
		}

		private void FixedUpdate()
		{
			Movement();
			CheckForDeSpawn();
		}

		private void CheckOnHealthCheck()
		{
			if ((BehaviourFacade.s_instance.CurrentScene.value != Scene.Game) || ShouldDie())
			{
				return;
			}

			if (ShouldTakeDamage())
			{
				CurrentHealth--;
			}
		}

		private void Movement()
		{
			float moveDistance = ((BugConfig) Config).MovementSpeed * Time.fixedDeltaTime;
			Transform bugTransform = transform;
			Vector3 forward = bugTransform.forward;

			//Check for collision
			float rayDistance = (Config.EntityCollisionSize.y / 2) + moveDistance;
			bool hitSegment = Physics.BoxCast(bugTransform.position,
											(Vector3) Config.EntityCollisionSize / 2,
											forward,
											out RaycastHit hitInfo,
											Quaternion.Euler(bugTransform.eulerAngles),
											rayDistance,
											SEGMENT_COLLISION_LAYER);

			if (hitSegment)
			{
				float distanceDelta = hitInfo.distance - (Config.EntityCollisionSize.y / 2);
				moveDistance -= distanceDelta;
				bugTransform.position += forward * distanceDelta;
				bugTransform.forward = Vector2.Reflect(forward.XYZtoXZ(), hitInfo.normal.XYZtoXZ()).XZtoXYZ();
			}

			if (moveDistance > 0)
			{
				//Translate position
				bugTransform.position += forward * moveDistance;
			}
		}

		private void CheckForDeSpawn()
		{
			if (!leftSpawnZone)
			{
				leftSpawnZone = !InSpawnZone();
			}
			else if (InSpawnZone())
			{
				EntityStateManager.ForceRemoveEntity(this, false);
			}
		}

		private bool InSpawnZone()
		{
			Vector2 center = (LevelSettings.Current.EntityWalkRectLowerPoint + LevelSettings.Current.EntityWalkRectUpperPoint) / 2;
			Vector2 rectSize = LevelSettings.Current.EntityWalkRectUpperPoint - LevelSettings.Current.EntityWalkRectLowerPoint;
			float dist = (transform.position.XYZtoXZ() - center).sqrMagnitude;

			return dist > rectSize.sqrMagnitude;
		}

		public sealed override bool ShouldDie()
		{
			return CurrentHealth <= 0;
		}

		private bool ShouldTakeDamage()
		{
			return !CanLeaveScreen() && IsAlone();
		}

		private bool CanLeaveScreen()
		{
			if (!HVanishPoint)
			{
				HVanishPoint = GameObject.FindGameObjectWithTag("Finish");
			}

			NavMeshPath path = new NavMeshPath();
			if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1, NavMesh.AllAreas)
				&& NavMesh.CalculatePath(hit.position, HVanishPoint.transform.position, NavMesh.AllAreas, path))
			{
				if ((path.corners[path.corners.Length - 1].XYZtoXZ() - HVanishPoint.transform.position.XYZtoXZ()).sqrMagnitude < REACHED_THRESHOLD)
				{
					return true;
				}
			}

			return false;
		}

		private bool IsAlone()
		{
			foreach (Entity other in EntityStateManager.ActiveEntities)
			{
				if (other == this)
				{
					continue;
				}

				NavMeshPath path = new NavMeshPath();
				if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1, NavMesh.AllAreas)
					&& NavMesh.CalculatePath(transform.position, other.transform.position, NavMesh.AllAreas, path))
				{
					if ((path.corners[path.corners.Length - 1].XYZtoXZ() - other.transform.position.XYZtoXZ()).sqrMagnitude < REACHED_THRESHOLD)
					{
						return false;
					}
				}
			}

			return true;
		}

		public override void Death()
		{
			//TODO: Implement death animation/ Fading etc. here, but make sure it is independent from the Entity because it will be destroyed next frame
			Debug.Log("Died");
		}

		public override void TearDown()
		{
			Pen.s_changedBlocker -= CheckOnHealthCheck;
			PoolOrigin?.ReturnPoolObject(this);
		}

		private void OnDestroy()
		{
			Pen.s_changedBlocker -= CheckOnHealthCheck;
		}
	}
}