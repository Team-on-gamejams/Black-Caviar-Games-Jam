using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CircleArrow : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] RectTransform rt;
	[SerializeField] Image activeImage;
	[SerializeField] Image inactiveImage;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rt)
			rt = GetComponent<RectTransform>();
		if (!activeImage)
			activeImage = GetComponentInChildren<Image>();
		if (!inactiveImage)
			inactiveImage = GetComponent<Image>();
	}
#endif

	public void Init() {
		activeImage.color = activeImage.color.SetA(0.0f);
	}

	public void PlaySelectAnimation() {
		LeanTween.cancel(activeImage.gameObject, false);

		LeanTweenEx.ChangeAlpha(activeImage, 1.0f, 0.2f)
		.setEase(LeanTweenType.easeInOutSine);
	}

	public void StopSelectAnimation() {
		LeanTween.cancel(activeImage.gameObject, false);

		LeanTweenEx.ChangeAlpha(activeImage, 0.0f, 0.2f)
		.setEase(LeanTweenType.easeInOutSine);
	}
}
