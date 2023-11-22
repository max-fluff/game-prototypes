using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class Radio : MonoBehaviour
    {
        public List<AudioSource> sounds;
        public AudioSource initSound;
        public Material materialWithEmission;

        private bool _isOn;
        private bool _wasTuned;

        private void Start()
        {
            TurnOff();
        }

        private async UniTask TurnOn()
        {
            _isOn = true;

            if (!_wasTuned)
            {
                initSound.PlayOneShot(initSound.clip);

                materialWithEmission.EnableKeyword("_EMISSION");

                await UniTask.WaitWhile(() => initSound.isPlaying);
            }

            if (!_isOn)
                return;

            _wasTuned = true;

            sounds.ForEach(s =>
            {
                if (s.isPlaying)
                    s.UnPause();
                else
                    s.Play();
            });
        }

        private void TurnOff()
        {
            _isOn = false;

            initSound.Stop();

            materialWithEmission.DisableKeyword("_EMISSION");

            sounds.ForEach(s => { s.Pause(); });
        }

        public void Toggle()
        {
            if (_isOn)
                TurnOff();
            else
                TurnOn();
        }
    }
}