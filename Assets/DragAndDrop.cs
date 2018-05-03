using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {

    public float factor;
    bool wasBeingDragged;

    // Update is called once per frame
    void Update()
    {
        bool beingDragged = false;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool overSprite = GetComponent<SpriteRenderer>().bounds.Contains(mousePosition);
        
        if (overSprite)
        {
            //If we've pressed down on the mouse (or touched on the iphone)
            if (Input.GetButton("Fire1"))
            {
                beingDragged = true;
            }
        }
        else
        {
            beingDragged = wasBeingDragged && Input.GetButton("Fire1");
        }

        if (beingDragged)
        {
            Vector3 newPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                             Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
                                             0.0f);
            /*//Set the position to the mouse position
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                             Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
                                             0.0f);
                                             */
            if (this.GetComponent<Rigidbody2D>() != null) this.GetComponent<Rigidbody2D>().AddForce((newPosition - transform.position) * factor);
        }

        wasBeingDragged = beingDragged;
    }
}
