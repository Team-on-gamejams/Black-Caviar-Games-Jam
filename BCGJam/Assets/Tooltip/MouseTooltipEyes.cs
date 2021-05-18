using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class MouseTooltipEyes : MonoBehaviour {
	public bool IsShowed => isShowed;

	[Header("Refs"), Space]
	[SerializeField] RectTransform rt;
	[SerializeField] Canvas canvas;
	[SerializeField] CanvasGroup cg;
	[SerializeField] TextMeshProUGUI textField;
	[SerializeField] LayoutElement childLayoutElement;
	[SerializeField] Transform parent;

	bool isShowed;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rt)
			rt = GetComponent<RectTransform>();
		if (!canvas)
			canvas = GetComponent<Canvas>();
		if (!cg)
			cg = GetComponent<CanvasGroup>();
	}
#endif

	private void Awake() {
		cg.alpha = 0.0f;
		transform.SetParent(parent.transform);
		transform.localScale = Vector3.one;
		transform.localEulerAngles = Vector3.zero;
		transform.localPosition = Vector3.zero;
	}

	private void Start() {
		enabled = false;
	}

	private void Update() {
		rt.anchorMin = new Vector2(1.0f, 0.0f);
		rt.anchorMax = new Vector2(1.0f, 0.0f);
		rt.pivot = new Vector2(1.0f, 0.0f);
		rt.position = UnityEngine.InputSystem.Mouse.current.position.ReadValue() + new Vector2(-16, 16);

		if (rt.position.x - rt.rect.width <= 0) {
			rt.anchoredPosition += new Vector2(rt.rect.width, 0) + new Vector2(16 + 32, 0);
		}

		if (rt.position.y + rt.rect.height >= Screen.height) {
			rt.anchoredPosition += new Vector2(0, -rt.rect.height) + new Vector2(0, -16 + -32);
		}
	}

	public void SetText(string text) {
		textField.text = text;

		bool needMaxWidth = false;
		int charsInThisLine = 0;

		for (int i = 0; i < text.Length; ++i) {
			if (text[i] == '\n') {
				charsInThisLine = 0;
			}
			else {
				++charsInThisLine;
				if (charsInThisLine > 25) {
					needMaxWidth = true;
					break;
				}
			}
		}

		childLayoutElement.enabled = needMaxWidth;
	}

	public void Show() {
		enabled = isShowed = true;

		canvas.sortingOrder = 1000;

		LeanTween.cancel(gameObject, false);
		LeanTweenEx.ChangeAlpha(cg, 1.0f, 0.1f)
			.setEase(LeanTweenType.easeInOutQuad)
			.setDelay(0.2f);
	}

	public void Hide() {
		isShowed = false;

		LeanTween.cancel(gameObject, false);
		LeanTweenEx.ChangeAlpha(cg, 0.0f, 0.1f)
		.setEase(LeanTweenType.easeInOutQuad)
		.setOnComplete(() => {
			enabled = false;
		});
	}
}
