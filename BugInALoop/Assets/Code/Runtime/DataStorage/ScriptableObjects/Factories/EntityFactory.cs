using BIAL.Runtime.Entities;
using BIAL.Runtime.DataStorage;
using UnityEngine;

namespace BIAL.Runtime.Interfaces
{
	public abstract class EntityFactory : ScriptableObject
	{
		public abstract Entity CreateNewEntity(EntityConfig config);
	}
}