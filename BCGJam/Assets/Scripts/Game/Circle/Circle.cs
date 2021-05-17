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
	[SerializeField] CircleArrow[] arrowsGroup1, arrowsGroup2;

	bool isFirstSpin;
	int zeroPos;

	public void Init() {
		zeroPos = 0;
		gameObject.transform.eulerAngles = Vector3.zero;

		for (int i = 0; i < sectors.Length; ++i) {
			sectors[i].Init(i);
		}

		foreach (var arr in arrowsGroup1) 
			arr.Init();
		foreach (var arr in arrowsGroup2) 
			arr.Init();

		isFirstSpin = true;
	}

	public void GetGroup1Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, out int redCount, out int blueCount, float multipleMod) {
		redMod = 0;
		yellowMod = 0;
		blueMod = 0;
		greenMod = 0;

		redCount = 0;
		blueCount = 0;

		foreach (var id in arrowsIdGroup1)
			sectors[(int)Mathf.Repeat(zeroPos + id, sectors.Length)].AddModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod, ref redCount, ref blueCount);

		ApplyModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod, redCount, blueCount, multipleMod);
	}

	public void GetGroup2Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, out int redCount, out int blueCount, float multipleMod) {
		redMod = 0;
		yellowMod = 0;
		blueMod = 0;
		greenMod = 0;

		redCount = 0;
		blueCount = 0;

		foreach (var id in arrowsIdGroup2)
			sectors[(int)Mathf.Repeat(zeroPos + id, sectors.Length)].AddModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod, ref redCount, ref blueCount);

		ApplyModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod, redCount, blueCount, multipleMod);
	}

	public void GetGroupBothModifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, out int redCount, out int blueCount, float multipleMod) {
		redMod = 0;
		yellowMod = 0;
		blueMod = 0;
		greenMod = 0;
		redCount = 0;
		blueCount = 0;

		foreach (var id in arrowsIdGroup1)
			sectors[(int)Mathf.Repeat(zeroPos + id, sectors.Length)].AddModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod, ref redCount, ref blueCount);

		foreach (var id in arrowsIdGroup2)
			sectors[(int)Mathf.Repeat(zeroPos + id, sectors.Length)].AddModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod, ref redCount, ref blueCount);

		ApplyModifiers(ref redMod, ref yellowMod, ref blueMod, ref greenMod, redCount, blueCount, multipleMod);
	}

	void ApplyModifiers(ref int redMod, ref int yellowMod, ref int blueMod, ref int greenMod, int redCount, int blueCount, float multipleMod) {
		if(redCount > 1)
			redMod += Mathf.RoundToInt((redCount - 1) * multipleMod);
		if(blueCount > 1)
			blueMod += Mathf.RoundToInt((blueCount - 1) * multipleMod);
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
			.setEase(LeanTweenType.easeOutSine)
			.setOnComplete(onSpinEnd);

		isFirstSpin = false;
	}

	public void AnimateArrowsGroup1() {
		foreach (var arr in arrowsGroup1) 
			arr.PlaySelectAnimation();
	}

	public void AnimateArrowsGroup2() {
		foreach (var arr in arrowsGroup2) 
			arr.PlaySelectAnimation();
	}

	public void StopAnimateArrowsGroup1() {
		foreach (var arr in arrowsGroup1) 
			arr.StopSelectAnimation();
	}

	public void StopAnimateArrowsGroup2() {
		foreach (var arr in arrowsGroup2) 
			arr.StopSelectAnimation();
	}

	public void AnimateSectorsGroup1() {
		foreach (var id in arrowsIdGroup1)
			sectors[(int)Mathf.Repeat(zeroPos + id, sectors.Length)].ShowAura();
	}

	public void AnimateSectorsGroup2() {
		foreach (var id in arrowsIdGroup2)
			sectors[(int)Mathf.Repeat(zeroPos + id, sectors.Length)].ShowAura();
	}

	public void StopAnimateSectorsGroup1() {
		foreach (var id in arrowsIdGroup1)
			sectors[(int)Mathf.Repeat(zeroPos + id, sectors.Length)].HideAura();
	}

	public void StopAnimateSectorsGroup2() {
		foreach (var id in arrowsIdGroup2)
			sectors[(int)Mathf.Repeat(zeroPos + id, sectors.Length)].HideAura();
	}

	public void Upgrade(int id, int level, CircleSector.SectorType type) {
		sectors[id].Upgrade(level, type);
	}
}
