using System;
using BIAL.Runtime.DataStorage;
using BIAL.Runtime.Spawning;
using UnityEngine;

namespace BIAL.Runtime.Singletons
{
	public class GameInitialiser : MonoBehaviour
	{
		public static Action GameSceneInitialised;
		public static Action GameSceneTerminated;

		[SerializeField] private LevelSettings targetLevelSettings = default;

		private bool gameSceneInitialised;
		private SpawningDirector[] activeDirectors;

		private void Awake()
		{
			BehaviourFacade.s_instance.CurrentScene += OnSceneChange;
		}

		private void OnDestroy()
		{
			if (BehaviourFacade.Exists())
			{
				BehaviourFacade.s_instance.CurrentScene -= OnSceneChange;
			}
		}

		private void OnSceneChange()
		{
			if (BehaviourFacade.s_instance.CurrentScene.value == Scene.Game)
			{
				InitialiseGameScene();
			}
			else if (gameSceneInitialised)
			{
				TerminateGameScene();
			}
		}

		private void InitialiseGameScene()
		{
			gameSceneInitialised = true;
			LevelSettings.Current = targetLevelSettings;
			activeDirectors = new SpawningDirector[targetLevelSettings.StartSpawningDirectors.Length];
			for (int i = 0; i < targetLevelSettings.StartSpawningDirectors.Length; i++)
			{
				activeDirectors[i] = Instantiate(targetLevelSettings.StartSpawningDirectors[i]);
			}

			GameSceneInitialised?.Invoke();
		}

		private void TerminateGameScene()
		{
			gameSceneInitialised = false;
			foreach (SpawningDirector director in activeDirectors)
			{
				director.ForceDestroyDirector();
			}

			LevelSettings.Current = null;
			activeDirectors = null;
			GameSceneTerminated?.Invoke();
		}
	}
}