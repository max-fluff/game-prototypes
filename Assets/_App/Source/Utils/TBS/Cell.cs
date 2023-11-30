using System;
using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

namespace MaxFluff.Prototypes
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private GameObject _barricadePrefab;
        [SerializeField] private GameObject _spearPrefab;
        private Figure _figureOccupied;

        private LeanButton _button;

        private GameObject _barricade;
        private GameObject _spear;
        private Side _spearSide;

        private State _lastState;

        public Side SpearSide
        {
            set
            {
                _spearSide = value;

                if (_spear)
                {
                    _spear.GetComponentInChildren<Graphic>().color = value switch
                    {
                        Side.White => new Color(1f, 0.9176471f, 0.7686275f, 0.6f),
                        Side.Black => new Color(0.254717f, 0.2133886f, 0.1477839f, 0.6f),
                        _ => _spear.GetComponentInChildren<Graphic>().color
                    };
                }
            }
            get => _spearSide;
        }

        private State _state;

        public event Action OnCellClicked;

        private Color _initColor;
        private readonly Color _highlightedColor = new Color(0.9058824f, 0.8235294f, 0.7333333f, 1f);
        private Graphic _graphic;

        public State State
        {
            get => _state;

            set
            {
                if (value == _state)
                    return;

                ProcessStateOff(_state);

                _state = value;

                ProcessStateOn(_state);
            }
        }

        public Figure FigureOccupied
        {
            get => _figureOccupied;
            set
            {
                _figureOccupied = value;
                _figureOccupied.transform.SetParent(transform, false);
            }
        }

        private void ProcessStateOff(State state)
        {
            switch (state)
            {
                case State.Barricade:
                {
                    if (_barricade)
                    {
                        Destroy(_barricade);
                        _barricade = null;
                    }

                    break;
                }

                case State.Spear:
                {
                    if (_spear)
                    {
                        Destroy(_spear);
                        _spear = null;
                    }

                    break;
                }
            }

            _lastState = state;
        }

        public void ToLastState() => State = _lastState;

        private void ProcessStateOn(State state)
        {
            switch (state)
            {
                case State.Barricade:
                {
                    _barricade = Instantiate(_barricadePrefab, transform);

                    break;
                }
                case State.Spear:
                {
                    _spear = Instantiate(_spearPrefab, transform);
                    SpearSide = SpearSide;

                    break;
                }
            }
        }

        private void Awake()
        {
            var figure = GetComponentInChildren<Figure>();
            if (figure)
            {
                FigureOccupied = figure;
                State = State.Figure;
            }

            _graphic = GetComponent<Graphic>();
            _initColor = _graphic.color;
            _button = GetComponent<LeanButton>();
            _button.OnClick.AddListener(OnClicked);
        }

        public void Highlight(bool state)
        {
            _graphic.color = state ? _highlightedColor : _initColor;
        }

        private void OnClicked() => OnCellClicked?.Invoke();
    }

    public enum State
    {
        None,
        Figure,
        Barricade,
        Spear
    }
}