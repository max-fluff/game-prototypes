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
        [SerializeField] private GameObject _explosionPrefab;
        private Figure _figureOccupied;

        private LeanButton _button;

        private GameObject _barricade;
        private GameObject _spear;
        private GameObject _explosion;
        private Side _spearSide;

        private State _lastState;
        private State _initState;

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

                if (_state != State.Figure)
                    _lastState = _state;

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
                if (_figureOccupied)
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

                case State.Figure:
                {
                    FigureOccupied = null;
                    break;
                }

                case State.Explosion:
                {
                    if (_explosion)
                    {
                        Destroy(_explosion);
                        _explosion = null;
                    }

                    break;
                }
            }
        }

        public void ToLastState()
        {
            State = _lastState != State.Figure ? _lastState : State.None;
        }

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
                case State.Explosion:
                {
                    _explosion = Instantiate(_explosionPrefab, transform);

                    break;
                }
            }
        }

        public void RecordInitState() => _initState = State;

        public void RestoreInitState()
        {
            State = _initState;
            _lastState = _initState;
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
        Spear,
        Explosion
    }
}