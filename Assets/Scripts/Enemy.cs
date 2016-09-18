using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public Bloc[] Blocs;
    private int _currentNbBlocs;
    private LevelManager _levelManager;

    private int[] _weakBlocIds;
    private int[] _strongBlocIds;

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

        _weakBlocIds = new int[_levelManager.WeakColorNumber];
        _strongBlocIds = new int[_currentNbBlocs - _levelManager.WeakColorNumber];

        AttributeColorsToBlocs();
        LinkBlocs();
    }

    void AttributeColorsToBlocs()
    {
        //Associating weak colors to random IDs
        int[] IdsNotSelected = new int[_currentNbBlocs];
        for (int i = 0; i < _currentNbBlocs; i++)
            IdsNotSelected[i] = i;

        int randomID = 0;
       
        for (int i = 0; i < _levelManager.WeakColorNumber; i++)
        {
            randomID = IdsNotSelected[Random.Range(0, IdsNotSelected.Length - 1)];
            Debug.Log("Itération " + i + " : Taille " + IdsNotSelected.Length);
            Debug.Log("Itération " + i + " : Selection " + randomID);
            IdsNotSelected = ArrayTools.RemoveElement(IdsNotSelected, randomID);
            _weakBlocIds[i] = randomID;
        }

        //Associate all weak colors
        for (int i = 0; i < _levelManager.WeakColorNumber; i++)
        {
            Blocs[_weakBlocIds[i]].UpdateColor(_levelManager.WeakColors[i]);
        }

        //Associate all other colors
        int j = 0;
        for (int i = 0; i < Blocs.Length; i++)
        {
            if (!((IList<int>) _weakBlocIds).Contains(i))
            {
                _strongBlocIds[j++] = i;
                Blocs[i].UpdateColor(_levelManager.StrongColors[Random.Range(0, _levelManager.StrongColors.Length)]);
            }
        }
    }

    void LinkBlocs()
    {
        //Define the chain size according to the numbers of blocs and the number of weak colors.
        int ChainSize = (_currentNbBlocs/_levelManager.WeakColorNumber) - 1;

        //Copying the strong points
        int[] tempStrongPoints = new int[_strongBlocIds.Length];
        Array.Copy(_strongBlocIds, 0, tempStrongPoints, 0, _strongBlocIds.Length);

        //Main loop on the weak points to create chain from them
        for (int weakCpt = 0; weakCpt < _weakBlocIds.Length; weakCpt++)
        {
            //Seed of the chain
            int seed = _weakBlocIds[weakCpt];
            for (int chainCpt = 0; chainCpt < ChainSize; chainCpt++)
            {
                //Strong point to be linked
                int LinkID = tempStrongPoints[Random.Range(0, tempStrongPoints.Length)];
                Blocs[seed].LinkedBloc = Blocs[LinkID];
                seed = LinkID;
                tempStrongPoints = ArrayTools.RemoveElement(tempStrongPoints, LinkID);
            } 
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
