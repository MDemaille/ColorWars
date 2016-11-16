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

    public float TimeOfLerpColor = 0.3f;
    private float _lerpTimer = 0;

    private LevelManager _levelManager;

    [FMODUnity.EventRef]
    public string SoundOnAppear = "event:/Level1/EnemyAppear";
    [FMODUnity.EventRef]
    public string SoundOnDeath = "event:/Level1/EnemyDie";
    [FMODUnity.EventRef]
    public string SoundOnExplode = "event:/Level1/TakeDamage";

    void Start()
    {
        _viewRenderer = GetComponentInChildren<SpriteRenderer>();
        _pattern = transform.parent.GetComponent<Pattern>();
        CurrentTime = TimeBeforeExplosion;
        _levelManager = GameObject.FindGameObjectWithTag( "LevelManager" ).GetComponent<LevelManager>();
        FMODUnity.RuntimeManager.PlayOneShot( SoundOnAppear, transform.position );
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

        if (CurrentTime < TimeOfLerpColor)
        {
            LerpColorWithTimeBeforeExplosion();
        }
    }

    public void ResetTimer()
    {
        CurrentTime = TimeBeforeExplosion;
    }

    private void LerpColorWithTimeBeforeExplosion()
    {
        _lerpTimer += Time.deltaTime;
        float time = _lerpTimer/TimeOfLerpColor;
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

        _levelManager.IncreaseCombo();
        _levelManager.IncreaseScore();
        FMODUnity.RuntimeManager.PlayOneShot( SoundOnDeath, transform.position );

        GameObject destructionFeedback = Instantiate(VisualEffects.FeedbackBubbleDestructionByPlayer, transform.position, Quaternion.identity) as GameObject;
	    destructionFeedback.GetComponent<ParticleSystem>().startColor = BlocColor;
        Debug.Log("Destruction Complete");
        Destroy(gameObject);
    }

    public void ExplodeAtPlayer()
    {
        _destroyed = true;
        _pattern.BlocDestroyed(this);

        FMODUnity.RuntimeManager.PlayOneShot( SoundOnExplode, transform.position );
        _levelManager.TakeDamage();
        _levelManager.ResetCombo();

        UpdateColor(Color.black);
        Instantiate(VisualEffects.FeedbackBubbleDestructionByPlayer, transform.position, Quaternion.identity);

        Camera.main.gameObject.GetComponent<CameraShake>().ShakeCamera(1.0f, 0.5f);

        Destroy(gameObject);
    }
}
