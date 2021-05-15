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
	[Header("Refs"), Space]
	[SerializeField] ProgressBarSector[] sectors;

	public void Init() {
		for (int i = 0; i < sectors.Length; ++i) {
			sectors[i].Init(i);
		}
	}
}
