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
	int statLose;
	int statWin;
	int statCombo;
	int filledBars;

	[Header("Particles"), Space]
	[SerializeField] ParticleSystem particlesWhenComboFull;
	[SerializeField] ParticleSystem[] particlesWhenComboUsed;

	[Header("Refs"), Space]
	[SerializeField] CanvasGroup cgBeforeSpin;
	[SerializeField] CanvasGroup cgAfterSpin;
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

		cgBeforeSpin.interactable = cgBeforeSpin.blocksRaycasts = true;
		cgBeforeSpin.alpha = 1.0f;


		cgAfterSpin.interactable = cgAfterSpin.blocksRaycasts = false;
		cgAfterSpin.alpha = 0.0f;

		useComboButton.interactable = false;

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

		statLose += 2;
		++statWin;
		statCombo += 2;
	}

	void UseGroup2() {
		circle.AnimateArrowsGroup2();
		
		//TODO: 
		Debug.Log("Calc sector data");

		statLose -= 2;
		--statWin;
		++statCombo;
	}

	#region Button Callbacks
	public void OnClickSpin() {
		cgBeforeSpin.interactable = cgBeforeSpin.blocksRaycasts = false;
		LeanTweenEx.ChangeAlpha(cgBeforeSpin, 0.0f, 0.2f);

		filledBars = 0;

		circle.Spin();
	}

	public void OnClickSelectGroup1() {
		OnSelectAnyGroup();
		UseGroup1();
		CheckConditions();
	}

	public void OnClickSelectGroup2() {
		OnSelectAnyGroup();
		UseGroup2();
		CheckConditions();
	}

	public void OnClickComboBar() {
		useComboButton.interactable = false;
		particlesWhenComboFull.Stop();

		if (isGroupSelectionShowed) {
			progressBarCombo.UpdateValueNoCallback(statCombo = 0);

			OnSelectAnyGroup();
			UseGroup1();
			UseGroup2();
			CheckConditions();
		}
		else {
			progressBarCombo.UpdateValueNoCallback(statCombo = 0);
			
			isActivateComboBar = true;

			foreach (var part in particlesWhenComboUsed)
				part.Play();
		}
	}

	void CheckConditions() {
		if (statWin > pointToWin) {
			statWin = pointToWin;
			//TODO: 
			Debug.Log("Win");
		}
		if (statWin < 0) {
			statWin = 0;
		}

		if (statLose > pointToLose) {
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
		cgAfterSpin.interactable = cgAfterSpin.blocksRaycasts = false;
		LeanTweenEx.ChangeAlpha(cgAfterSpin, 0.0f, 0.2f);
		isGroupSelectionShowed = false;
	}
	#endregion


	#region Callbacks
	void OnSpinEnd() {
		if (isActivateComboBar) {
			isActivateComboBar = false;

			foreach (var part in particlesWhenComboUsed)
				part.Stop();

			OnSelectAnyGroup();
			UseGroup1();
			UseGroup2();
			CheckConditions();
		}
		else {
			cgAfterSpin.interactable = cgAfterSpin.blocksRaycasts = true;
			LeanTweenEx.ChangeAlpha(cgAfterSpin, 1.0f, 0.2f);
			isGroupSelectionShowed = true;
		}
	}

	void OnAllEffectAnimationDone() {
		cgBeforeSpin.interactable = cgBeforeSpin.blocksRaycasts = true;
		LeanTweenEx.ChangeAlpha(cgBeforeSpin, 1.0f, 0.2f);
	}

	void OnComboBarFill() {
		if (statCombo == pointToCombo) {
			useComboButton.interactable = true;

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
