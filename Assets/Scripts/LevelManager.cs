using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

public enum PatternShape
{
    SQUARE,
    CIRCLE
}

public enum PatternEffect
{
    SWITCH_COLORS,
    SWITCH_POSITIONS
}

public class LevelManager : MonoBehaviour
{
    public bool GameOver = false;

    public int WeakColorNumber;

    public GameObject Bloc;

    public Color[] AllColors;
    public Color[] WeakColors { get; private set; }
    public Color[] StrongColors { get; private set; }

    public PatternShape[] AuthorizedShapes;

    public int MaxSizeSquare;
    public int MaxSizeCircle;

    //In order of Definition
    public float SwitchColorProbability;
    public float SwitchPositionProbability;
    public float SwitchWeakColorProbability;

    public int Health = 10;
    public int Score = 0;
    public int Combo = 0;
    public int Multiplier = 1;
    public bool DamageTakenDuringPattern = false;

    public float TimeBeforeBubblesExplosion = 1.0f;
    public float TimeBeforeBubblesLerpColor = 1.0f;
    public float TimeBetweenSwitchColors = 1.5f;
    public float TimeBetweenSwitchPositions = 2.5f;
    public float TimeSwitchingPositions = 0.5f;

    private WeakColorsUI _weakColorsUI;
    private GameObject _gameOverMenu;

    private Pattern _currentPattern;

    void Awake()
    {
        WeakColors = new Color[ WeakColorNumber ];
        StrongColors = new Color[ AllColors.Length - WeakColorNumber ];
        PickWeakAndStrongColors();
        Score = 0;
        Combo = 0;
        Multiplier = 1;
        DamageTakenDuringPattern = false;
        GameOver = false;
    }

    void Start()
    {
        Score = 0;
        _weakColorsUI = GameObject.FindGameObjectWithTag( "WeakColorUI" ).GetComponent<WeakColorsUI>();
        _gameOverMenu = GameObject.FindGameObjectWithTag("GameOver");
        _gameOverMenu.SetActive(false);
        CreatePattern();
    }

    IEnumerator EndGame()
    {
        GameOver = true;
        _currentPattern.Death();
        Debug.Log( "GameOver" );
        yield return new WaitForSeconds(1.5f);
        _gameOverMenu.SetActive( true );
    }

    public void TakeDamage()
    {
        DamageTakenDuringPattern = true;
        Combo = 0;
        Health -= 1;
        if (Health <= 0)
        {
            StartCoroutine(EndGame());
        }
    }

    public void IncreaseScore()
    {
        Debug.Log("Score");
        int increase = ( 100 + Combo ) * Multiplier;
        Score += increase;
    }

    public void IncreaseCombo()
    {
        Combo += 1;
    }

    public void ResetCombo()
    {
        Combo = 0;
    }

    public void IncreaseMultiplier()
    {
        Multiplier += 1;
    }

    public void DecreaseMultiplier()
    {
        if(Multiplier > 1)
            Multiplier -= 1;
    }

    void PickWeakAndStrongColors()
    {
        Color colorPicked = AllColors[ Random.Range( 0, AllColors.Length - 1 ) ];
        for ( int i = 0; i < WeakColorNumber; i++ )
        {
            //If the random color has already been selected, retry until it's not
            while ( ( (IList<Color>)WeakColors ).Contains( colorPicked ) )
            {
                Debug.Log( "Try here, might be a problem" );
                colorPicked = AllColors[ Random.Range( 0, AllColors.Length - 1 ) ];
            }
            WeakColors[ i ] = colorPicked;
        }

        //Fill array with strong colors
        int cptStrong = 0;
        for ( int i = 0; i < AllColors.Length; i++ )
        {
            if ( !( (IList<Color>)WeakColors ).Contains( AllColors[ i ] ) )
                StrongColors[ cptStrong++ ] = AllColors[ i ];
        }
    }

    public void ChangeWeakColors()
    {
        WeakColors = new Color[ WeakColorNumber ];
        StrongColors = new Color[ AllColors.Length - WeakColorNumber ];
        PickWeakAndStrongColors();
        _weakColorsUI.UpdateWeakColors();
    }

    public void UpdateDifficulty()
    {
        if (Score > 250000)
        {
            SwitchWeakColorProbability = 0.1f;
            SwitchColorProbability = 0.2f;
            SwitchPositionProbability = 0.2f;

            MaxSizeCircle = 5;
        }

        if (Score > 500000)
        {
            SwitchWeakColorProbability = 0.3f;
            SwitchColorProbability = 0.5f;
            SwitchPositionProbability = 0.5f;

            MaxSizeCircle = 7;
        }

        if (Score > 750000)
        {
            SwitchWeakColorProbability = 0.5f;
            SwitchColorProbability = 0.6f;
            SwitchPositionProbability = 0.6f;

            MaxSizeCircle = 10;
        }

        if ( Score > 1000000 )
        {
            TimeBetweenSwitchColors = 1.0f;
            TimeBetweenSwitchPositions = 1.5f;
            TimeSwitchingPositions = 0.6f;
        }

        if ( Score > 1250000 )
        {
            SwitchWeakColorProbability = 0.7f;
        }

        if ( Score > 2000000 )
        {
            TimeBeforeBubblesExplosion = 1.0f;
        }

        if ( Score > 4000000 )
        {
            SwitchWeakColorProbability = 0.9f;
            SwitchColorProbability = 0.8f;
            SwitchPositionProbability = 0.8f;
        }
    }

    public void CreatePattern()
    {
        if (GameOver)
            return;

        UpdateDifficulty();


        DamageTakenDuringPattern = false;

        if ( Random.Range( 0f, 1f ) < SwitchWeakColorProbability )
        {
            ChangeWeakColors();
        }

        PatternShape newPatternShape = AuthorizedShapes[ Random.Range( 0, AuthorizedShapes.Length - 1 ) ];

        GameObject newPattern = new GameObject( "Pattern" );
        newPattern.transform.position = Vector3.zero;

        int newSize = 0;

        switch ( newPatternShape )
        {
            case ( PatternShape.SQUARE ):

                newSize = Random.Range( 2, MaxSizeSquare );

                Vector3 topLeftCorner = new Vector3( -newSize, newSize, 0 );
                Vector3 bottomRightCorner = new Vector3( newSize, -newSize, 0 );

                float xpos = 0;
                float ypos = 0;

                for ( int x = 0; x < newSize; x++ )
                {
                    for ( int y = 0; y < newSize; y++ )
                    {
                        xpos = topLeftCorner.x + ( ( bottomRightCorner.x - topLeftCorner.x ) / ( newSize + 1 ) ) * ( x + 1 );
                        ypos = bottomRightCorner.y + ( ( topLeftCorner.y - bottomRightCorner.y ) / ( newSize + 1 ) ) * ( y + 1 );

                        GameObject currentBloc = Instantiate( Bloc, Vector3.zero, Quaternion.identity, transform ) as GameObject;
                        currentBloc.GetComponent<Bloc>().TimeBeforeExplosion = TimeBeforeBubblesExplosion;
                        currentBloc.GetComponent<Bloc>().TimeOfLerpColor = TimeBeforeBubblesLerpColor;
                        currentBloc.transform.position = new Vector3( xpos, ypos, 0 );
                        currentBloc.transform.parent = newPattern.transform;
                    }
                }

                break;

            case ( PatternShape.CIRCLE ):

                newSize = Random.Range( WeakColorNumber, MaxSizeCircle );

                for ( int i = 0; i < newSize; i++ )
                {
                    float value = (float)i / (float)newSize;
                    float teta = ( Mathf.PI * 2 ) * value;
                    double phi = -Math.PI / 2;

                    float pX = newPattern.transform.position.x + (float)( Math.Sin( phi ) * Math.Cos( teta ) );
                    float pY = newPattern.transform.position.y + (float)( Math.Sin( phi ) * Math.Sin( ( teta ) ) );

                    Vector2 bubblePosition = new Vector2( pX, pY );
                    bubblePosition.Normalize();

                    GameObject currentBloc = Instantiate( Bloc, Vector3.zero, Quaternion.identity, transform ) as GameObject;
                    currentBloc.GetComponent<Bloc>().TimeBeforeExplosion = TimeBeforeBubblesExplosion;
                    currentBloc.GetComponent<Bloc>().TimeOfLerpColor = TimeBeforeBubblesLerpColor;
                    currentBloc.transform.position = bubblePosition * 2;
                    currentBloc.transform.parent = newPattern.transform;
                }

                break;

        }

        Pattern patternComponent = newPattern.AddComponent<Pattern>();

        patternComponent.TimeBetweenSwitchColors = TimeBetweenSwitchColors;
        patternComponent.TimeBetweenSwitchPositions = TimeBetweenSwitchPositions;
        patternComponent.TimeSwitchingPositions = TimeSwitchingPositions;

        if ( Random.Range( 0f, 1f ) < SwitchColorProbability )
        {
            patternComponent.IsColorSwitchEnabled = true;
        }

        if ( Random.Range( 0f, 1f ) < SwitchPositionProbability )
        {
            patternComponent.IsPositionSwitchEnabled = true;
        }

        _currentPattern = patternComponent;

    }

    bool ColorAlreadyPicked( Color color )
    {
        return ( (IList<Color>)WeakColors ).Contains( color );
    }
}
