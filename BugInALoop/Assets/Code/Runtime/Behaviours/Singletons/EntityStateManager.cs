using System.Collections.Generic;
using AsserTOOLres;
using BIAL.Runtime.Entities;

namespace BIAL.Runtime.Singletons
{
	public class EntityStateManager : Singleton<EntityStateManager>
	{
		public static IReadOnlyList<Entity> ActiveEntities => ActiveEntitiesInternal;
		private static readonly List<Entity> ActiveEntitiesInternal = new List<Entity>();

		private void FixedUpdate()
		{
			ValidateEntityStates();
		}

		public static void AddActiveEntity(Entity newEntity)
		{
			ActiveEntitiesInternal.Add(newEntity);
			newEntity.Ready();
		}

		public static void ForceRemoveEntity(int index, bool killEntity = true)
		{
			if (killEntity)
			{
				ActiveEntitiesInternal[index].Death();
			}

			ActiveEntitiesInternal[index].TearDown();
			ActiveEntitiesInternal.RemoveAt(index);
		}

		private static void ValidateEntityStates()
		{
			for (int i = ActiveEntitiesInternal.Count; i >= 0; i--)
			{
				if (ActiveEntitiesInternal[i].ShouldDie())
				{
					ForceRemoveEntity(i);
				}
			}
		}
	}
}