using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class SoundsController : MonoBehaviour {
	[Header("Audio"), Space]
	[SerializeField] AudioSource MouseOverBronze;
	[SerializeField] AudioSource MouseOverRedBronze;
	[SerializeField] AudioSource MouseOverMulti;
	[SerializeField] AudioSource MouseOverEye;
	[Space]
	[SerializeField] AudioSource MouseClickBronze;
	[SerializeField] AudioSource MouseClickRedBronze;
	[SerializeField] AudioSource MouseClickMulti;
	[SerializeField] AudioSource MouseClickEye;
	[Space]
	[SerializeField] AudioSource EventSpinInProcess;
	[SerializeField] AudioSource EventSpinStop;
	[Space]
	[SerializeField] AudioSource EventUpgradeSectorIn;
	[SerializeField] AudioSource EventUpgradeSectorOut;
	[SerializeField] AudioSource EventUpgradeMagnet;

	[Space]
	[Space]
	[SerializeField] AudioSource[] EventBlueBarRise;
	[SerializeField] AudioSource[] EventRedBarRise;
	[Space]
	[SerializeField] AudioSource EventYellowRise;
	[SerializeField] AudioSource EventYellowComboReady;
	[Space]
	[SerializeField] AudioSource EventWin;
	[SerializeField] AudioSource EventLose;

	[Header("Refs"), Space]
	[SerializeField] Game game;
	[SerializeField] MusicLooper looper;

	private void Awake() {
		game.onMouseOverGroup1 += MouseOverBronze.Play;
		game.onMouseOverGroup2 += MouseOverRedBronze.Play;
		game.onMouseOverGroupBoth += MouseOverMulti.Play;
		game.onMouseOverEye += () => {
			MouseOverEye.volume = 1.0f;
			MouseOverEye.Play();
			AudioManager.Instance.ChangeASVolume(MouseOverEye, 0.0f, 0.5f);
		}; 

		game.onMouseClickGroup1 += MouseClickBronze.Play;
		game.onMouseClickGroup2 += MouseClickRedBronze.Play;
		game.onMouseClickGroupBoth += MouseClickMulti.Play;

		game.onMouseClickEye += () => {
			MouseClickEye.Play();
		};

		game.onSpinStart += ()=> {
			EventSpinInProcess.Play();
		};
		game.onSpinEnd = () => {
			EventSpinInProcess.Stop();
			EventSpinStop.Play();
		};

		game.upgradeSectorIn += EventUpgradeSectorIn.Play;
		game.upgradeSectorOut += EventUpgradeSectorOut.Play;
		game.upgradeMagnet += EventUpgradeMagnet.Play;

		game.blueBarRise += ()=> EventBlueBarRise.Random().Play();
		game.redBarRise += ()=> EventRedBarRise.Random().Play();
		game.yellowBarRise += EventYellowRise.Play;

		game.yellowComboReady += EventYellowComboReady.Play;

		game.winEvent += () => {
			looper.StopPlay(0.0f);
			EventWin.Play();
		};
		game.loseEvent += () => {
			looper.StopPlay(2.0f);
			EventLose.Play();
		};
	}
}
