using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] CanvasGroup cgBeforeSpin;
	[SerializeField] CanvasGroup cgAfterSpin;
	[Space]
	[SerializeField] Button useComboButton;
	[Space]
	[SerializeField] Circle circle;
	[SerializeField] ProgressBar progressBarWin;
	[SerializeField] ProgressBar progressBarLose;
	[SerializeField] ProgressBar progressBarCombo;

	private void Awake() {
		circle.Init();
		progressBarWin.Init();
		progressBarLose.Init();
		progressBarCombo.Init();
	}
}
