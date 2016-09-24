using UnityEngine;
using System.Collections;

public class PatternManager : MonoBehaviour
{

    private LevelManager _levelManager;

	void Start ()
    {
        _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

    }
	
	void Update () {
	
	}
}
