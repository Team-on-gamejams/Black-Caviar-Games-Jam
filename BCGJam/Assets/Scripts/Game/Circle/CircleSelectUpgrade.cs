using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CircleSelectUpgrade : MonoBehaviour {
	[NonSerialized] public Transform anchorTo;

	[Header("Data"), Space]
	[SerializeField] float scaleMod = 0.6f;

	[Header("Refs"), Space]
	[SerializeField] Image backImg;
	[SerializeField] Transform circleCenter;

	bool isSelectingUpdate = false;

	public void StartSelectUpgrade() {
		isSelectingUpdate = true;
		backImg.GetComponent<Selectable>().interactable = backImg.raycastTarget = false;

		transform.SetParent(backImg.canvas.transform.Find("SelectUpgradeParent"));
	}

	public void StopSelectUpgrade() {
		isSelectingUpdate = false;
		backImg.GetComponent<Selectable>().interactable = backImg.raycastTarget = true;

		anchorTo = null;

		transform.localScale = Vector3.one;

		transform.position += new Vector3(10000, 0);
	}

	private void Update() {
		if (isSelectingUpdate) {
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * scaleMod, Time.deltaTime * 5);

			transform.position = Vector3.Lerp(transform.position, anchorTo != null ? TemplateGameManager.Instance.Camera.WorldToScreenPoint(anchorTo.position) : (Vector3)Mouse.current.position.ReadValue(), Time.deltaTime * 5);

			Vector3 vectorToTarget = TemplateGameManager.Instance.Camera.WorldToScreenPoint(circleCenter.position) - transform.position;
			float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 180;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * 5);
		}
	}
}
