using UnityEngine;
using System.Collections;

public class Bloc : MonoBehaviour
{
    //[HideInInspector]
    public int BlocId;
    //[HideInInspector]
    public Color BlocColor;
    public Bloc LinkedBloc;

    private Pattern _pattern ;
    private SpriteRenderer _viewRenderer;

    void Start()
    {
        _viewRenderer = GetComponentInChildren<SpriteRenderer>();
        _pattern = transform.parent.GetComponent<Pattern>();
    }

    public void UpdateColor(Color color)
    {
        BlocColor = color;
        Debug.Log("Update color de l'ID : " + BlocId);
        GameObject colorAttributionFeedback = Instantiate(VisualEffects.FeedbackColorAttribution, transform.position, Quaternion.identity) as GameObject;
        colorAttributionFeedback.GetComponent<ParticleSystem>().startColor = BlocColor;

        _viewRenderer.color = color;
    }

	public void Destroy ()
    {
        _pattern.BlocDestroyed(this);

        GameObject destructionFeedback = Instantiate(VisualEffects.FeedbackBlocDestruction, transform.position, Quaternion.identity) as GameObject;
	    destructionFeedback.GetComponent<ParticleSystem>().startColor = BlocColor;
        Debug.Log("Destruction Complete");
        Destroy(gameObject);
    }	
}
