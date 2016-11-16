using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonRetry : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener( delegate { SceneManager.LoadScene(SceneManager.GetActiveScene().name); } );
    }
}
