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
	[SerializeField] AudioSource loopMusic;

	[Header("Refs"), Space]
	[SerializeField] TemplateSplashScreenAnyKey splash;


	private void Awake() {
		loopMusic.loop = true;
		loopMusic.Play();
	}
}
