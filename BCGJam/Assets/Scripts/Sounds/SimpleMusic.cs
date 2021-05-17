using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class SimpleMusic : MonoBehaviour {
	[Header("Audio"), Space]
	[SerializeField] float fadeTime = 3.0f;
	[SerializeField] AudioSource loopMusic;

	[Header("Refs"), Space]
	[SerializeField] TemplateSplashScreenAnyKey splash;


	float volume;

	private void Awake() {
		volume = loopMusic.volume;

		splash.onShow += StartPlay;
	}

	public void StartPlay() {
		LeanTween.cancel(gameObject);

		loopMusic.volume = 0.0f;

		LeanTween.value(gameObject, loopMusic.volume, volume, fadeTime)
			.setEase(LeanTweenType.easeInOutSine)
			.setOnStart(loopMusic.Play) 
			.setOnUpdate((float v)=> { 
				loopMusic.volume = v;
			});

		LeanTween.delayedCall(loopMusic.clip.length - fadeTime - fadeTime, () => {
			LeanTween.value(gameObject, loopMusic.volume, 0.0f, fadeTime)
				.setEase(LeanTweenType.easeInOutSine)
				.setOnUpdate((float v) => {
					loopMusic.volume = v;
				})
				.setOnComplete(StartPlay);
		});
	}
}
