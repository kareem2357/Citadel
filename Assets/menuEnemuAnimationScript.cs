using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuEnemuAnimationScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(this.transform.position.y<-10)
        {
            GameObject.Destroy(this.gameObject);
        }
	}
}
