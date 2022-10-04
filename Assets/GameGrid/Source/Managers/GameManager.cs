using GameGrid.Source.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void NextStep()
    {
        SelectManager.Instance.ResetState();
        UnitsManager.Instance.ResetMovementPointsOfUnits();
    }
}
