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
	[Header("Animation"), Space]
	[SerializeField] Vector2 stayPos = new Vector2(0, -258);
	[SerializeField] Vector2 clickPos = new Vector2(0, -200);

	[Header("Refs"), Space]
	[SerializeField] RectTransform rt;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rt)
			rt = GetComponent<RectTransform>();
	}
#endif

	public void PlaySelectAnimation() {
		LeanTween.cancel(gameObject, false);

		LeanTween.value(gameObject, rt.anchoredPosition, clickPos, 0.2f)
		.setEase(LeanTweenType.easeInOutBack)
		.setOnUpdate((Vector2 val)=> {
			rt.anchoredPosition = val;
		})
		.setOnComplete(() => {
			LeanTween.value(gameObject, rt.anchoredPosition, stayPos, 0.2f)
			.setEase(LeanTweenType.easeOutBack)
			.setOnUpdate((Vector2 val) => {
				rt.anchoredPosition = val;
			})
			.setOnComplete(() => {

			});
		});
	}
}
