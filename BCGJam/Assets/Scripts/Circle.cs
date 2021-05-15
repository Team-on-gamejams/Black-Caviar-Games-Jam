using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Circle : MonoBehaviour {
	public Action onSpinEnd;

	[Header("Balance"), Space]
	[SerializeField] int[] arrowsIdGroup1;
	[SerializeField] int[] arrowsIdGroup2;

	[Header("Spin data"), Space]
	[SerializeField] Vector2 spinRange = new Vector2(100, 101);
	[SerializeField] float spinPerSecond = 1440f;
	[SerializeField] float oneSpinDegree = 30.0f;

	[Header("Refs"), Space]
	[SerializeField] CircleSector[] sectors;

	public void Init() {
		for (int i = 0; i < sectors.Length; ++i) {
			sectors[i].Init(i);
		}
	}

	public void Spin() {
		int spinTimer = spinRange.GetRandomValue();
		float degreesRotation = oneSpinDegree * spinTimer;
		Debug.Log($"Spin by {spinTimer}");

		LeanTween.rotateAround(gameObject, Vector3.forward,  degreesRotation, degreesRotation / spinPerSecond)
			.setEase(LeanTweenType.easeOutQuint)
			.setOnComplete(onSpinEnd);
	}
}
