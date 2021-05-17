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

public class EyeFollowMouse : MonoBehaviour {
	[Header("Data"), Space]
	[SerializeField] Transform center;
	[SerializeField] float radius = 6.0f;
	[SerializeField] bool keepZeroRotation = true;

	void Update() {
		Vector3 mousePosScreen = Mouse.current.position.ReadValue();
		//Vector3 mousePosWorld = TemplateGameManager.Instance.Camera.ScreenToWorldPoint(mousePosLocal).SetZ(0.0f);
		Vector3 offset = mousePosScreen - center.transform.position;

		transform.position = center.position + offset.normalized * Mathf.Lerp(0, radius, offset.magnitude / 500f);

		if(keepZeroRotation)
			transform.eulerAngles = Vector3.zero;
	}
}
