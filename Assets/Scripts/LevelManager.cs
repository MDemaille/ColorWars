using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public enum PatternShape
{
    SQUARE
}

public enum PatternEffect
{
    SWITCH_COLORS,
    SWITCH_POSITIONS
}

public class LevelManager : MonoBehaviour
{
    public int WeakColorNumber;

    public GameObject Bloc;

    public Color[] AllColors;
    public Color[] WeakColors { get; private set; }
    public Color[] StrongColors { get; private set; }

    public PatternShape[] AuthorizedShapes;

    public int MaxSize;

    //In order of Definition
    public float SwitchColorProbability;
    public float SwitchPositionProbability;

    public static int Score = 0;

    void Awake()
    {
        WeakColors = new Color[WeakColorNumber];
        StrongColors = new Color[AllColors.Length - WeakColorNumber];
        PickWeakAndStrongColors();
    }

    void Start()
    {
        Score = 0;
        CreatePattern();
    }
    void PickWeakAndStrongColors()
    {
        Color colorPicked = AllColors[Random.Range(0, AllColors.Length - 1)];
        for (int i = 0; i < WeakColorNumber; i++)
        {
            //If the random color has already been selected, retry until it's not
            while (((IList<Color>)WeakColors).Contains(colorPicked))
            {
                Debug.Log("Try here, might be a problem");
                colorPicked = AllColors[Random.Range(0, AllColors.Length - 1)];
            }

            WeakColors[i] = colorPicked;
        }

        //Fill array with strong colors
        int cptStrong = 0;
        for (int i = 0; i < AllColors.Length; i++)
        {
            if (!((IList<Color>)WeakColors).Contains(AllColors[i]))
                StrongColors[cptStrong++] = AllColors[i];
        }
    }


    public void CreatePattern()
    {

        PatternShape newPatternShape = AuthorizedShapes[Random.Range(0, AuthorizedShapes.Length - 1)];
        int newSize = Random.Range(1, MaxSize);

        GameObject newPattern = new GameObject("Pattern");
        newPattern.transform.position = Vector3.zero;

        switch (newPatternShape)
        {
            case (PatternShape.SQUARE):

                Vector3 topLeftCorner = new Vector3(-newSize, newSize, 0);
                Vector3 bottomRightCorner = new Vector3(newSize, -newSize, 0);

                float xpos = 0;
                float ypos = 0;

                for (int x = 0; x < newSize; x++)
                {
                    for (int y = 0; y < newSize; y++)
                    {
                        xpos = topLeftCorner.x + ((bottomRightCorner.x - topLeftCorner.x) / (newSize + 1)) * (x + 1);
                        ypos = bottomRightCorner.y + ((topLeftCorner.y - bottomRightCorner.y) / (newSize + 1)) * (y + 1);

                        GameObject currentBloc = Instantiate(Bloc, Vector3.zero, Quaternion.identity, transform) as GameObject;
                        currentBloc.transform.position = new Vector3(xpos, ypos, 0);
                        currentBloc.transform.parent = newPattern.transform;
                    }
                }

                break;

        }

        newPattern.AddComponent<Pattern>();

        if (Random.Range(0f, 1f) > SwitchColorProbability )
        {
            newPattern.GetComponent<Pattern>().IsColorSwitchEnabled = true;
        }

        if (Random.Range(0f, 1f) > SwitchPositionProbability)
        {
            newPattern.GetComponent<Pattern>().IsPositionSwitchEnabled = true;
        }

    }

    bool ColorAlreadyPicked(Color color)
    {
        return ((IList<Color>)WeakColors).Contains(color);
    }
}
