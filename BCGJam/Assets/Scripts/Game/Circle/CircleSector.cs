using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CircleSector : MonoBehaviour {
	public int Id { get; private set; }

	[Header("Data"), Space]
	[SerializeField] int initialLevel = 0;
	[SerializeField] SectorType initialType = SectorType.Empty;


	[Header("Refs"), Space]
	[SerializeField] Image sectorContentImage; 
	[SerializeField] LevelData[] levels;
	[SerializeField] TypeData[] types;
	[SerializeField] AuraData[] auras;

	int currLevel;
	SectorType currType;
	public void Init(int id) {
		Id = id;

		currLevel = initialLevel;
		currType = initialType;

		foreach (var aura in auras) {
			aura.DeactivateForce();
		}

		RecalcVisuals();
	}

	public void AddModifiers(ref int redMod, ref int yellowMod, ref int blueMod, ref int greenMod, ref int redCount, ref int blueCount) {
		int mod;
		switch (currLevel) {
			case 0:
				mod = 1;
				break;
			case 1:
				mod = 2;
				break;
			case 2:
				mod = 3;
				break;
			default:
				mod = 1;
				break;
		}

		switch (currType) {
			case SectorType.Red:
				redMod += mod;
				++redCount;
				break;
			case SectorType.Blue:
				blueMod += mod;
				++blueCount;
				break;
			case SectorType.Yellow:
				yellowMod += mod;
				break;
			case SectorType.Green:
				greenMod += mod;
				break;
		}
	}

	public void Upgrade(int level, SectorType type) {
		currLevel = level;
		currType = type;

		RecalcVisuals();
	}

	public void ShowAura() {
		foreach (var aura in auras) {
			if(aura.level == currLevel && aura.type == currType)
				aura.Activate();
			else
				aura.Deactivate();
		}
	}

	public void HideAura() {
		foreach (var aura in auras) {
			aura.Deactivate();
		}
	}

	void RecalcVisuals() {
		foreach (var level in levels) {
			if (level.level == currLevel)
				level.Activate();
			else
				level.Deactivate();
		}

		foreach (var type in types) {
			if (type.type == currType)
				type.Activate();
			else
				type.Deactivate();
		}
	}

	public enum SectorType : byte { Empty, Red, Blue, Yellow, Green }
	[Serializable]
	public struct TypeData {
		public SectorType type;
		public Image[] eyeBorder;
		public Image[] eye;

		public void Activate() {
			foreach (var i in eyeBorder) 
				i.gameObject.SetActive(true);
			foreach (var i in eye)
				i.gameObject.SetActive(true);
		}

		public void Deactivate() {
			foreach (var i in eyeBorder)
				i.gameObject.SetActive(false);
			foreach (var i in eye)
				i.gameObject.SetActive(false);
		}
	}

	[Serializable]
	public struct LevelData {
		public int level;
		public Transform sector;

		public void Activate() {
			sector.gameObject.SetActive(true);
		}

		public void Deactivate() {
			sector.gameObject.SetActive(false);
		}
	}

	[Serializable]
	public struct AuraData {
		public int level;
		public SectorType type;
		public Transform aura;

		public void Activate() {
			LeanTween.cancel(aura.gameObject);
			LeanTweenEx.ChangeAlpha(aura.GetComponent<Image>(), 1.0f, 0.2f);
		}

		public void Deactivate() {
			LeanTween.cancel(aura.gameObject);
			LeanTweenEx.ChangeAlpha(aura.GetComponent<Image>(), 0.0f, 0.2f);
		}

		public void DeactivateForce() {
			LeanTween.cancel(aura.gameObject);
			aura.GetComponent<Image>().color = Color.white.SetA(0.0f);
		}
	}
}
