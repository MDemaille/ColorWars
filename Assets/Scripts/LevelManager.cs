using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public int WeakColorNumber;

    public Color[] AllColors;
    public Color[] WeakColors; //{ get; private set; }
    public Color[] StrongColors; // { get; private set; }

    void Awake()
    {
        WeakColors = new Color[WeakColorNumber];
        StrongColors = new Color[AllColors.Length - WeakColorNumber];
        PickWeakAndStrongColors();
    }

    void PickWeakAndStrongColors()
    {
        Color colorPicked = AllColors[Random.Range(0, AllColors.Length - 1)];
        for (int i = 0; i < WeakColorNumber; i++)
        {
            //If the random color has already been selected, retry until it's not
            while (((IList<Color>)WeakColors).Contains(colorPicked))
            {
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

    bool ColorAlreadyPicked(Color color)
    {
        return WeakColors.Any(_color => _color != null && _color.Equals(color));
    }
}
