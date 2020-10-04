using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAL.Runtime {
	public class PlayRandomAudioSource : MonoBehaviour {
		[SerializeField] AudioSource[] audioSources;

		public void Play() {
			int index = Random.Range(0, audioSources.Length);

			//audioSources[index].Play();

			audioSources[index].PlayOneShot(audioSources[index].clip);
		}
	}
}
