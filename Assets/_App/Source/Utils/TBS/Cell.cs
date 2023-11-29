using System;
using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

namespace MaxFluff.Prototypes
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private GameObject _barricadePrefab;
        private Figure _figureOccupied;

        private LeanButton _button;

        private GameObject _barricade;

        private State _state;

        public event Action OnCellClicked;

        private Color _initColor;
        private Color _highlightedColor = new Color(0.9058824f, 0.8235294f, 0.7333333f, 1f);
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
            }
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
        Barricade
    }
}