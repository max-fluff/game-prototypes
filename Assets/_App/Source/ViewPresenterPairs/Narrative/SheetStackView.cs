using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lean.Transition;
using LeTai.Asset.TranslucentImage;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace MaxFluff.Prototypes
{
    public class SheetStackView : ViewBase
    {
        public TapProcessor TapProcessor;
        public SingleSheetView SingleSheetTemplate;
        public ScalableBlurConfig ScalableBlurConfig;
        public Transform Camera;
        public LeanPlayer SleepTransition;
        public AudioMixerGroup Muffler;
    }

    public class SheetStackPresenter : PresenterBase<SheetStackView>
    {
        private SingleSheetPresenter _currentSheet;
        private const float MaxRadius = 30;
        private const float MaxCameraOffset = 5;
        private const float MaxSheets = 30;
        private const float MaxMuffle = 800;
        private const float MinMuffle = 5000;
        private int _currentSheetsCount;
        private readonly Vector3 _initCameraAngle;
        private readonly Random _random = new Random();

        public SheetStackPresenter(SheetStackView view) : base(view)
        {
            _view.TapProcessor.OnDraggedRight.AddListener(CreateSheet);
            _initCameraAngle = _view.Camera.localEulerAngles;
            CameraBobble().Forget();
        }

        private void CreateSheet()
        {
            if (_currentSheet != null) return;

            var newSheet = Object.Instantiate(_view.SingleSheetTemplate);
            newSheet.transform.position = _view.transform.position;
            _currentSheet = new SingleSheetPresenter(newSheet);

            _currentSheet.OnDestroy += RemoveCurrentSheet;
            _currentSheet.PlayGetTransition();
            _view.TapProcessor.enabled = false;

            _currentSheetsCount++;
            CheckIfReadyToSleepAndPlay().Forget();
        }

        private async UniTask CheckIfReadyToSleepAndPlay()
        {
            if (_currentSheetsCount < MaxSheets) return;

            _view.SleepTransition.Begin();

            var timer = 0f;
            var maxTime = 3;

            while (timer < maxTime)
            {
                _view.Muffler.audioMixer.SetFloat("Muffle",  MinMuffle - (MinMuffle - MaxMuffle) * timer / maxTime);
                _view.ScalableBlurConfig.Radius = MaxRadius * 2 * timer / maxTime;

                await UniTask.Yield();
                timer += Time.deltaTime;
            }
        }

        private async UniTask CameraBobble()
        {
            float timer;
            while (_currentSheetsCount < MaxSheets)
            {
                var newCameraRotation = _initCameraAngle;
                var offset = (_currentSheetsCount / MaxSheets) * MaxCameraOffset;
                newCameraRotation.x = _initCameraAngle.x + offset;
                var duration = GetDuration();
                _view.Camera.DOLocalRotate(newCameraRotation, duration);

                timer = 0f;

                while (timer < duration)
                {
                    _view.ScalableBlurConfig.Radius = MaxRadius * timer / duration * _currentSheetsCount / MaxSheets;
                    _view.Muffler.audioMixer.SetFloat("Muffle",  MinMuffle - (MinMuffle - MaxMuffle) * timer / duration * _currentSheetsCount / MaxSheets);

                    await UniTask.Yield();
                    timer += Time.deltaTime;
                }

                newCameraRotation = _initCameraAngle;
                duration = GetDuration();
                _view.Camera.DOLocalRotate(newCameraRotation, duration);

                timer = 0f;

                while (timer < duration)
                {
                    _view.ScalableBlurConfig.Radius =
                        MaxRadius * (duration - timer) / duration * _currentSheetsCount / MaxSheets;
                    _view.Muffler.audioMixer.SetFloat("Muffle",
                        MinMuffle - (MinMuffle - MaxMuffle) * (duration - timer) / duration * _currentSheetsCount / MaxSheets) ;


                    await UniTask.Yield();
                    timer += Time.deltaTime;
                }

                await UniTask.Delay((int) (_random.NextDouble() * 5 + 5) * 1000);
            }
        }

        private float GetDuration()
        {
            return (float) (_random.NextDouble() * 3f + 2f);
        }

        private void RemoveCurrentSheet()
        {
            _currentSheet = null;
            _view.TapProcessor.enabled = true;
        }

        public void SetStampOnCurrent(Vector3 worldPos) => _currentSheet.SetStamp(worldPos);
        public void SetSignatureOnCurrent(Vector3 worldPos) => _currentSheet.SetSignature(worldPos);
    }
}