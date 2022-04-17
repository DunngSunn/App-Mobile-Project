using System;
using System.Collections.Generic;
using DG.Tweening;
using DunnGSunn;
using UnityEngine;

namespace Manager
{
    // ------------------------------- //
    [Serializable]
    public class Sound
    {
        // ======================== //
        #region Fields
        
        [Header("Clip")]
        [SerializeField] private string clipName;
        [SerializeField] private AudioClip clip;
        
        [Header("Clip fields")]
        [Range(0f, 1f), SerializeField] private float volume = 1f;
        [Range(0f, 3f), SerializeField] private float pitch = 1f;
        [SerializeField] private bool loop;
        [SerializeField] private bool playOnAwake;
        
        private AudioSource _audioSource;

        public string ClipName
        {
            get => clipName;
            set => clipName = value;
        }
        
        #endregion

        // ======================== //
        #region Sound control functions

        public void InitializeSource(AudioSource audioSource)
        {
            _audioSource = audioSource;
            _audioSource.clip = clip;
            _audioSource.pitch = pitch;
            _audioSource.volume = volume;
            _audioSource.playOnAwake = playOnAwake;
            _audioSource.loop = loop;
        }

        public void Play()
        {
            if (_audioSource.isPlaying) return;
            _audioSource.Play();
        }

        public void PlayWithFadeEffect()
        {
            if (_audioSource.isPlaying) return;
            _audioSource.volume = 0;
            _audioSource.Play();
            _audioSource.DOFade(1f, .35f);
        }

        public void Stop()
        {
            if (!_audioSource.isPlaying) return;
            _audioSource.Stop();
        }

        public void StopWithFadeEffect()
        {
            if (!_audioSource.isPlaying) return;
            _audioSource.volume = 1f;
            _audioSource.DOFade(0f, .35f).OnComplete(() => _audioSource.Stop());
        }

        public void SetVolume(float value)
        {
            _audioSource.volume = value;
        }

        #endregion
    }
    
    // ------------------------------- //
    public class AudioManager : SunMonoSingleton<AudioManager>
    {
        // ======================== //
        #region Fields

        [Header("Sounds")]
        [SerializeField] private List<Sound> sounds;

        #endregion

        // ======================== //
        #region Unity callback functions

        protected override void LoadInAwake()
        {
            // Initialize all of sounds
            if (sounds.Count > 0)
            {
                foreach (var sound in sounds)
                {
                    var go = new GameObject($"Sound_{sound.ClipName}");
                    go.transform.SetParent(transform);
                    sound.InitializeSource(go.AddComponent<AudioSource>());
                }   
            }
            
            //
            SunEventManager.StartListening(EventID.ThemeAudioPlay, OnThemeAudioPlay);
            SunEventManager.StartListening(EventID.ThemeAudioStop, OnThemeAudioStop);
            
            //
            SunEventManager.StartListening(EventID.BreathInAudio, OnBreathInAudio);
            SunEventManager.StartListening(EventID.BreathOutAudio, OnBreathOutAudio);
            SunEventManager.StartListening(EventID.BreathStop, OnBreathStop);
        }

        private void OnDestroy()
        {
            //
            SunEventManager.StopListening(EventID.ThemeAudioPlay, OnThemeAudioPlay);
            SunEventManager.StopListening(EventID.ThemeAudioStop, OnThemeAudioStop);
            
            //
            SunEventManager.StopListening(EventID.BreathInAudio, OnBreathInAudio);
            SunEventManager.StopListening(EventID.BreathOutAudio, OnBreathOutAudio);
            SunEventManager.StopListening(EventID.BreathStop, OnBreathStop);
        }

        #endregion

        // ======================== //
        #region Sound control functions

        public void PlaySound(string nameClip)
        {
            foreach (var sound in sounds)
            {
                if (sound.ClipName == nameClip)
                {
                    sound.Play();
                    return;
                }
            }
        }
        
        public void PlaySoundWithEffect(string nameClip)
        {
            foreach (var sound in sounds)
            {
                if (sound.ClipName == nameClip)
                {
                    sound.PlayWithFadeEffect();
                    return;
                }
            }
        }

        public void StopSound(string nameClip)
        {
            foreach (var sound in sounds)
            {
                if (sound.ClipName == nameClip)
                {
                    sound.Stop();
                    return;
                }
            }
        }
        
        public void StopSoundWithEffect(string nameClip)
        {
            foreach (var sound in sounds)
            {
                if (sound.ClipName == nameClip)
                {
                    sound.StopWithFadeEffect();
                    return;
                }
            }
        }

        #endregion

        // ======================== //
        #region Sun events

        private void OnThemeAudioPlay()
        {
            var nameTheme = (string) SunEventManager.GetSender(EventID.ThemeAudioPlay);
            PlaySoundWithEffect(nameTheme);
        }

        private void OnThemeAudioStop()
        {
            var nameTheme = (string) SunEventManager.GetSender(EventID.ThemeAudioStop);
            StopSoundWithEffect(nameTheme);
        }

        private void OnBreathOutAudio()
        {
            StopSound("Breath_Out");
            PlaySound("Breath_Out");
        }

        private void OnBreathInAudio()
        {
            StopSound("Breath_In");
            PlaySound("Breath_In");
        }

        private void OnBreathStop()
        {
            StopSound("Breath_In");
            StopSound("Breath_Out");
        }

        #endregion
    }
}