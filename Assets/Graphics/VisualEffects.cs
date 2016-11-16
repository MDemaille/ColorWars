using UnityEngine;
using System.Collections;

public class VisualEffects : MonoBehaviour
{
    public static GameObject FeedbackTouch;
    public static GameObject FeedbackBubbleDestructionByPlayer;
    public static GameObject FeedbackColorAttribution;
    public static GameObject FeedbackBubbleExplosionOnPlayer;

    private static bool _backgroundIsChanging;

    private static GameObject _background;

    void Awake ()
    {
        DontDestroyOnLoad(gameObject);
        FeedbackTouch = Resources.Load("FeedbackTouch") as GameObject;
        FeedbackBubbleDestructionByPlayer = Resources.Load("FeedbackBubbleDestruction") as GameObject;
        FeedbackColorAttribution = Resources.Load("FeedbackBubbleColorAttribution") as GameObject;
        FeedbackBubbleExplosionOnPlayer = Resources.Load("FeedbackBubbleExplosionOnPlayer") as GameObject;
        _background = GameObject.FindGameObjectWithTag("Background");
    }

    public static IEnumerator BlinkColorsBackground(Color[] colors, float timeFade, bool returnToClear)
    {
        if(_backgroundIsChanging)
            yield break;

        _backgroundIsChanging = true;
        SpriteRenderer renderer = _background.GetComponent<SpriteRenderer>();
        Color initialColor = renderer.color;
        Color currentColor = renderer.color;
        float timer = 0;
        float progression = 0;

        for (int colorID = 0; colorID < colors.Length; colorID++)
        {
            
            currentColor = renderer.color;
            timer = 0;
            
            while (timer < timeFade)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                progression = timer / timeFade;
                timer += Time.deltaTime;
                renderer.color = Color.Lerp(currentColor, colors[colorID], progression);
            }
        }

        if (returnToClear)
        {
            progression = timer / timeFade;
            currentColor = renderer.color;
            timer = 0;

            while ( timer < timeFade )
            {
                yield return new WaitForSeconds( Time.deltaTime );
                progression = timer / timeFade;
                timer += Time.deltaTime;
                renderer.color = Color.Lerp( currentColor, initialColor, progression );
            }
        }

        renderer.color = initialColor;

        _backgroundIsChanging = false;
    }

    //TODO Camera shake
    //TODO ScreenColorChange
}
