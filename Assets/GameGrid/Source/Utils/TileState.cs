using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameGrid.Source.Utils
{
    public class TileState : MonoBehaviour
    {
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color selectColor;
        [SerializeField] private Color possibleWaysColor;

        private Image[] _borderLines;
        
        public SelectType CurrentSelectType { private set; get; }

        private void Awake()
        {
            _borderLines = GetComponentsInChildren<Image>();
        }

        public void SetBorderColor(SelectType selectType)
        {
            CurrentSelectType = selectType;
            Color setNewColor = selectType switch
            {
                SelectType.Default => defaultColor,
                SelectType.Select => selectColor,
                SelectType.PossibleWays => possibleWaysColor,
                _ => defaultColor
            };

            foreach (Image borderLine in _borderLines)
            {
                borderLine.color = setNewColor;
            }
        }
    }

    public enum SelectType
    {
        Default,
        Select,
        PossibleWays
    }
}
