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

        private TypeSelect _selectType;
        public TypeSelect SelectType
        {
            set
            {
                _selectType = value;

                Color setNewColor = value switch
                {
                    TypeSelect.Default => defaultColor,
                    TypeSelect.Select => selectColor,
                    TypeSelect.PossibleWay => possibleWaysColor,
                    _ => defaultColor
                };

                foreach (Image borderLine in _borderLines)
                {
                    borderLine.color = setNewColor;
                }
            }
            get => _selectType;
        }

        private void Awake()
        {
            _borderLines = GetComponentsInChildren<Image>();
        }
    }

    public enum TypeSelect
    {
        Default,
        Select,
        PossibleWay
    }
}
