using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public Bloc[] Blocs;
    private int _currentNbBlocs;
    private LevelManager _levelManager;

    void Start()
    {
        Blocs = GetComponentsInChildren<Bloc>();
        _currentNbBlocs = Blocs.Length;

        _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

        if (_currentNbBlocs%_levelManager.WeakColorNumber != 0)
        {
            Debug.LogError("Number of blocs not fitting number of weak colors (must be a multiple)");
            Application.Quit();
        }

        if (_currentNbBlocs < _levelManager.WeakColorNumber)
        {
            Debug.LogError("Not enough blocs to get weak colors");
            Application.Quit();
        }

        for (int i = 0; i < _currentNbBlocs; i++)
        {
            Blocs[i].BlocId = i;
        }

        AttributeColorsToBlocs();
        LinkBlocs();
    }

    void AttributeColorsToBlocs()
    {
        //Associating weak colors to random IDs
        int[] randIDs = new int[_levelManager.WeakColorNumber];

        int randomID = Random.Range(0, _currentNbBlocs - 1);

        for (int i = 0; i < _levelManager.WeakColorNumber; i++)
        {
            //If the random ID has already been selected, retry until it's not
            while (((IList<int>) randIDs).Contains(randomID))
            {
                randomID = Random.Range(0, _currentNbBlocs - 1);
                Debug.Log("Could be a random problem");
            }

            randIDs[i] = randomID;
        }

        //Associate all weak colors
        for (int i = 0; i < _levelManager.WeakColorNumber; i++)
        {
            Blocs[randIDs[i]].UpdateColor(_levelManager.WeakColors[i]);
        }

        //Associate all other colors
        for (int i = 0; i < Blocs.Length; i++)
        {
            if(!((IList<int>)randIDs).Contains(i))
                Blocs[i].UpdateColor(_levelManager.StrongColors[Random.Range(0, _levelManager.StrongColors.Length)]);
        }
    }

    void LinkBlocs()
    {
        for(int i = 0; i<Blocs.Length ; i++)
        {
            if(Blocs[i].LinkedBloc == null)
                continue;

            int j = Random.Range(0, Blocs.Length - 1);

            while (i == j || Blocs[j].LinkedBloc != null)
            {
                j = Random.Range(0, Blocs.Length - 1);
                Debug.Log("Link Try");
            }

            Blocs[i].LinkedBloc = Blocs[j];

        }
    }

    public void BlocDestroyed()
    {
        _currentNbBlocs --;
        if(_currentNbBlocs <= 0)
            Death();
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
