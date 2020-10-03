using UnityEngine;

namespace BIAL.Runtime.Interfaces
{
	public interface IPoolableObject<T> : ITearDown where T : Component, IPoolableObject<T>
	{
		ObjectPool<T> PoolOrigin { get; set; }
	}
}