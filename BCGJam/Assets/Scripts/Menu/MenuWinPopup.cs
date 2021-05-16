using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class MenuWinPopup : PopupMenuBase {
	public void PlayAgain() {
		MenuManager.HideTopMenu();
		GameManager.Instance.game.Init();
	}

	public void Exit() {
		QuitGame.QuitApp();
	}
}
