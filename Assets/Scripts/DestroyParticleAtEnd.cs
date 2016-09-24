using UnityEngine;
using System.Collections;

public class DestroyParticleAtEnd : MonoBehaviour {

	void Start ()
    {
	    Destroy(gameObject, GetComponent<ParticleSystem>().duration);
	}
}
