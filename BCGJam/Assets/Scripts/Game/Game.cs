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
	[SerializeField] float statMultiplierFor2Plus = 2.0f;
	[SerializeField] int pointToWin = 8;
	[SerializeField] int pointToLose = 8;
	[SerializeField] int pointToCombo = 4;
	[Space]
	[SerializeField] int loseGrowPerTurn = 1;
	[SerializeField] int turnsToIncreaseGrow = 5;
	[Space]
	[SerializeField] int pointToUpgradeSingle = 1;
	[SerializeField] int pointToUpgradeDouble = 3;
	[SerializeField] int pointToUpgradeTripple = 4;

	int statLose;
	int statWin;
	int statCombo;

	int statLoseGrowPerTurn;
	int statTurnsToIncreaseGrow;

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
	[Space]
	[SerializeField] ProgressBar progressBarWin;
	[SerializeField] ProgressBar progressBarLose;
	[SerializeField] ProgressBar progressBarCombo;
	[SerializeField] ProgressBar progressBarUpgrade;
	[SerializeField] ProgressBar progressBarLoseGrow;
	[Space]
	[SerializeField] ShowMouseTooltip defeatBarTooltip;
	[SerializeField] [Multiline(3)] string defeatBarTooltipText;

	[Header("Refs - menu"), Space]
	[SerializeField] MenuManager menuManager;
	[SerializeField] MenuWinPopup popupWin;
	[SerializeField] MenuLosePopup popupLose;
	public MenuUpgradePopup popupUpgrade;

	bool isGroupSelectionShowed;
	bool isActivateComboBar;
	int lastGreenMod;

	private void Awake() {
		Init();

		circle.onSpinEnd += OnSpinEnd;

		progressBarWin.onValueUpdated += OnSingleBarFill;
		progressBarLose.onValueUpdated += OnSingleBarFill;
		progressBarCombo.onValueUpdated += OnSingleBarFill;
		progressBarLoseGrow.onValueUpdated += OnSingleBarFill;
		progressBarUpgrade.onValueUpdated += OnSingleBarFill;

		progressBarCombo.onValueUpdated += OnComboBarFill;

		popupUpgrade.onStartSelectUpgradeEvent += StartSelectSectorForUpgrade;
		popupUpgrade.onSkipUpgradeEvent += AllowSpin;

		GameManager.Instance.game = this;
	}

	public void Init() {
		circle.Init();
		progressBarWin.Init();
		progressBarLose.Init();
		progressBarCombo.Init();
		progressBarLoseGrow.Init();
		progressBarUpgrade.Init();

		buttonSpin.interactable = true;

		foreach (var b in buttonsSelectGroup) {
			b.image.raycastTarget = b.interactable = false;
		}

		useComboButton.image.raycastTarget = useComboButton.interactable = false;

		statLose = 0;
		statWin = 0;
		statCombo = 0;

		filledBars = 0;

		statLoseGrowPerTurn = loseGrowPerTurn;
		statTurnsToIncreaseGrow = 0;

		defeatBarTooltip.UpdateText(defeatBarTooltipText.Replace("{num}", statLoseGrowPerTurn.ToString()));

		isGroupSelectionShowed = false;

		particlesWhenComboFull.Stop();
		foreach (var part in particlesWhenComboUsed) 
			part.Stop();
	}

	void UseGroup1() {
		circle.AnimateArrowsGroup1();

		circle.GetGroup1Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, statMultiplierFor2Plus);

		statLose -= blueMod;
		statWin += redMod;
		statCombo += yellowMod;
		lastGreenMod = greenMod;
	}

	void UseGroup2() {
		circle.AnimateArrowsGroup2();

		circle.GetGroup2Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, statMultiplierFor2Plus);

		statLose -= blueMod;
		statWin += redMod;
		statCombo += yellowMod;
		lastGreenMod = greenMod;
	}

	void UseGroupBoth() {
		circle.AnimateArrowsGroup1();
		circle.AnimateArrowsGroup2();

		circle.GetGroupBothModifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, statMultiplierFor2Plus);

		statLose -= blueMod;
		statWin += redMod;
		statCombo += yellowMod;
		lastGreenMod = greenMod;
	}

	#region Mouse over Callbacks
	public void OnMouseOverGroup1() {
		circle.GetGroup1Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, statMultiplierFor2Plus);

		progressBarLose.UpdateHalfFillValue(statLose - blueMod + statLoseGrowPerTurn);
		progressBarWin.UpdateHalfFillValue(statWin + redMod);
		progressBarCombo.UpdateHalfFillValue(statCombo + yellowMod);
		progressBarUpgrade.UpdateHalfFillValue(greenMod);
	}

	public void OnMouseOverGroup2() {
		circle.GetGroup2Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, statMultiplierFor2Plus);

		progressBarLose.UpdateHalfFillValue(statLose - blueMod + statLoseGrowPerTurn);
		progressBarWin.UpdateHalfFillValue(statWin + redMod);
		progressBarCombo.UpdateHalfFillValue(statCombo + yellowMod);
		progressBarUpgrade.UpdateHalfFillValue(greenMod);
	}

	public void OnMouseOverGroupBoth() {
		circle.GetGroupBothModifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, statMultiplierFor2Plus);

		progressBarLose.UpdateHalfFillValue(statLose - blueMod + statLoseGrowPerTurn);
		progressBarWin.UpdateHalfFillValue(statWin + redMod);
		progressBarCombo.UpdateHalfFillValue(statCombo + yellowMod);
		progressBarUpgrade.UpdateHalfFillValue(greenMod);
	}

	public void OnMouseExitGroup1() {
		progressBarLose.UpdateHalfFillValue(statLose + statLoseGrowPerTurn);
		progressBarWin.ClearHalfFillValue();
		progressBarCombo.ClearHalfFillValue();
		progressBarUpgrade.ClearHalfFillValue();
	}

	public void OnMouseExitGroup2() {
		progressBarLose.UpdateHalfFillValue(statLose + statLoseGrowPerTurn);
		progressBarWin.ClearHalfFillValue();
		progressBarCombo.ClearHalfFillValue();
		progressBarUpgrade.ClearHalfFillValue();
	}

	public void OnMouseExitGroupBoth() {
		progressBarLose.UpdateHalfFillValue(statLose + statLoseGrowPerTurn);
		progressBarWin.ClearHalfFillValue();
		progressBarCombo.ClearHalfFillValue();
		progressBarUpgrade.ClearHalfFillValue();
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
			UseGroupBoth();
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
		}
		if (statWin < 0) {
			statWin = 0;
		}

		statLose += statLoseGrowPerTurn;

		++statTurnsToIncreaseGrow;
		if(statTurnsToIncreaseGrow > turnsToIncreaseGrow) {
			statTurnsToIncreaseGrow = 0;
			++statLoseGrowPerTurn;
			defeatBarTooltip.UpdateText(defeatBarTooltipText.Replace("{num}", statLoseGrowPerTurn.ToString()));
		}

		if (statLose >= pointToLose) {
			statLose = pointToLose;
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

		progressBarLoseGrow.ClearHalfFillValue();
		progressBarLoseGrow.UpdateValue(statTurnsToIncreaseGrow);

		progressBarUpgrade.ClearHalfFillValue();
		progressBarUpgrade.UpdateValue(lastGreenMod);
	}

	void OnSelectAnyGroup() {
		foreach (var b in buttonsSelectGroup) {
			b.image.raycastTarget = b.interactable = false;
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
			UseGroupBoth();
			AfterUseGroup();

			progressBarCombo.UpdateValueNoCallback(statCombo = 0);
		}
		else {
			foreach (var b in buttonsSelectGroup) {
				b.image.raycastTarget = b.interactable = true;
			}

			isGroupSelectionShowed = true;
			progressBarLose.UpdateHalfFillValue(statLose + statLoseGrowPerTurn);
			progressBarLoseGrow.UpdateHalfFillValue(statTurnsToIncreaseGrow + 1);
		}
	}

	void OnAllEffectAnimationDone() {
		LeanTween.delayedCall(gameObject, 0.5f, () => {
			if (statWin >= pointToWin) {
				menuManager.Show(popupWin, false);
			}
			else if (statLose >= pointToLose) {
				menuManager.Show(popupLose, false);
			}
			else {
				if (lastGreenMod != 0) {
					progressBarUpgrade.UpdateValueNoCallback(0);

					if (lastGreenMod < pointToUpgradeSingle) {
						AllowSpin();
					}
					else if (lastGreenMod < pointToUpgradeDouble) {
						popupUpgrade.InitWindow(0);
						menuManager.Show(popupUpgrade, false);
					}
					else if (lastGreenMod < pointToUpgradeTripple) {
						popupUpgrade.InitWindow(1);
						menuManager.Show(popupUpgrade, false);
					}
					else if (lastGreenMod >= pointToUpgradeTripple) {
						popupUpgrade.InitWindow(2);
						menuManager.Show(popupUpgrade, false);
					}

					lastGreenMod = 0;
				}
				else {
					AllowSpin();
				}
			}
		});
	}

	void OnComboBarFill() {
		if (statCombo == pointToCombo) {
			useComboButton.image.raycastTarget = useComboButton.interactable = true;

			particlesWhenComboFull.Play();
		}
	}

	void OnSingleBarFill() {
		++filledBars;

		if(filledBars == 5) {
			OnAllEffectAnimationDone();
		}
	}

	void AllowSpin() {
		buttonSpin.interactable = true;
	}

	void StartSelectSectorForUpgrade() {
		circle.StartCatchUpgradeInput();
	}
	
	public void EndSelectSectorForUpgrade(int id) {
		circle.StopCatchUpgradeInput();

		circle.Upgrade(id, popupUpgrade.neededLevel, popupUpgrade.neededType);

		popupUpgrade.selectedSector.StopSelectUpgrade();
		AllowSpin();
	}
	#endregion
}
