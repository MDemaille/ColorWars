using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeakColorsUI : MonoBehaviour
{

    public GameObject WeakColorDisplay;

    private LevelManager _levelManager;

    private RectTransform _leftBound;
    private RectTransform _rightBound;

    GameObject[] WeakColorUI;

    void Awake()
    {
        _levelManager = GameObject.FindGameObjectWithTag( "LevelManager" ).GetComponent<LevelManager>();
        _leftBound = transform.Find( "LeftBound" ).GetComponent<RectTransform>();
        _rightBound = transform.Find( "RightBound" ).GetComponent<RectTransform>();
    }

    void Start()
    {
        UpdateWeakColors();
    }

    public void UpdateWeakColors()
    {
        if ( WeakColorUI != null )
        {
            for ( int i = 0; i < WeakColorUI.Length; i++ )
            {
                Destroy( WeakColorUI[ i ] );
            }
        }

        WeakColorUI = WeakColorUI = new GameObject[ _levelManager.WeakColorNumber ];

        float xPos = 0;
        int nbInterval = _levelManager.WeakColorNumber + 1;

        for ( int i = 0; i < _levelManager.WeakColorNumber; i++ )
        {
            xPos = _leftBound.localPosition.x + ( ( _rightBound.localPosition.x - _leftBound.localPosition.x ) / nbInterval ) * ( i + 1 );
            GameObject currentDisplay = Instantiate( WeakColorDisplay, Vector3.zero, Quaternion.identity, transform ) as GameObject;
            currentDisplay.transform.localPosition = new Vector3( xPos, 0, 0 );

            currentDisplay.GetComponent<Image>().color = _levelManager.WeakColors[ i ];

        }
    }

}
