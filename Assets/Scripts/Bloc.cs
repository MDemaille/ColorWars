using UnityEngine;
using System.Collections;

public class Bloc : MonoBehaviour
{
    [HideInInspector]
    public int BlocId;
    [HideInInspector]
    public Color BlocColor;
    [HideInInspector]
    public Bloc LinkedBloc;

    private Pattern _pattern ;
    private SpriteRenderer _viewRenderer;

    private bool _destroyed = false;

    [HideInInspector]
    public bool IsWeak = false;

    [HideInInspector]
    public float TimeBeforeExplosion = 100;
    [HideInInspector]
    public float CurrentTime;

    void Start()
    {
        _viewRenderer = GetComponentInChildren<SpriteRenderer>();
        _pattern = transform.parent.GetComponent<Pattern>();
        CurrentTime = TimeBeforeExplosion;
    }

    void Update()
    {
        if (IsWeak)
        {
            CurrentTime -= Time.deltaTime;
            if (CurrentTime <= 0)
            {
                ExplodeAtPlayer();
                IsWeak = false;
            }
        }

        LerpColorWithTimeBeforeExplosion();
    }

    public void ResetTimer()
    {
        CurrentTime = TimeBeforeExplosion;
    }

    private void LerpColorWithTimeBeforeExplosion()
    {
        float time = 1- CurrentTime/TimeBeforeExplosion;
        _viewRenderer.color = Color.Lerp(BlocColor, Color.black, time );
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
	    _destroyed = true;
        _pattern.BlocDestroyed(this);

        GameObject destructionFeedback = Instantiate(VisualEffects.FeedbackBubbleDestructionByPlayer, transform.position, Quaternion.identity) as GameObject;
	    destructionFeedback.GetComponent<ParticleSystem>().startColor = BlocColor;
        Debug.Log("Destruction Complete");
        Destroy(gameObject);
    }

    public void ExplodeAtPlayer()
    {
        _destroyed = true;
        _pattern.BlocDestroyed(this);

        UpdateColor(Color.black);
        Instantiate(VisualEffects.FeedbackBubbleDestructionByPlayer, transform.position, Quaternion.identity);

        Camera.main.gameObject.GetComponent<CameraShake>().ShakeCamera(1.0f, 0.5f);

        Destroy(gameObject);
    }
}
