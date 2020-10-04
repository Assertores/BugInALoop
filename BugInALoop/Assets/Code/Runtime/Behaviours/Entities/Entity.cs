using BIAL.Runtime.DataStorage;
using UnityEngine;

namespace BIAL.Runtime.Entities
{
	public abstract class Entity : MonoBehaviour
	{
		public EntityConfig Config { get; private set; }

		public void Initialise(EntityConfig config)
		{
			Config = config;
			OnEntityInitialised();
		}

		public abstract void Ready();
		public abstract bool ShouldDie();
		public abstract void Death();
		public abstract void TearDown();

		protected virtual void OnEntityInitialised() { }
	}
}