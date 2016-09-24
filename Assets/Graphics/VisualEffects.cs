using UnityEngine;
using System.Collections;

public class VisualEffects : MonoBehaviour
{
    public static GameObject FeedbackTouch;
    public static GameObject FeedbackBlocDestruction;
    public static GameObject FeedbackColorAttribution;

    void Awake ()
    {
        DontDestroyOnLoad(gameObject);
        FeedbackTouch = Resources.Load("FeedbackTouch") as GameObject;
        FeedbackBlocDestruction = Resources.Load("FeedbackBlocDestruction") as GameObject;
        FeedbackColorAttribution = Resources.Load("FeedbackColorAttribution") as GameObject;
    }
}
