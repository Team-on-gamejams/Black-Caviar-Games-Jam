using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class ProgressBar : MonoBehaviour {
	public Action onValueUpdated;

	int currValue;

	[Header("Visual"), Space]
	[SerializeField] float fillTime = 0.1f;

	[Header("Refs"), Space]
	[SerializeField] ProgressBarSector[] sectors;

	public void Init() {
		for (int i = 0; i < sectors.Length; ++i) {
			sectors[i].Init(i);
		}

		currValue = 0;
	}

	public void UpdateValue(int newValue) {
		float delta = Mathf.Abs(currValue - newValue);

		if(delta == 0) {
			LeanTweenEx.InvokeNextFrame(gameObject, onValueUpdated);
			return;
		}

		if (newValue - currValue > 0) {
			for (int i = currValue; i < newValue; ++i) {
				sectors[i].Fill(fillTime, (i - currValue) * fillTime);
			}
		}
		else {
			for (int i = currValue - 1; i >= newValue; --i) {
				sectors[i].UnFill(fillTime, (i - currValue) * fillTime);
			}
		}
		
		currValue = newValue;

		LeanTween.delayedCall(gameObject, fillTime * delta, onValueUpdated);
	}
}
