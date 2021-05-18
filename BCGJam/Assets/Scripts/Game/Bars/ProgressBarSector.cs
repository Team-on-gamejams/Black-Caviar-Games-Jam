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
	[Space]
	[SerializeField] CanvasGroup eyeCg;

	bool isFilled;
	bool isFilledHalf;

	private void Update() {
		fillHalfImage.color = fillHalfImage.color.SetA(Mathf.PingPong(Time.time, 0.5f));
	}

	public void Init(int id) {
		isFilledHalf = isFilled = false;
		if (fillImage) {
			fillImage.fillAmount = 0.0f;
			fillHalfImage.fillAmount = 0.0f;
		}
		else {
			eyeCg.alpha = 0.0f;
			eyeCg.transform.localScale = Vector3.zero;
		}
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

		float startAlpha = 0;
		Vector3 startScale = Vector3.zero;

		if(eyeCg != null) {
			startAlpha = eyeCg.alpha;
			startScale = eyeCg.transform.localScale;
			t *= 3;
		}

		if (isFilledHalf) {
			if (isFilled) {
				if (fillImage != null) {
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
					LeanTween.value(gameObject, 0.0f, 1.0f, t)
					.setDelay(delay)
					.setEase(LeanTweenType.easeOutBack)
					.setOnUpdate((float val) => {
						eyeCg.alpha = Mathf.Lerp(startAlpha, 0.5f, val);
						eyeCg.transform.localScale = Vector3.Lerp(startScale, Vector3.one, val);
					});
				}
			}
			else {
				if (fillImage != null) {
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
					LeanTween.value(gameObject, 0.0f, 1.0f, t)
					.setDelay(delay)
					.setEase(LeanTweenType.easeOutBack)
					.setOnUpdate((float val) => {
						eyeCg.alpha = Mathf.Lerp(startAlpha, 0.5f, val);
						eyeCg.transform.localScale = Vector3.Lerp(startScale, Vector3.one, val);
					});
				}
			}
		}
		else {
			if (fillImage != null) {
				if (isFilled) {
					LeanTween.value(gameObject, fillImage.fillAmount, 1.0f, t)
					.setDelay(delay)
					.setOnUpdate((float val) => {
						fillImage.fillAmount = val;
					})
					.setOnComplete(() => {
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
			else {
				if (isFilled) {
					LeanTween.value(gameObject, 0.0f, 1.0f, t)
					.setDelay(delay)
					.setEase(LeanTweenType.easeOutBack)
					.setOnUpdate((float val) => {
						eyeCg.alpha = Mathf.Lerp(startAlpha, 1, val);
						eyeCg.transform.localScale = Vector3.Lerp(startScale, Vector3.one, val);
					});
				}
				else {
					LeanTween.value(gameObject, 0.0f, 1.0f, t)
					.setDelay(delay)
					.setEase(LeanTweenType.easeInBack)
					.setOnUpdate((float val) => {
						eyeCg.alpha = Mathf.Lerp(startAlpha, 0, val);
						eyeCg.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, val);
					});
				}
			}
		}
	}
}
