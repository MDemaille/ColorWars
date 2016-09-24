using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Pattern : MonoBehaviour
{
    public Bloc[] Blocs;

    public bool IsColorSwitchEnabled = false;
    public bool IsPositionSwitchEnabled = false;

    private int _currentNbBlocs;
    private LevelManager _levelManager;

    private int[] _weakBlocIds;
    private int[] _strongBlocIds;

    private bool _effectLock = false; //Lock to prevent to reset everything while a bloc transition in active

    void Start()
    {
        Blocs = GetComponentsInChildren<Bloc>();
        _currentNbBlocs = Blocs.Length;

        _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();


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

        AttributeColorsToBlocs();
        LinkBlocs();

        if (IsColorSwitchEnabled)
          StartCoroutine(SwitchColors(1.0f));

        if(IsPositionSwitchEnabled)
            StartCoroutine(SwitchPositions(1.0f));

    }

    void AttributeColorsToBlocs()
    {
        _strongBlocIds = new int[_currentNbBlocs - _levelManager.WeakColorNumber];


        //Associating weak colors to random IDs
        int[] IdsNotSelected = new int[_currentNbBlocs];
        for (int i = 0; i < _currentNbBlocs; i++)
            IdsNotSelected[i] = Blocs[i].BlocId;

        int randomID = 0;

        for (int i = 0; i < _levelManager.WeakColorNumber; i++)
        {
            randomID = IdsNotSelected[Random.Range(0, IdsNotSelected.Length - 1)];
            //Debug.Log("Itération " + i + " : Taille " + IdsNotSelected.Length);
            //Debug.Log("Itération " + i + " : Selection " + randomID);
            IdsNotSelected = ArrayTools.RemoveElement(IdsNotSelected, randomID);
            _weakBlocIds[i] = randomID;
        }

        //Associate all weak colors
        int weakCpt = 0; //TODO change that with getBlocID
        for (int i = 0; i < Blocs.Length; i++)
        {
            if (((IList<int>)_weakBlocIds).Contains(Blocs[i].BlocId))
            {
                Blocs[i].UpdateColor(_levelManager.WeakColors[weakCpt++]);
            }
        }

        //Associate all other colors
        int j = 0;
        for (int i = 0; i < Blocs.Length; i++)
        {
            if (!((IList<int>)_weakBlocIds).Contains(Blocs[i].BlocId))
            {
                _strongBlocIds[j++] = Blocs[i].BlocId;
                Blocs[i].UpdateColor(_levelManager.StrongColors[Random.Range(0, _levelManager.StrongColors.Length)]);
            }
        }
    }

    IEnumerator SwitchPositions(float time)
    {
        ExchangeBlocsPositions();

        while (true)
        {
            yield return new WaitForSeconds(time);
            ExchangeBlocsPositions();
        }
    }

    IEnumerator SwitchColors(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            if(!_effectLock)
                ChangeBlocsColor();
        }
    }

    void ExchangeBlocsPositions()
    {
        if (_currentNbBlocs == 1)
            return;

        int[] BlocChain = new int[_currentNbBlocs];

        int[] IdsNotSelected = new int[_currentNbBlocs];
        for (int i = 0; i < _currentNbBlocs; i++)
            IdsNotSelected[i] = Blocs[i].BlocId;

        int randomID = 0;

        for (int i = 0; i < _currentNbBlocs; i++)
        {
            randomID = IdsNotSelected[Random.Range(0, IdsNotSelected.Length - 1)];

            IdsNotSelected = ArrayTools.RemoveElement(IdsNotSelected, randomID);
            BlocChain[i] = randomID;
        }

        for (int i = 0; i < _currentNbBlocs - 1; i++)
        {
            LeanTween.moveLocal(GetBloc(BlocChain[i]).gameObject,
                GetBloc(BlocChain[i + 1]).gameObject.transform.position, 1.0f);
        }

        LeanTween.moveLocal(GetBloc(BlocChain[BlocChain.Length-1]).gameObject,
                GetBloc(BlocChain[0]).gameObject.transform.position, 1.0f);

    }

    void ChangeBlocsColor()
    {
        AttributeColorsToBlocs();
        LinkBlocs();
    }

    void LinkBlocs()
    {
        //Define the chain size according to the numbers of blocs and the number of weak colors.
        int chainSize = (int)Mathf.Floor((float)(_currentNbBlocs / _levelManager.WeakColorNumber) - 1);

        //Used if chain size is not an integer
        int lastChainSize = chainSize + 1;

        //Copying the strong points
        int[] tempStrongPoints = new int[_strongBlocIds.Length];
        Array.Copy(_strongBlocIds, 0, tempStrongPoints, 0, _strongBlocIds.Length);

        //Main loop on the weak points to create chain from them
        for (int weakCpt = 0; weakCpt < _weakBlocIds.Length; weakCpt++)
        {
            //Seed of the chain
            int seed = _weakBlocIds[weakCpt];
            int currentChainSize = chainSize;

            //If it's the last chain and if there is a rest
            if (_currentNbBlocs % _levelManager.WeakColorNumber != 0 && weakCpt == _weakBlocIds.Length - 1)
            {
                currentChainSize = lastChainSize;
            }

            for (int chainCpt = 0; chainCpt < currentChainSize; chainCpt++)
            {
                //Strong point to be linked
                int LinkID = tempStrongPoints[Random.Range(0, tempStrongPoints.Length)];

                GetBloc(seed).LinkedBloc = GetBloc(LinkID);
                seed = LinkID;

                tempStrongPoints = ArrayTools.RemoveElement(tempStrongPoints, LinkID);
            }
        }
    }

    Bloc GetBloc(int id)
    {
        for (int i = 0; i < Blocs.Length; i++)
        {
            if (Blocs[i].BlocId == id)
                return Blocs[i];
        }

        return null;
    }

    IEnumerator AttributeColorToBloc(int id, Color color, float time)
    {
        _effectLock = true;
        yield return new WaitForSeconds(time);
        GetBloc(id).UpdateColor(color);
        _effectLock = false;
    }

    public void BlocDestroyed(Bloc blocDestroyed)
    {
        _currentNbBlocs--;
        Blocs = ArrayTools.RemoveElement(Blocs, blocDestroyed);
        Debug.Log("Blocs restants : " + Blocs.Length);

        if (_currentNbBlocs <= 0)
        {
            Death();
            return;
        }

        //Color Link       
        if (blocDestroyed.LinkedBloc != null)
        {
            Color blocDestroyedColor = blocDestroyed.BlocColor;
            int blocDestroyedLinkedId = blocDestroyed.LinkedBloc.BlocId;
            StartCoroutine(AttributeColorToBloc(blocDestroyedLinkedId, blocDestroyedColor, 0.25f));
        }

    }

    private void Death()
    {
        _levelManager.CreatePattern();
        Destroy(gameObject);
    }
}
