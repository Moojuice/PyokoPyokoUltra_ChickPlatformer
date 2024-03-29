﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject player;
	public GameCamera cam;
	private GameObject currentPlayer;
	private Vector3 checkpoint;
	
	public static int levelCount = 2;
	public static int currentLevel = 1;
	
	// Use this for initialization
	void Start () {
		cam = GetComponent<GameCamera>();
		
		if (GameObject.FindGameObjectWithTag("Spawn")) {
			checkpoint = GameObject.FindGameObjectWithTag ("Spawn").transform.position;
		}
		SpawnPlayer (checkpoint);
	}
	
	private void SpawnPlayer(Vector3 spawnPos) {
		currentPlayer = Instantiate (player, spawnPos, Quaternion.identity) as GameObject;
		cam.SetTarget(currentPlayer.transform);
	}
	
	private void Update() {
		if (!currentPlayer) {
			if (Input.GetButtonDown ("Respawn")) {
				SpawnPlayer (checkpoint);
			}
		}
	}
	public void SetCheckpoint (Vector3 cp) {
		checkpoint = cp;
	}
	public void EndLevel() {
		if (currentLevel < levelCount) {
			currentLevel++;
			Application.LoadLevel ("Level " + currentLevel);
		}
		else {
			Debug.Log ("Game Over");
		}
	}
}
