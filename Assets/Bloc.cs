using UnityEngine;
using System.Collections;

public class Bloc : MonoBehaviour
{
    //[HideInInspector]
    public int BlocId;
    //[HideInInspector]
    public Color BlocColor;
    public Bloc LinkedBloc;

    private Enemy _enemy ;
    private SpriteRenderer _viewRenderer;

    void Awake()
    {
        _viewRenderer = GetComponentInChildren<SpriteRenderer>();
        _enemy = transform.parent.GetComponent<Enemy>();
    }

    public void UpdateColor(Color color)
    {
        BlocColor = color;
        _viewRenderer.color = color;
    }

	void Destroy ()
    {
        _enemy.BlocDestroyed();
	    if (LinkedBloc != null)
	    {
	        LinkedBloc.UpdateColor(BlocColor);
	    }

        Destroy(gameObject);
	}	
}
