using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeControl : MonoBehaviour {

	public GameObject Player;
	public GameObject kaiguan;
	private bool up;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (kaiguan.GetComponent<Switch_control>().on && !up)
		{
			Moveup();
			up = true;
		}
		else if(!kaiguan.GetComponent<Switch_control>().on && up)
		{
			Movedown();
			up = false;
		}
	}

	void OnCollisionEnter (Collision other){
		if (other.collider.tag == "Player") {
			Player.SendMessage ("death");
		}
	}

	void Moveup()
	{
		for(int i = 0; i < 2000; i++)
		{
			this.gameObject.transform.position = this.gameObject.transform.position+new Vector3(0,0.001F,0);
		}
	}
	void Movedown()
	{
		for(int i = 0; i < 2000; i++)
		{
			this.gameObject.transform.position = this.gameObject.transform.position + new Vector3(0, -0.001F, 0);
		}
	}
}
