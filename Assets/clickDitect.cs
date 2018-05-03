using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickDitect : MonoBehaviour
{

    public Sprite[] armorCondition;
    int currentConditon = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && currentConditon < 20)
        {
            this.GetComponent<SpriteRenderer>().sprite = armorCondition[currentConditon];
            currentConditon++;
        }

        if(currentConditon == 19)
        {
            this.GetComponent<DragAndDrop>().enabled = true;
        }
    }


}
