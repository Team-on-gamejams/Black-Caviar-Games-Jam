using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Circle : MonoBehaviour {
	[Header("Balance"), Space]
	[SerializeField] int[] arrowsIdGroup1;
	[SerializeField] int[] arrowsIdGroup2;

	[Header("Refs"), Space]
	[SerializeField] CircleSector[] sectors;

	public void Init() {
		for (int i = 0; i < sectors.Length; ++i) {
			sectors[i].Init(i);
		}
	}
}
