using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign_control : MonoBehaviour {
    public Text SignText;
    public GameObject chr;
    private float dist;
    private bool near;
	public string sign;
    // Use this for initialization
    void Start () {
        NearJudge();
        DialogueEdit();
    }
	
	// Update is called once per frame
	void Update () {
        NearJudge();
        DialogueEdit();
    }
    void DialogueEdit()
    {
        if (near)
        {
            SignText.text = sign;
            //this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            SignText.text = "";
            //this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    void NearJudge()
    {
        dist = Vector3.Distance(this.gameObject.transform.position, chr.transform.position);
        if (dist <= 3 && this.gameObject.transform.position[1] - chr.transform.position[1] < 5)
        {
            near = true;
        }
        else
        {
            near = false;
        }
    }
}
