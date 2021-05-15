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
	[SerializeField] int firstSpin = 24;

	[Header("Refs"), Space]
	[SerializeField] CircleSector[] sectors;
	[SerializeField] CircleArrow[] arrowsGroup1;
	[SerializeField] CircleArrow[] arrowsGroup2;

	bool isFirstSpin;

	public void Init() {
		for (int i = 0; i < sectors.Length; ++i) {
			sectors[i].Init(i);
		}

		isFirstSpin = true;
	}

	public void GetGroup1Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod) {
		redMod = 0;
		yellowMod = 0;
		blueMod = 0;
		greenMod = 0;

		foreach (var id in arrowsIdGroup1) 
			sectors[id].AddModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod);
	}

	public void GetGroup2Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod) {
		redMod = 0;
		yellowMod = 0;
		blueMod = 0;
		greenMod = 0;

		foreach (var id in arrowsIdGroup2) 
			sectors[id].AddModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod);
	}

	public void GetGroupBothModifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod) {
		redMod = 0;
		yellowMod = 0;
		blueMod = 0;
		greenMod = 0;

		foreach (var id in arrowsIdGroup1) 
			sectors[id].AddModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod);

		foreach (var id in arrowsIdGroup2) 
			sectors[id].AddModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod);
	}

	public void Spin() {
		int spinTimer = isFirstSpin ? firstSpin : spinRange.GetRandomValue();
		float degreesRotation = oneSpinDegree * spinTimer;
		Debug.Log($"Spin by {spinTimer}");

		LeanTween.rotateAround(gameObject, Vector3.forward,  degreesRotation, degreesRotation / spinPerSecond)
			.setEase(LeanTweenType.easeOutQuint)
			.setOnComplete(onSpinEnd);

		isFirstSpin = false;
	}

	public void AnimateArrowsGroup1() {
		foreach (var arr in arrowsGroup1) {
			arr.PlaySelectAnimation();
		}
	}

	public void AnimateArrowsGroup2() {
		foreach (var arr in arrowsGroup2) {
			arr.PlaySelectAnimation();
		}
	}
}
