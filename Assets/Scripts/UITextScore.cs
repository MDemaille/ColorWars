using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;

public enum Variable
{
    Score,
    Multiplier,
    Combo
}

public class UITextScore : MonoBehaviour
{
    public Variable VariableToDisplay = Variable.Score;

    private LevelManager _levelManager;
    private Text _text;

    private int _lastUpdatedScore = 0;

    public float TimeToReachValue = 0.1f;
    private float _velocity = 0f;

    void Start()
    {
        _levelManager = GameObject.FindGameObjectWithTag( "LevelManager" ).GetComponent<LevelManager>();
        _text = GetComponent<Text>();
    }
	
	void Update ()
	{
	    if (VariableToDisplay.Equals(Variable.Score))
	    {
	        var nfi = (NumberFormatInfo) CultureInfo.InvariantCulture.NumberFormat.Clone();
	        nfi.NumberGroupSeparator = " ";

	        _lastUpdatedScore =
	            Mathf.RoundToInt(Mathf.SmoothDamp(_lastUpdatedScore, _levelManager.Score, ref _velocity, TimeToReachValue));
	        _text.text = _lastUpdatedScore.ToString("#,0", nfi);
	    }

        if ( VariableToDisplay.Equals( Variable.Combo ) )
        {
            _text.text = _levelManager.Combo.ToString();
        }

        if ( VariableToDisplay.Equals( Variable.Multiplier ) )
        {
            _text.text = "X " + _levelManager.Multiplier.ToString();
        }
    }
}
