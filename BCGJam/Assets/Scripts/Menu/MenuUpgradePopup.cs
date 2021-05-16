using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class MenuUpgradePopup : PopupMenuBase {
	public Action onSelectUpgradeEvent;

	int level;

	public void InitWindow(int _level) {
		level = _level;
	}

	public void RandomUpgrade() {
		MenuManager.HideTopMenu();
		GameManager.Instance.game.UpgradeRandom(level);
		onSelectUpgradeEvent?.Invoke();
	}
}
