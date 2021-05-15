using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class ProgressBarSector : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] Image fillImage;
	[SerializeField] Image fillHalfImage;
	[SerializeField] Image borderImage;

	bool isFilled;
	bool isFilledHalf;

	public void Init(int id) {
		isFilledHalf = isFilled = false;
		fillImage.fillAmount = 0.0f;
		fillHalfImage.fillAmount = 0.0f;
	}

	public void Fill(float t, float delay) {
		if (isFilled)
			return;
		isFilled = true;

		UpdateVisual(t, delay);
	}

	public void UnFill(float t, float delay) {
		if (!isFilled)
			return;
		isFilled = false;

		UpdateVisual(t, delay);
	}

	public void FillHalf(float t) {
		if (isFilledHalf)
			return;
		isFilledHalf = true;

		UpdateVisual(t, 0);
	}

	public void UnFillHalf(float t) {
		if (!isFilledHalf)
			return;
		isFilledHalf = false;

		UpdateVisual(t, 0);
	}

	void UpdateVisual(float t, float delay) {
		LeanTween.cancel(gameObject, false);
		
		if (isFilledHalf) {
			if (isFilled) {
				LeanTween.value(gameObject, fillImage.fillAmount, 0.0f, t)
				.setDelay(delay)
				.setOnUpdate((float val) => {
					fillImage.fillAmount = val;
				});

				LeanTween.value(gameObject, fillHalfImage.fillAmount, 1.0f, t)
				.setDelay(delay)
				.setOnUpdate((float val) => {
					fillHalfImage.fillAmount = val;
				});
			}
			else {
				LeanTween.value(gameObject, fillImage.fillAmount, 0.0f, t)
				.setDelay(delay)
				.setOnUpdate((float val) => {
					fillImage.fillAmount = val;
				});

				LeanTween.value(gameObject, fillHalfImage.fillAmount, 1.0f, t)
				.setDelay(delay)
				.setOnUpdate((float val) => {
					fillHalfImage.fillAmount = val;
				});
			}
		}
		else {
			if (isFilled) {
				LeanTween.value(gameObject, fillImage.fillAmount, 1.0f, t)
				.setDelay(delay)
				.setOnUpdate((float val) => {
					fillImage.fillAmount = val;
				})
				.setOnComplete(()=> { 
					fillHalfImage.fillAmount = 0.0f;
				});
			}
			else {
				LeanTween.value(gameObject, fillImage.fillAmount, 0.0f, t)
				.setDelay(delay)
				.setOnUpdate((float val) => {
					fillImage.fillAmount = val;
				});

				LeanTween.value(gameObject, fillHalfImage.fillAmount, 0.0f, t)
				.setDelay(delay)
				.setOnUpdate((float val) => {
					fillHalfImage.fillAmount = val;
				});
			}
		}
	}
}
