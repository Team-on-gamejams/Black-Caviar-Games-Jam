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
	[SerializeField] int pointToMaxUpgrade = 4;
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
	[SerializeField] Image auraYellow;

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
	[Space]
	[SerializeField] Animator tentacle1;
	[SerializeField] Animator tentacle2;

	[Header("Refs - menu"), Space]
	[SerializeField] MenuManager menuManager;
	[SerializeField] MenuWinPopup popupWin;
	[SerializeField] MenuLosePopup popupLose;
	public MenuUpgradePopup popupUpgrade;

	bool isGroupSelectionShowed;
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

		auraYellow.color = auraYellow.color.SetA(0.0f);
	}

	void UseGroup1() {
		onMouseClickGroup1?.Invoke();
	
		circle.AnimateArrowsGroup1();
		circle.AnimateSectorsGroup1();

		circle.GetGroup1Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, out int redCount, out int blueCount, statMultiplierFor2Plus);

		statLose -= blueMod;
		statWin += redMod;
		statCombo += yellowMod;
		lastGreenMod = greenMod;

		if (redMod >= 1)
			redBarRise?.Invoke();
		if (yellowMod >= 1)
			yellowBarRise?.Invoke();

		if (greenMod >= 2)
			GameManager.Instance.game.upgradeSectorIn?.Invoke();
	}

	void UseGroup2() {
		onMouseClickGroup2?.Invoke();
		
		circle.AnimateArrowsGroup2();
		circle.AnimateSectorsGroup2();

		circle.GetGroup2Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, out int redCount, out int blueCount, statMultiplierFor2Plus);

		statLose -= blueMod;
		statWin += redMod;
		statCombo += yellowMod;
		lastGreenMod = greenMod;

		if (redMod >= 1)
			redBarRise?.Invoke();
		if (yellowMod >= 1)
			yellowBarRise?.Invoke();

		if (greenMod >= 2)
			GameManager.Instance.game.upgradeSectorIn?.Invoke();
	}

	void UseGroupBoth() {
		onMouseClickGroupBoth?.Invoke();
		
		circle.AnimateArrowsGroup1();
		circle.AnimateSectorsGroup1();

		circle.AnimateArrowsGroup2();
		circle.AnimateSectorsGroup2();

		circle.GetGroupBothModifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, out int redCount, out int blueCount, statMultiplierFor2Plus);

		statLose -= blueMod;
		statWin += redMod;
		statCombo += yellowMod;
		lastGreenMod = greenMod;

		if (redMod >= 1)
			redBarRise?.Invoke();
		if (yellowMod >= 1)
			yellowBarRise?.Invoke();

		if (greenMod >= 2)
			GameManager.Instance.game.upgradeSectorIn?.Invoke();
	}

	#region Mouse over Callbacks
	public void OnMouseOverGroup1() {
		onMouseOverGroup1?.Invoke();
		
		circle.AnimateArrowsGroup1();
		circle.AnimateSectorsGroup1();

		circle.GetGroup1Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, out int redCount, out int blueCount, statMultiplierFor2Plus);

		if(blueCount > 1) {
			int bonusValue = Mathf.RoundToInt((blueCount - 1) * statMultiplierFor2Plus);
			int origValue = blueMod - bonusValue;

			progressBarLose.UpdateHalfFillValue(statLose - blueMod + statLoseGrowPerTurn, $"+{statLoseGrowPerTurn}{-origValue}{-bonusValue}");
		}
		else if (blueMod != 0) {
			progressBarLose.UpdateHalfFillValue(statLose - blueMod + statLoseGrowPerTurn, $"+{statLoseGrowPerTurn}{-blueMod}");
		}
		else {
			progressBarLose.UpdateHalfFillValue(statLose - blueMod + statLoseGrowPerTurn, $"+{statLoseGrowPerTurn}");
		}

		if (redCount > 1) {
			int bonusValue = Mathf.RoundToInt((redCount - 1) * statMultiplierFor2Plus);
			int origValue = redMod - bonusValue;

			progressBarWin.UpdateHalfFillValue(statWin + redMod, $"+{origValue}+{bonusValue}");
		}
		else {
			progressBarWin.UpdateHalfFillValue(statWin + redMod);
		}

		progressBarCombo.UpdateHalfFillValue(statCombo + yellowMod);
		progressBarUpgrade.UpdateHalfFillValue(greenMod);
	}

	public void OnMouseOverGroup2() {
		onMouseOverGroup2?.Invoke();
		
		circle.AnimateArrowsGroup2();
		circle.AnimateSectorsGroup2();

		circle.GetGroup2Modifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, out int redCount, out int blueCount, statMultiplierFor2Plus);

		if (blueCount > 1) {
			int bonusValue = Mathf.RoundToInt((blueCount - 1) * statMultiplierFor2Plus);
			int origValue = blueMod - bonusValue;

			progressBarLose.UpdateHalfFillValue(statLose - blueMod + statLoseGrowPerTurn, $"+{statLoseGrowPerTurn}{-origValue}{-bonusValue}");
		}
		else if (blueMod != 0) {
			progressBarLose.UpdateHalfFillValue(statLose - blueMod + statLoseGrowPerTurn, $"+{statLoseGrowPerTurn}{-blueMod}");
		}
		else {
			progressBarLose.UpdateHalfFillValue(statLose - blueMod + statLoseGrowPerTurn, $"+{statLoseGrowPerTurn}");
		}

		if (redCount > 1) {
			int bonusValue = Mathf.RoundToInt((redCount - 1) * statMultiplierFor2Plus);
			int origValue = redMod - bonusValue;

			progressBarWin.UpdateHalfFillValue(statWin + redMod, $"+{origValue}+{bonusValue}");
		}
		else {
			progressBarWin.UpdateHalfFillValue(statWin + redMod);
		}

		progressBarCombo.UpdateHalfFillValue(statCombo + yellowMod);
		progressBarUpgrade.UpdateHalfFillValue(greenMod);
	}

	public void OnMouseOverGroupBoth() {
		onMouseOverGroupBoth?.Invoke();
		
		circle.AnimateArrowsGroup1();
		circle.AnimateArrowsGroup2();

		if (isGroupSelectionShowed) {
			circle.AnimateSectorsGroup1();
			circle.AnimateSectorsGroup2();

			circle.GetGroupBothModifiers(out int redMod, out int yellowMod, out int blueMod, out int greenMod, out int redCount, out int blueCount, statMultiplierFor2Plus);

			if (blueCount > 1) {
				int bonusValue = Mathf.RoundToInt((blueCount - 1) * statMultiplierFor2Plus);
				int origValue = blueMod - bonusValue;

				progressBarLose.UpdateHalfFillValue(statLose - blueMod + statLoseGrowPerTurn, $"+{statLoseGrowPerTurn}{-origValue}{-bonusValue}");
			}
			else if (blueMod != 0) {
				progressBarLose.UpdateHalfFillValue(statLose - blueMod + statLoseGrowPerTurn, $"+{statLoseGrowPerTurn}{-blueMod}");
			}
			else {
				progressBarLose.UpdateHalfFillValue(statLose - blueMod + statLoseGrowPerTurn, $"+{statLoseGrowPerTurn}");
			}

			if (redCount > 1) {
				int bonusValue = Mathf.RoundToInt((redCount - 1) * statMultiplierFor2Plus);
				int origValue = redMod - bonusValue;

				progressBarWin.UpdateHalfFillValue(statWin + redMod, $"+{origValue}+{bonusValue}");
			}
			else {
				progressBarWin.UpdateHalfFillValue(statWin + redMod);
			}

			progressBarCombo.UpdateHalfFillValue(statCombo + yellowMod);
			progressBarUpgrade.UpdateHalfFillValue(greenMod);
		}
	}

	public void OnMouseExitGroup1() {
		progressBarLose.UpdateHalfFillValue(statLose + statLoseGrowPerTurn);
		progressBarWin.ClearHalfFillValue();
		progressBarCombo.ClearHalfFillValue();
		progressBarUpgrade.ClearHalfFillValue();

		circle.StopAnimateArrowsGroup1();
		circle.StopAnimateArrowsGroup2();
		circle.StopAnimateSectorsGroup1();
		circle.StopAnimateSectorsGroup2();
	}

	public void OnMouseExitGroup2() {
		progressBarLose.UpdateHalfFillValue(statLose + statLoseGrowPerTurn);
		progressBarWin.ClearHalfFillValue();
		progressBarCombo.ClearHalfFillValue();
		progressBarUpgrade.ClearHalfFillValue();

		circle.StopAnimateArrowsGroup1();
		circle.StopAnimateArrowsGroup2();
		circle.StopAnimateSectorsGroup1();
		circle.StopAnimateSectorsGroup2();
	}

	public void OnMouseExitGroupBoth() {
		if (isGroupSelectionShowed) {
			progressBarLose.UpdateHalfFillValue(statLose + statLoseGrowPerTurn);
			progressBarWin.ClearHalfFillValue();
			progressBarCombo.ClearHalfFillValue();
			progressBarUpgrade.ClearHalfFillValue();
		}

		circle.StopAnimateArrowsGroup1();
		circle.StopAnimateArrowsGroup2();
		circle.StopAnimateSectorsGroup1();
		circle.StopAnimateSectorsGroup2();
	}

	public void OnMouseOverEye() {
		onMouseOverEye?.Invoke();

		tentacle1.SetBool("IsGrab", true);
		tentacle2.SetBool("IsGrab", true);
	}

	public void OnMouseExitEye() {
		onMouseExitEye?.Invoke();

		tentacle1.SetBool("IsGrab", false);
		tentacle2.SetBool("IsGrab", false);

	}
	#endregion

	#region Button Callbacks
	public void OnClickSpin() {
		onMouseClickEye?.Invoke();
		onSpinStart?.Invoke();

		buttonSpin.interactable = false;

		filledBars = 0;

		if(tentacle1.GetCurrentAnimatorStateInfo(0) && tentacle1.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
			tentacle1.SetTrigger("IsSpin");
			tentacle2.SetTrigger("IsSpin");
			tentacle1.SetBool("IsGrab", false);
			tentacle2.SetBool("IsGrab", false);

			LeanTween.delayedCall(gameObject, 0.02f, () => {
				circle.Spin();
			});
		}
		else {
			LeanTween.delayedCall(gameObject, 0.5f, () => {
				tentacle1.SetTrigger("IsSpin");
				tentacle2.SetTrigger("IsSpin");
				tentacle1.SetBool("IsGrab", false);
				tentacle2.SetBool("IsGrab", false);

				LeanTween.delayedCall(gameObject, 0.02f, () => {
					circle.Spin();
				});
			});
		}
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
		LeanTweenEx.ChangeAlpha(auraYellow, 0.0f, 0.2f);

		if (isGroupSelectionShowed) {
			OnSelectAnyGroup();
			UseGroupBoth();
			AfterUseGroup();

			progressBarCombo.UpdateValueNoCallback(statCombo = 0);
		}
	}

	void AfterUseGroup() {
		if (statWin >= pointToWin) {
			statWin = pointToWin;
		}
		if (statWin < 0) {
			statWin = 0;
		}

		statLose += statLoseGrowPerTurn;

		++statTurnsToIncreaseGrow;
		if(statTurnsToIncreaseGrow > turnsToIncreaseGrow) {
			blueBarRise?.Invoke();
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

		if(lastGreenMod > pointToMaxUpgrade) {
			lastGreenMod = pointToMaxUpgrade;
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
		useComboButton.image.raycastTarget = useComboButton.interactable = false;

		isGroupSelectionShowed = false;

		progressBarCombo.ClearHalfFillValue();
		progressBarLose.ClearHalfFillValue();
		progressBarWin.ClearHalfFillValue();
	}
	#endregion

	#region Callbacks
	void OnSpinEnd() {
		onSpinEnd?.Invoke();

		foreach (var b in buttonsSelectGroup) {
			b.image.raycastTarget = b.interactable = true;
		}

		if (statCombo == pointToCombo && useComboButton.image.raycastTarget == false) {
			yellowComboReady?.Invoke();

			useComboButton.image.raycastTarget = useComboButton.interactable = true;

			LeanTweenEx.ChangeAlpha(auraYellow, 1.0f, 0.2f);
		}

		isGroupSelectionShowed = true;
		progressBarLose.UpdateHalfFillValue(statLose + statLoseGrowPerTurn);
		progressBarLoseGrow.UpdateHalfFillValue(statTurnsToIncreaseGrow + 1);
	}

	void OnAllEffectAnimationDone() {
		circle.StopAnimateArrowsGroup1();
		circle.StopAnimateArrowsGroup2();
		circle.StopAnimateSectorsGroup1();
		circle.StopAnimateSectorsGroup2();

		LeanTween.delayedCall(gameObject, 0.5f, () => {
			if (statWin >= pointToWin) {
				menuManager.Show(popupWin, false);
				winEvent?.Invoke();
			}
			else if (statLose >= pointToLose) {
				menuManager.Show(popupLose, false);
				loseEvent?.Invoke();
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
		//if (statCombo == pointToCombo) {
		//	if(useComboButton.image.raycastTarget == false)
		//		yellowComboReady?.Invoke();

		//	useComboButton.image.raycastTarget = useComboButton.interactable = true;

		//	LeanTweenEx.ChangeAlpha(auraYellow, 1.0f, 0.2f);
		//}
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

	#region Audio callbacks
	public Action onMouseOverGroup1;
	public Action onMouseOverGroup2;
	public Action onMouseOverGroupBoth;
	public Action onMouseOverEye;

	public Action onMouseExitEye;

	public Action onMouseClickGroup1;
	public Action onMouseClickGroup2;
	public Action onMouseClickGroupBoth;
	public Action onMouseClickEye;

	public Action onSpinStart;
	public Action onSpinEnd;

	public Action upgradeMagnet;
	public Action upgradeSectorIn;
	public Action upgradeSectorOut;

	public Action blueBarRise;
	public Action redBarRise;
	public Action yellowBarRise;

	public Action yellowComboReady;

	public Action winEvent;
	public Action loseEvent;

	public int GetRedFill() {
		return statWin;
	}
	#endregion
}
