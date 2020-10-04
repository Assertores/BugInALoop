using System;
using BIAL.Runtime.DataStorage;
using BIAL.Runtime.Entities;
using UnityEngine;

namespace BIAL.Runtime.Interfaces
{
	public class BugFactory : EntityFactory
	{
		private static readonly ObjectPool<Bug> BugEntityPool = new ObjectPool<Bug>(CreateEmptyBug);

		public override Entity CreateNewEntity(EntityConfig config)
		{
			BugConfig bugConfig = config as BugConfig;
			if (bugConfig == null)
			{
				throw new Exception($"{nameof(BugFactory)} was requested to factorise an Entity based on a invalid {nameof(EntityConfig)} type: {config.GetType()}.");
			}

			Bug targetBug = BugEntityPool.GetPoolObject();
			for (int i = 0; i < targetBug.transform.childCount; i++)
			{
				Destroy(targetBug.transform.GetChild(i).gameObject);
			}

			targetBug.Initialise(bugConfig);
			Instantiate(config.EntityView, targetBug.transform);

			return targetBug;
		}

		private static Bug CreateEmptyBug()
		{
			GameObject bugCore = new GameObject();

			return bugCore.AddComponent<Bug>();
		}
	}
}