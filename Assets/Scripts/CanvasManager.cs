using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject CanvasHome;
    public GameObject CanvasMenu;
    // Start is called before the first frame update
    void Start()
    {
        if (GameData.esconderHome)
        {
            CanvasHome.SetActive(false);
            CanvasMenu.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowLevelSelect()
    {
        CanvasHome.SetActive(false);
        CanvasMenu.SetActive(true);
    }
}
