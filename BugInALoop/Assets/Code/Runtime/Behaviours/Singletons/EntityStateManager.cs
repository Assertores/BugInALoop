﻿using System.Collections.Generic;
using BIAL.Runtime.Entities;
using UnityEngine;

namespace BIAL.Runtime.Singletons
{
	public class EntityStateManager : MonoBehaviour
	{
		public static IReadOnlyList<Entity> ActiveEntities => ActiveEntitiesInternal;
		private static readonly List<Entity> ActiveEntitiesInternal = new List<Entity>();

		private void Start()
		{
			BehaviourFacade.s_instance.CurrentScene += SceneChanged;
		}

		private void OnDestroy()
		{
			if (BehaviourFacade.Exists())
			{
				BehaviourFacade.s_instance.CurrentScene -= SceneChanged;
			}
		}

		private void SceneChanged()
		{
			if (BehaviourFacade.s_instance.CurrentScene.value == Scene.GameOver)
			{
				ClearEntities();
			}
		}

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

		public static void ForceRemoveEntity(Entity target, bool killEntity = true)
		{
			for (int i = 0; i < ActiveEntitiesInternal.Count; i++)
			{
				if (ActiveEntitiesInternal[i] == target)
				{
					ForceRemoveEntity(i, killEntity);

					return;
				}
			}
		}

		private static void ValidateEntityStates()
		{
			for (int i = ActiveEntitiesInternal.Count - 1; i >= 0; i--)
			{
				if (ActiveEntitiesInternal[i].ShouldDie())
				{
					ForceRemoveEntity(i);
				}
			}
		}

		private static void ClearEntities()
		{
			for (int i = ActiveEntitiesInternal.Count - 1; i >= 0; i--)
			{
				ForceRemoveEntity(i);
			}
		}
	}
}