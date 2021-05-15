using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour {
	[Header("Balance"), Space]
	[SerializeField] int pointToWin = 8;
	[SerializeField] int pointToLose = 8;
	[SerializeField] int pointToCombo = 4;
	[SerializeField] int loseGrowPerTurn = 1;
	int statLose;
	int statWin;
	int statCombo;
	int filledBars;

	[Header("Particles"), Space]
	[SerializeField] ParticleSystem particlesWhenComboFull;
	[SerializeField] ParticleSystem[] particlesWhenComboUsed;

	[Header("Refs"), Space]
	[SerializeField] Button buttonSpin;
	[SerializeField] Button[] buttonsSelectGroup;
	[Space]
	[SerializeField] Button useComboButton;
	[Space]
	[SerializeField] Circle circle;
	[SerializeField] ProgressBar progressBarWin;
	[SerializeField] ProgressBar progressBarLose;
	[SerializeField] ProgressBar progressBarCombo;

	bool isGroupSelectionShowed;
	bool isActivateComboBar;

	private void Awake() {
		Init();

		circle.onSpinEnd += OnSpinEnd;

		progressBarWin.onValueUpdated += OnSingleBarFill;
		progressBarLose.onValueUpdated += OnSingleBarFill;
		progressBarCombo.onValueUpdated += OnSingleBarFill;

		progressBarCombo.onValueUpdated += OnComboBarFill;
	}

	void Init() {
		circle.Init();
		progressBarWin.Init();
		progressBarLose.Init();
		progressBarCombo.Init();

		buttonSpin.interactable = true;

		foreach (var b in buttonsSelectGroup) {
			b.interactable = false;
		}

		useComboButton.image.raycastTarget = useComboButton.interactable = false;

		statLose = 0;
		statWin = 0;
		statCombo = 0;

		filledBars = 0;

		isGroupSelectionShowed = false;

		particlesWhenComboFull.Stop();
		foreach (var part in particlesWhenComboUsed) 
			part.Stop();
	}

	void UseGroup1() {
		circle.AnimateArrowsGroup1();

		circle.GetGroup1Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod);

		//TODO: 
		Debug.Log("green mod data");

		statLose -= blueMod;
		statWin += redMod;
		statCombo += yellowMod;
	}

	void UseGroup2() {
		circle.AnimateArrowsGroup2();

		circle.GetGroup2Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod);

		//TODO: 
		Debug.Log("green mod data");

		statLose -= blueMod;
		statWin += redMod;
		statCombo += yellowMod;
	}

	#region Mouse over Callbacks
	public void OnMouseOverGroup1() {
		circle.GetGroup1Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod);

		progressBarLose.UpdateHalfFillValue(statLose - blueMod + loseGrowPerTurn);
		progressBarWin.UpdateHalfFillValue(statWin + redMod);
		progressBarCombo.UpdateHalfFillValue(statCombo + yellowMod);

		//TODO: 
		Debug.Log("green mod UI");
	}

	public void OnMouseOverGroup2() {
		circle.GetGroup2Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod);
		
		progressBarLose.UpdateHalfFillValue(statLose - blueMod + loseGrowPerTurn);
		progressBarWin.UpdateHalfFillValue(statWin + redMod);
		progressBarCombo.UpdateHalfFillValue(statCombo + yellowMod);

		//TODO: 
		Debug.Log("green mod UI");
	}

	public void OnMouseOverGroupBoth() {
		circle.GetGroupBothModifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod);

		progressBarLose.UpdateHalfFillValue(statLose - blueMod + loseGrowPerTurn);
		progressBarWin.UpdateHalfFillValue(statWin + redMod);
		progressBarCombo.UpdateHalfFillValue(statCombo + yellowMod);

		//TODO: 
		Debug.Log("green mod UI");
	}

	public void OnMouseExitGroup1() {
		progressBarLose.UpdateHalfFillValue(statLose + loseGrowPerTurn);
		progressBarWin.ClearHalfFillValue();
		progressBarCombo.ClearHalfFillValue();
	}

	public void OnMouseExitGroup2() {
		progressBarLose.UpdateHalfFillValue(statLose + loseGrowPerTurn);
		progressBarWin.ClearHalfFillValue();
		progressBarCombo.ClearHalfFillValue();
	}

	public void OnMouseExitGroupBoth() {
		progressBarLose.UpdateHalfFillValue(statLose + loseGrowPerTurn);
		progressBarWin.ClearHalfFillValue();
		progressBarCombo.ClearHalfFillValue();
	}
	#endregion

	#region Button Callbacks
	public void OnClickSpin() {
		buttonSpin.interactable = false;

		filledBars = 0;

		circle.Spin();
	}

	public void OnClickSelectGroup1() {
		OnSelectAnyGroup();
		UseGroup1();
		AfterUseGroup();
	}

	public void OnClickSelectGroup2() {
		OnSelectAnyGroup();
		UseGroup2();
		AfterUseGroup();
	}

	public void OnClickComboBar() {
		useComboButton.image.raycastTarget = useComboButton.interactable = false;
		particlesWhenComboFull.Stop();

		if (isGroupSelectionShowed) {
			OnSelectAnyGroup();
			UseGroup1();
			UseGroup2();
			AfterUseGroup();

			progressBarCombo.UpdateValueNoCallback(statCombo = 0);
		}
		else {
			progressBarCombo.UpdateValueNoCallback(statCombo = 0);
			
			isActivateComboBar = true;

			foreach (var part in particlesWhenComboUsed)
				part.Play();
		}
	}

	void AfterUseGroup() {
		foreach (var part in particlesWhenComboUsed)
			part.Stop();

		if (statWin >= pointToWin) {
			statWin = pointToWin;
			//TODO: 
			Debug.Log("Win");
		}
		if (statWin < 0) {
			statWin = 0;
		}

		statLose += loseGrowPerTurn;

		if (statLose >= pointToLose) {
			statLose = pointToLose;
			//TODO: 
			Debug.Log("Lose");
		}
		if (statLose < 0) {
			statLose = 0;
		}

		if (statCombo > pointToCombo) {
			statCombo = pointToCombo;
		}
		if (statCombo < 0) {
			statCombo = 0;
		}

		//TODO: 
		Debug.Log("Fly effects");

		progressBarWin.UpdateValue(statWin);
		progressBarLose.UpdateValue(statLose);
		progressBarCombo.UpdateValue(statCombo);
	}

	void OnSelectAnyGroup() {
		foreach (var b in buttonsSelectGroup) {
			b.interactable = false;
		}

		isGroupSelectionShowed = false;

		progressBarCombo.ClearHalfFillValue();
		progressBarLose.ClearHalfFillValue();
		progressBarWin.ClearHalfFillValue();
	}
	#endregion

	#region Callbacks
	void OnSpinEnd() {
		if (isActivateComboBar) {
			isActivateComboBar = false;

			OnSelectAnyGroup();
			UseGroup1();
			UseGroup2();
			AfterUseGroup();

			progressBarCombo.UpdateValueNoCallback(statCombo = 0);
		}
		else {
			foreach (var b in buttonsSelectGroup) {
				b.interactable = true;
			}

			isGroupSelectionShowed = true;
			progressBarLose.UpdateHalfFillValue(statLose + loseGrowPerTurn);
		}
	}

	void OnAllEffectAnimationDone() {
		buttonSpin.interactable = true;
	}

	void OnComboBarFill() {
		if (statCombo == pointToCombo) {
			useComboButton.image.raycastTarget = useComboButton.interactable = true;

			particlesWhenComboFull.Play();
		}
	}

	void OnSingleBarFill() {
		++filledBars;

		if(filledBars == 3) {
			OnAllEffectAnimationDone();
		}
	}
	#endregion
}
