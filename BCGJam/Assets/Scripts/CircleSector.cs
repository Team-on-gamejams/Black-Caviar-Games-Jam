using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CircleSector : MonoBehaviour {
	public int Id { get; private set; }

	[Header("Refs"), Space]
	[SerializeField] Image sectorContentImage; 
	[SerializeField] TextMeshProUGUI debugTextField;


	public void Init(int id) {
		Id = id;
		debugTextField.text = id.ToString();
	}
}
