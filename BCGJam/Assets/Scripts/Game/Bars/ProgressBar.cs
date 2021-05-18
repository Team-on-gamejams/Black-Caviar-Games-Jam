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

	[Header("Visual"), Space]
	[SerializeField] float fillTime = 0.1f;

	[Header("Refs"), Space]
	[SerializeField] ProgressBarSector[] sectors;
	[SerializeField] TextMeshProUGUI halfFillTextField;

	int currValue;


	public void Init() {
		for (int i = 0; i < sectors.Length; ++i) {
			sectors[i].Init(i);
		}

		currValue = 0;
		if(halfFillTextField)
			halfFillTextField.text = "";
	}

	public void UpdateHalfFillValue(int newValue, string textOverride = null) {
		if (newValue > sectors.Length)
			newValue = sectors.Length;
		if (newValue < 0)
			newValue = 0;

		float delta = newValue - currValue;

		if (delta == 0) {
			if (halfFillTextField) {
				if (textOverride != null)
					halfFillTextField.text = textOverride;
				else
					halfFillTextField.text = "";
			}

			for (int i = 0; i < sectors.Length; ++i) {
				sectors[i].UnFillHalf(fillTime);
			}
		}
		else if (delta > 0) {
			if (halfFillTextField) {
				if(textOverride != null)
					halfFillTextField.text = textOverride;
				else
					halfFillTextField.text = $"+{delta}";
			}

			for (int i = 0; i < sectors.Length; ++i) {
				if(currValue <= i && i < newValue)
					sectors[i].FillHalf(fillTime);
				else
					sectors[i].UnFillHalf(fillTime);
			}
		}
		else if (delta < 0) {
			if (halfFillTextField) {
				if (textOverride != null)
					halfFillTextField.text = textOverride;
				else
					halfFillTextField.text = $"{delta}";
			}

			for (int i = 0; i < sectors.Length; ++i) {
				if (newValue <= i && i < currValue)
					sectors[i].FillHalf(fillTime);
				else
					sectors[i].UnFillHalf(fillTime);
			}
		}
	}

	public void ClearHalfFillValue() {
		UpdateHalfFillValue(currValue);
	}

	public void UpdateValueNoCallback(int newValue) {
		float delta = Mathf.Abs(currValue - newValue);

		if (delta == 0) {
			return;
		}

		if (newValue - currValue > 0) {
			for (int i = currValue; i < newValue; ++i) {
				sectors[i].Fill(fillTime, (i - currValue) * fillTime);
			}
		}
		else {
			for (int i = currValue - 1; i >= newValue; --i) {
				sectors[i].UnFill(fillTime, (currValue - i - 1) * fillTime);
			}
		}

		currValue = newValue;
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
				sectors[i].UnFill(fillTime, (currValue - i - 1) * fillTime);
			}
		}
		
		currValue = newValue;

		LeanTween.delayedCall(gameObject, fillTime * delta, onValueUpdated);
	}
}
