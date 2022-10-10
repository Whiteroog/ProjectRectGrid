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
        StartCoroutine(DelayNextStepProcessing());
        SelectManager.Instance.ResetState();
        UnitsManager.Instance.ResetMovementPointsOfUnits();
    }

    private IEnumerator DelayNextStepProcessing()
    {
        SelectManager.Instance.IsProcessing = true;
        yield return new WaitForSeconds(1);
        SelectManager.Instance.IsProcessing = false;
    }
}
