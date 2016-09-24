using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonQuit : MonoBehaviour {

    void Start ()
    {
        GetComponent<Button>().onClick.AddListener(delegate { Application.Quit(); });
    }
}
