using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Omega.Kulibin
{
    public sealed class VideoPlayerView : ViewBase
    {
        public VideoPlayer VideoPlayer;
        public RawImage Image;
    }

    public sealed class VideoPlayerPresenter : PresenterBase<VideoPlayerView>
    {
        private Tweener _hidingFade;
        private Tweener _showingFade;

        private VideoPlayerState _state;

        public event Action OnShowed;
        public event Action OnHided;
        
        public VideoPlayerPresenter(VideoPlayerView view) : base(view)
        {
            
        }

        public VideoClip Clip => _view.VideoPlayer.clip;
        public bool IsPlaying => _state != VideoPlayerState.Off;

        public void SetRenderTexture(RenderTexture renderTexture)
        {
            _view.Image.texture = renderTexture;
            _view.VideoPlayer.targetTexture = renderTexture;
        }

        public async UniTask Prepare(VideoClip clip)
        {
            _view.VideoPlayer.Stop();
            _view.VideoPlayer.clip = clip;
            _view.VideoPlayer.Prepare();

            await UniTask.WaitUntil(() => _view.VideoPlayer.isPrepared);
        }
        
        public void Play()
        {
            _view.VideoPlayer.Play();
        }
        
        public void Pause()
        {
            _view.VideoPlayer.Pause();
        }

        public void Show(float duration, Ease ease, bool immediate = false)
        {
            _view.transform.SetAsLastSibling();

            if (immediate)
            {
                _hidingFade?.Kill();
                _showingFade?.Kill();
                _view.Image.color = Color.white;
                SetPlayingState();
            }
            else
            {
                switch (_state)
                {
                    case VideoPlayerState.Playing:
                    case VideoPlayerState.Showing:
                        return;
                    case VideoPlayerState.Hiding:
                        _showingFade?.PlayForward();
                        _hidingFade?.PlayBackwards();
                        break;
                    case VideoPlayerState.Off:
                        _showingFade = DOTween.To(() => _view.Image.color, value => _view.Image.color = value,
                                Color.white, duration)
                            .SetEase(ease)
                            .OnComplete(SetPlayingState)
                            .OnRewind(SetOffState);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _state = VideoPlayerState.Showing;
            }
        }

        public async UniTask ShowAsync(float duration, Ease ease, bool immediate = false)
        {
            Show(duration, ease, immediate);

            if (!immediate)
                await UniTask.WaitWhile(() => _state == VideoPlayerState.Showing);
        }

        public void Hide(float duration, Ease ease, bool immediate = false)
        {
            if (immediate)
            {
                _hidingFade?.Kill();
                _showingFade?.Kill();
                _view.Image.color = new Color(1f, 1f, 1f, 0f);
                SetOffState();
            }
            else
            {
                switch (_state)
                {
                    case VideoPlayerState.Off:
                    case VideoPlayerState.Hiding:
                        return;
                    case VideoPlayerState.Showing:
                        _showingFade?.PlayBackwards();
                        _hidingFade?.PlayForward();
                        break;
                    case VideoPlayerState.Playing:
                        _hidingFade = DOTween.To(() => _view.Image.color, value => _view.Image.color = value,
                                new Color(1f, 1f, 1f, 0f), duration)
                            .SetEase(ease)
                            .OnComplete(SetOffState)
                            .OnRewind(SetPlayingState);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _state = VideoPlayerState.Hiding;
            }
        }

        public async UniTask HideAsync(float duration, Ease ease, bool immediate = false)
        {
            Hide(duration, ease, immediate);

            if (!immediate)
                await UniTask.WaitWhile(() => _state == VideoPlayerState.Hiding);
        }

        private void SetOffState()
        {
            _state = VideoPlayerState.Off;
            OnHided?.Invoke();
        }

        private void SetPlayingState()
        {
            _state = VideoPlayerState.Playing;
            OnShowed?.Invoke();
        }
    }
}