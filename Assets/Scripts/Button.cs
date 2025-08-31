using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{

    // UnityEngine.UI.Button b;
    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     b = GetComponent<UnityEngine.UI.Button>();
    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    public void Notify()
    {
        Debug.Log("PRESSED BUTTON");
    }

    public void Quit()
    {
        Debug.Log("APP QUIT");
        Application.Quit();
    }
}
