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
	[SerializeField] Image borderImage;

	public void Init(int id) {
		fillImage.fillAmount = 0.0f;
	}
}
