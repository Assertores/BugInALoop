using System;
using BIAL.Runtime;
using BIAL.Runtime.DataStorage;
using BIAL.Runtime.Interfaces;

namespace BIAL.Entities
{
	public class Bug : Entity, IPoolableObject<Bug>
	{
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

		private bool CanLeaveScreen()
		{
			throw new NotImplementedException();
		}

		private bool IsAlone()
		{
			throw new NotImplementedException();
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