using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Realm3 : MonoBehaviour {

	public GameObject terrain;
	public Light light;
	public GameObject dust;
	public GameObject terrainPoints;
	public AudioSource music;

	void OnEnable() {
		var color = Color.black;
		if (Game.main.bossMode) {
			RenderSettings.fogDensity = 0.04f;
			RenderSettings.fogMode = FogMode.Exponential;
		} else {
			music.volume = 1f;
			RenderSettings.fogDensity = 0.0075f;
			RenderSettings.fogMode = FogMode.ExponentialSquared;
		}

		RenderSettings.fogColor = color;

		RenderSettings.fog = true;
		Camera.main.backgroundColor = color;
		Camera.main.farClipPlane = 200f;
		terrain.SetActive (true);
		terrainPoints.SetActive (true);
		RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
		RenderSettings.ambientLight = Color.white;
		light.enabled = false;
		dust.SetActive (false);

	}

	void OnDisable() {
		terrain.SetActive (false);
		terrainPoints.SetActive (false);
		light.enabled = true;
		if (!Game.main.bossMode && music != null) {
			music.volume = 0f;
		}
	}
}
