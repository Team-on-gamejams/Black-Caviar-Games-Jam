using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class MenuUpgradePopup : PopupMenuBase {
	public Action onStartSelectUpgradeEvent;
	public Action onSkipUpgradeEvent;

	[NonSerialized] public int neededLevel;
	[NonSerialized] public CircleSector.SectorType neededType;
	[NonSerialized] public CircleSelectUpgrade selectedSector;

	[Header("Refs"), Space] 
	[SerializeField] CircleSector[] sectorsToSelect;
	[SerializeField] Transform[] sectorAnchors;

	CircleSector.SectorType[] allTypes;
	CircleSector.SectorType[] types;

	public void InitWindow(int _level) {
		neededLevel = _level;
		types = FillTypes();

		for (int i = 0; i < sectorsToSelect.Length; ++i) {
			sectorsToSelect[i].Upgrade(neededLevel, types[i]);
		}

		for (int i = 0; i < sectorAnchors.Length; ++i) {
			sectorsToSelect[i].transform.SetParent(sectorAnchors[i]);
			sectorsToSelect[i].transform.localPosition = Vector3.zero;
			sectorsToSelect[i].transform.localEulerAngles = Vector3.zero;
		}
	}

	public void OnSelectUpgrade(int id) {
		neededType = types[id];

		selectedSector = sectorsToSelect[id].GetComponent<CircleSelectUpgrade>();
		selectedSector.StartSelectUpgrade();

		MenuManager.HideTopMenu();
		onStartSelectUpgradeEvent?.Invoke();
	}

	public void OnSkipUpgrade() {
		MenuManager.HideTopMenu();
		onSkipUpgradeEvent?.Invoke();
	}

	public void ToggleShow() {
		if (isShowed) {
			isShowed = false;
			LeanTween.move(popupTransform.gameObject, closePos.position, animTime)
				.setEase(easeOut);
		}
		else {
			isShowed = true;
			LeanTween.move(popupTransform.gameObject, openPos.position, animTime)
				.setEase(easeIn)
				.setDelay(Time.deltaTime);
		}
	}

	CircleSector.SectorType[] FillTypes() {
		if(allTypes == null || allTypes.Length == 0) {
			allTypes = new CircleSector.SectorType[4];
			allTypes[0] = CircleSector.SectorType.Yellow;
			allTypes[1] = CircleSector.SectorType.Green;
			allTypes[2] = CircleSector.SectorType.Blue;
			allTypes[3] = CircleSector.SectorType.Red;
		}
		else {
			allTypes.Shuffle();
		}

		CircleSector.SectorType[] types = new CircleSector.SectorType[sectorsToSelect.Length];
		for (int i = 0; i < types.Length; ++i) {
			types[i] = allTypes[i];
		}

		return types;
	}
}
