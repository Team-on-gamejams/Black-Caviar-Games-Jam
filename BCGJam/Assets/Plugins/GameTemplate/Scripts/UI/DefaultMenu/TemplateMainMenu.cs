using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateMainMenu : MenuBase {
	[SerializeField] int sceneIdToLoad = 1;

	public void SetRu() {
		Polyglot.Localization.Instance.SelectedLanguage= Polyglot.Language.Russian;
		SceneLoader.Instance.LoadScene(sceneIdToLoad, true, true);

	}

	public void SetEng() {
		Polyglot.Localization.Instance.SelectedLanguage = Polyglot.Language.English;
		SceneLoader.Instance.LoadScene(sceneIdToLoad, true, true);
	}

	public void Play() {
		SceneLoader.Instance.LoadScene(sceneIdToLoad, true, true);

	}

	public void Load() {
		SceneLoader.Instance.LoadScene(sceneIdToLoad, false, false);
	}
}
