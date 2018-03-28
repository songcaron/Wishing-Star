using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

	public float speed;
	private float waitTime;
	public float startWaitTime;
	public Transform moveSpots;
	public float minX;
	public float maxX;
	public float minY;
	public float maxY;


	void Start(){
		waitTime = startWaitTime;
		moveSpots.position = new Vector3 (Random.Range (minX, maxX), Random.Range (minY, maxY));
	}

	void Update(){
		transform.position = Vector3.MoveTowards (transform.position, moveSpots.position, speed * Time.deltaTime);

		if(Vector3.Distance(transform.position, moveSpots.position) < 0.2f){
			if(waitTime <= 0){
				moveSpots.position = new Vector3 (Random.Range (minX, maxX), Random.Range (minY, maxY));

				waitTime = startWaitTime;
			}
			else{
				waitTime -= Time.deltaTime;
			}
		}

	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "jumptrigger") {
			Debug.Log ("Enemy triggered jump ability");
		}
	}
}