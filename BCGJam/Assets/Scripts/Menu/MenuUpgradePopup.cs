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

	public void RandomUpgrade() {
		MenuManager.HideTopMenu();
		GameManager.Instance.game.UpgradeRandom();
		onSelectUpgradeEvent?.Invoke();
	}
}
