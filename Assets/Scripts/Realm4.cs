using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Realm4 : MonoBehaviour {

	public GameObject terrainPoints;
	public Terrain terrain;
	public Material terrainMaterial;
	public GameObject light_;
	public GameObject dust;
	public AudioSource music;

	public GameObject bowArrow;

	private Material oldMaterial;


	void OnEnable() {
		RenderSettings.fog = false;
		Camera.main.backgroundColor = Color.black;
		Camera.main.farClipPlane = 128f;
		terrainPoints.SetActive (true);
		oldMaterial = terrain.materialTemplate;
		terrain.materialTemplate = terrainMaterial;
		terrain.enabled = true;
		RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
		RenderSettings.ambientLight = Color.white;
		light_.SetActive (false);
		dust.SetActive (false);
		bowArrow.SetActive (false);
		if (!Game.main.bossMode) {
			music.volume = 1f;
		} 
	}

	void OnDisable() {
		bowArrow.SetActive (true);
		light_.SetActive (true);
		terrainPoints.SetActive (false);
		terrain.enabled = false;
		terrain.materialTemplate = oldMaterial;
		if (Game.main != null && !Game.main.bossMode && music != null) {
			music.volume = 0f;
		}
	}
}
