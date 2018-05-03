using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour {

    public float MAX_HITPOINTS = 1000;
    public float HitPoints;
	public Text hpText;
    public _2dxFX_Clipping castleHpClip;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        hpText.text = Mathf.Clamp((int)this.HitPoints, 0, int.MaxValue).ToString();
        castleHpClip._ClipRight = 1f - (((float)HitPoints) / MAX_HITPOINTS);
    }

    public void TakeDamage(float damage) {
		this.HitPoints-=damage;

        if (HitPoints <= 0) {
			GameEngine.GetInstance().EndGame();
		}
	}
}
