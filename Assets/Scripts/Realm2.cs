using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Realm2 : MonoBehaviour {

	public GameObject terrain;
	public GameObject dust;
	public AudioSource music;

	void OnEnable() {
		var color = Util.HexRGBAColor ("303A4900");
		if (Game.main.bossMode) {
			color = Color.black;
			RenderSettings.fogDensity = 0.04f;
			RenderSettings.fogMode = FogMode.Exponential;
		} else {
			RenderSettings.fogMode = FogMode.Linear;
			RenderSettings.fogDensity = 0.02f;
			music.volume = 1f;
		}
		RenderSettings.fogColor = color;

		RenderSettings.fogEndDistance = 200;
		RenderSettings.fog = true;
		Camera.main.backgroundColor = color;
		Camera.main.farClipPlane = 512f;
		terrain.SetActive (true);
		RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
		RenderSettings.ambientLight = Color.white;
		dust.SetActive (true);
	}

	void OnDisable() {
		terrain.SetActive (false);
		if (!Game.main.bossMode && music != null) {
			music.volume = 0f;
		}
	}
}
