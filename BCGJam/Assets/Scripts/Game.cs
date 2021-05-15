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

		//TODO: 
		Debug.Log("Calc sector data");

		statLose += 3;
		--statWin;
		statCombo += 2;
	}

	void UseGroup2() {
		circle.AnimateArrowsGroup2();

		//TODO: 
		Debug.Log("Calc sector data");

		--statLose;
		statWin += 3;
	}

	#region Button Callbacks
	public void OnMouseOverGroup1() {
		progressBarLose.UpdateHalfFillValue(statLose + 3 + loseGrowPerTurn);
		progressBarWin.UpdateHalfFillValue(statWin - 1);
		progressBarCombo.UpdateHalfFillValue(statCombo + 2);
	}

	public void OnMouseOverGroup2() {
		progressBarLose.UpdateHalfFillValue(statLose - 1 + loseGrowPerTurn);
		progressBarWin.UpdateHalfFillValue(statWin + 3);
		progressBarCombo.UpdateHalfFillValue(statCombo);
	}

	public void OnMouseOverGroupBoth() {
		progressBarLose.UpdateHalfFillValue(statLose + 3 - 1 + loseGrowPerTurn);
		progressBarWin.UpdateHalfFillValue(statWin - 1 + 3);
		progressBarCombo.UpdateHalfFillValue(statCombo + 2);
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
