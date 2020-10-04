using System;
using System.Collections.Generic;
using BIAL.Runtime;
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
		private static GameObject HVanishPoint = null;
		public ObjectPool<Bug> PoolOrigin { get; set; }
		public int CurrentHealth;

		public override void Ready()
		{
			throw new NotImplementedException();
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
			//TODO: Dont use this in fixed update, call it when necessary 
			if (ShouldTakeDamage())
			{
				CurrentHealth--;
			}

			Movement();
		}

		private void Movement()
		{
			throw new NotImplementedException();
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
			if (NavMesh.CalculatePath(transform.position, HVanishPoint.transform.position, NavMesh.AllAreas, path))
			{
				if ((path.corners[path.corners.Length - 1].XYZtoXZ() - HVanishPoint.transform.position.XYZtoXZ()).sqrMagnitude < REACHED_THRESHOLD)
				{
					Debug.Log(gameObject.name + " has found the vanish point.");

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
				if (NavMesh.CalculatePath(transform.position, other.transform.position, NavMesh.AllAreas, path))
				{
					if ((path.corners[path.corners.Length - 1].XYZtoXZ() - other.transform.position.XYZtoXZ()).sqrMagnitude < REACHED_THRESHOLD)
					{
						Debug.Log(gameObject.name + " has found someone else.");

						return false;
					}
				}
			}

			return true;
		}

		public override void Death()
		{
			//TODO: Implement death animation/ Fading etc. here, but make sure it is independent from the Entity because it will be destroyed next frame
			throw new NotImplementedException();
		}

		public override void TearDown()
		{
			PoolOrigin?.ReturnPoolObject(this);
		}
	}
}