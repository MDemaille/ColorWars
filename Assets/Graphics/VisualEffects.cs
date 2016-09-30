using UnityEngine;
using System.Collections;

public class VisualEffects : MonoBehaviour
{
    public static GameObject FeedbackTouch;
    public static GameObject FeedbackBubbleDestructionByPlayer;
    public static GameObject FeedbackColorAttribution;
    public static GameObject FeedbackBubbleExplosionOnPlayer;

    private GameObject _background;

    void Awake ()
    {
        DontDestroyOnLoad(gameObject);
        FeedbackTouch = Resources.Load("FeedbackTouch") as GameObject;
        FeedbackBubbleDestructionByPlayer = Resources.Load("FeedbackBubbleDestruction") as GameObject;
        FeedbackColorAttribution = Resources.Load("FeedbackBubbleColorAttribution") as GameObject;
        FeedbackBubbleExplosionOnPlayer = Resources.Load("FeedbackBubbleExplosionOnPlayer") as GameObject;
    }



    //TODO Camera shake
    //TODO ScreenColorChange
}
