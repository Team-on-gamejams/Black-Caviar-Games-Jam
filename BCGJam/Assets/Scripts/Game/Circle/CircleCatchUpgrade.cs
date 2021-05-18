using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CircleCatchUpgrade : MonoBehaviour {
	[NonSerialized] public bool isCatchInput;
	public CircleSector sector;

	public void OnClick() {
		if (isCatchInput) {
			GameManager.Instance.game.upgradeSectorOut?.Invoke();
			GameManager.Instance.game.EndSelectSectorForUpgrade(GetComponentInParent<CircleSector>().Id);
		}
	}

	public void OnEnter() {
		if (!isCatchInput) {
			//TODO:
			Debug.Log("Show popup");
		}
		else {
			GameManager.Instance.game.upgradeMagnet?.Invoke();
			GameManager.Instance.game.popupUpgrade.selectedSector.anchorTo = transform;
		}
	}

	public void OnExit() {
		//TODO:
		Debug.Log("Hide popup");

		if(isCatchInput && GameManager.Instance.game.popupUpgrade.selectedSector.anchorTo == transform) {
			GameManager.Instance.game.popupUpgrade.selectedSector.anchorTo = null;
		}
	}
}
