using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

		public GameObject Player;
		private Vector3 Distant;


		// Use this for initialization
		void Start () {
			Distant = transform.position - Player.transform.position;
		}

		// Update is called once per frame
		void Update () {
			transform.position = Player.transform.position + Distant;
		}
	}
