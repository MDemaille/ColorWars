using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{

    private LevelManager _levelManager;

    void Start ()
    {
        _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        ///////////MOUSE/////////////////////
	    if (Input.GetMouseButtonDown(0))
	    {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray, Vector2.zero);
            // Create a particle if hit
            foreach (var currentHit in hits)
            {
                Debug.Log("Hit");
                if (currentHit.collider.gameObject.CompareTag("Bloc"))
                {
                    Bloc hitBloc = currentHit.collider.gameObject.GetComponent<Bloc>();
                    if (((IList<Color>)_levelManager.WeakColors).Contains(hitBloc.BlocColor))
                    {
                        hitBloc.Destroy();
                        Debug.Log("Destruction");
                    }
                }
            }

            Instantiate(VisualEffects.FeedbackTouch, ray, Quaternion.identity);

        }


        ////////TOUCH/////////////
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                // Construct a ray from the current touch coordinates
                Vector2 ray = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                RaycastHit2D[] hits = Physics2D.RaycastAll(ray, Vector2.zero);
                // Create a particle if hit

                foreach (var currentHit in hits)
                {
                    Debug.Log("Hit");
                    if (currentHit.collider.gameObject.CompareTag("Bloc"))
                    {
                        Bloc hitBloc = currentHit.collider.gameObject.GetComponent<Bloc>();
                        if (((IList<Color>) _levelManager.WeakColors).Contains(hitBloc.BlocColor))
                        {
                            hitBloc.Destroy();
                            Debug.Log("Destruction");
                        }
                    }
                }

                Instantiate(VisualEffects.FeedbackTouch, ray, Quaternion.identity);
            }
        }
    }
}
