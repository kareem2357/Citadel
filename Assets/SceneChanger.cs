using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour {

    public void changeToScene(int changeTheScene)
    {
        SceneManager.LoadScene(changeTheScene);


    }
}
