using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Realm1 : MonoBehaviour {

	public Terrain terrain;
	public GameObject dust;
	public AudioSource music;

	void OnEnable() {
		var color = Util.HexRGBAColor ("314D7900");
		if (Game.main != null && Game.main.bossMode) {
			color = Color.black;
			RenderSettings.fogDensity = 0.04f;
		} else {
			RenderSettings.fogDensity = 0.02f;
			if (music != null) {
				music.volume = 1f;
			}
		}
		RenderSettings.fogColor = color;
		RenderSettings.fogMode = FogMode.Exponential;
		RenderSettings.fog = true;
		if (Camera.main != null) {
			Camera.main.backgroundColor = color;
			Camera.main.farClipPlane = 512f;
		}
		terrain.enabled = true;
		RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
		dust.SetActive (true);
	}

	void OnDisable() {
		terrain.enabled = false;
		if (Game.main != null && !Game.main.bossMode && music != null) {
			music.volume = 0f;
		}
	}

}
