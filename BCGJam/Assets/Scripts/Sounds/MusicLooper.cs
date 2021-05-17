using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class MusicLooper : MonoBehaviour {
	[Header("Audio"), Space]
	[SerializeField] float fadeTime = 3.0f;
	[SerializeField] AudioData[] audioData;

	[Header("Refs"), Space]
	[SerializeField] Game game;

	float[] volumes;

	private void Awake() {
		volumes = new float[audioData.Length];
		for(int i = 0; i < volumes.Length; ++i) {
			volumes[i] = audioData[i].source.volume;
			audioData[i].source.volume = 0.0f;
		}

		StartPlay(audioData[0], volumes[0]);
	}

	void StartPlay(AudioData data, float volume) {
		LeanTween.cancel(gameObject);

		data.source.volume = 0.0f;

		LeanTween.value(gameObject, data.source.volume, volume, fadeTime)
			.setEase(LeanTweenType.easeInOutSine)
			.setOnStart(data.source.Play)
			.setOnUpdate((float v) => {
				data.source.volume = v;
			});

		LeanTween.delayedCall(data.source.clip.length - fadeTime - fadeTime, () => {
			LeanTween.value(gameObject, data.source.volume, 0.0f, fadeTime)
				.setEase(LeanTweenType.easeInOutSine)
				.setOnUpdate((float v) => {
					data.source.volume = v;
				})
				.setOnComplete(()=> {

					for(int i = audioData.Length - 1; i >= 1; --i) {
						if(audioData[i].minNeededLevel <= game.GetRedFill()) {
							StartPlay(audioData[i], volumes[i]);
							break;
						}
					}
				});
		});
	}

	[Serializable]
	public struct AudioData {
		public int minNeededLevel;
		public AudioSource source;
	}
}
