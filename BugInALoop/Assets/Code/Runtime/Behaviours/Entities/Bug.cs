using System;
using System.Collections.Generic;
using BIAL.Runtime;
using BIAL.Runtime.DataStorage;
using BIAL.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace BIAL.Entities
{
	public class Bug : Entity, IPoolableObject<Bug>
	{
		public ObjectPool<Bug> PoolOrigin { get; set; }
		public int CurrentHealth;

		static List<Bug> references = new List<Bug>();

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

		private void Awake() {
			references.Add(this);
		}

		private void OnDestroy() {
			references.Remove(this);
		}

		private void FixedUpdate()
		{
			if (ShouldTakeDamage())
			{
				CurrentHealth--;
			}
		}

		public sealed override bool ShouldDie()
		{
			return CurrentHealth <= 0;
		}

		private bool ShouldTakeDamage()
		{
			return !CanLeaveScreen() && IsAlone();
		}

		GameObject h_vanishPoint = null;
		private bool CanLeaveScreen()
		{
			if(!h_vanishPoint) {
				h_vanishPoint = GameObject.FindGameObjectWithTag("Finish");
			}
			var path = new NavMeshPath();
			if(NavMesh.CalculatePath(transform.position, h_vanishPoint.transform.position, NavMesh.AllAreas, path))
			{
				if(path.corners[path.corners.Length - 1].x == h_vanishPoint.transform.position.x &&
					path.corners[path.corners.Length - 1].z == h_vanishPoint.transform.position.z)
				{
					Debug.Log(gameObject.name + " has found the vanishpoint.");
					return true;
				}
			}
			return false;
		}

		private bool IsAlone()
		{
			foreach(var it in references) {
				var path = new NavMeshPath();
				if(NavMesh.CalculatePath(transform.position, it.transform.position, NavMesh.AllAreas, path))
				{
					if(path.corners[path.corners.Length - 1].x == it.transform.position.x &&
						path.corners[path.corners.Length - 1].z == it.transform.position.z)
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
			throw new NotImplementedException();
		}

		public override void TearDown()
		{
			PoolOrigin?.ReturnPoolObject(this);
		}
	}
}