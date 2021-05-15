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
	}

	void UseGroup1() {
		//TODO: 
		Debug.Log("Calc sector data");

		statLose += 2;
		++statWin;
		statCombo += 2;
	}

	void UseGroup2() {
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

		progressBarCombo.UpdateValue(statCombo = 0);

		//TODO: 
		Debug.Log("Use combo");
	}

	void CheckConditions() {
		if (statWin > pointToWin) {
			statWin = pointToWin;
			//TODO: 
			Debug.Log("Win");
		}

		if (statLose > pointToLose) {
			statLose = pointToLose;
			//TODO: 
			Debug.Log("Lose");
		}

		if (statCombo > pointToCombo) {
			statCombo = pointToCombo;
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
	}
	#endregion


	#region Callbacks
	void OnSpinEnd() {
		cgAfterSpin.interactable = cgAfterSpin.blocksRaycasts = true;
		LeanTweenEx.ChangeAlpha(cgAfterSpin, 1.0f, 0.2f);
	}

	void OnAllEffectAnimationDone() {
		cgBeforeSpin.interactable = cgBeforeSpin.blocksRaycasts = true;
		LeanTweenEx.ChangeAlpha(cgBeforeSpin, 1.0f, 0.2f);
	}

	void OnComboBarFill() {
		if (statCombo == pointToCombo) {
			useComboButton.interactable = true;

			//TODO: 
			Debug.Log("Effect for fill");
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
