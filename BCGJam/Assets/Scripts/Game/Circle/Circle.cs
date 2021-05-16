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
	int zeroPos;

	public void Init() {
		zeroPos = 0;
		gameObject.transform.eulerAngles = Vector3.zero;

		for (int i = 0; i < sectors.Length; ++i) {
			sectors[i].Init(i);
		}

		isFirstSpin = true;
	}

	public void GetGroup1Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, float multipleMod) {
		redMod = 0;
		yellowMod = 0;
		blueMod = 0;
		greenMod = 0;

		foreach (var id in arrowsIdGroup1)
			sectors[(int)Mathf.Repeat(zeroPos + id, sectors.Length)].AddModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod);

		ApplyModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod, multipleMod);
	}

	public void GetGroup2Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, float multipleMod) {
		redMod = 0;
		yellowMod = 0;
		blueMod = 0;
		greenMod = 0;

		foreach (var id in arrowsIdGroup2)
			sectors[(int)Mathf.Repeat(zeroPos + id, sectors.Length)].AddModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod);

		ApplyModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod, multipleMod);
	}

	public void GetGroupBothModifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, float multipleMod) {
		redMod = 0;
		yellowMod = 0;
		blueMod = 0;
		greenMod = 0;

		foreach (var id in arrowsIdGroup1)
			sectors[(int)Mathf.Repeat(zeroPos + id, sectors.Length)].AddModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod);

		foreach (var id in arrowsIdGroup2)
			sectors[(int)Mathf.Repeat(zeroPos + id, sectors.Length)].AddModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod);

		ApplyModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod, multipleMod);
	}

	void ApplyModifiers(ref int redMod, ref int yellowMod, ref int blueMod, ref int greenMod, float multipleMod) {
		if(redMod > 1) 
			redMod = Mathf.RoundToInt(multipleMod * redMod);
		if (yellowMod > 1)
			yellowMod = Mathf.RoundToInt(multipleMod * yellowMod);
		if (blueMod > 1)
			blueMod = Mathf.RoundToInt(multipleMod * blueMod);
		if (greenMod > 1)
			greenMod = Mathf.RoundToInt(multipleMod * greenMod);
	}

	public void StartCatchUpgradeInput() {
		for (int i = 0; i < sectors.Length; ++i) {
			sectors[i].GetComponentInChildren<CircleCatchUpgrade>().isCatchInput = true;
		}
	}

	public void StopCatchUpgradeInput() {
		for (int i = 0; i < sectors.Length; ++i) {
			sectors[i].GetComponentInChildren<CircleCatchUpgrade>().isCatchInput = false;
		}
	}

	public void Spin() {
		int spinTimer = isFirstSpin ? firstSpin : spinRange.GetRandomValue();
		float degreesRotation = oneSpinDegree * spinTimer;

		zeroPos += 12 - spinTimer % 12;
		zeroPos %= 12;

		Debug.Log($"Spin by {spinTimer}. Curr zero: {zeroPos}");

		LeanTween.rotateAround(gameObject, Vector3.forward, degreesRotation, degreesRotation / spinPerSecond)
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

	public void Upgrade(int id, int level, CircleSector.SectorType type) {
		sectors[id].Upgrade(level, type);
	}
}
