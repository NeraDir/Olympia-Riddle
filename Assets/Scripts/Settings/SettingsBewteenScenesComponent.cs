using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsBewteenScenesComponent : MonoBehaviour {
   private AudioSource _musicSource;
   private AudioSource _effectsSource;
   private MneuManager _mneuManager;
   private Image _bgImage;
   
   public static Action<AudioClip> playEffect;
   public static Action changeBg;
   
   [SerializeField] private List<Sprite> _bgSprites;
   
   private void Awake() {
      transform.parent = null;
      DontDestroyOnLoad(this.gameObject);
      _musicSource = transform.GetChild(0).GetComponent<AudioSource>();
      _effectsSource = transform.GetChild(1).GetComponent<AudioSource>();
      _musicSource.volume = DataContainer.MusicVolume;
      _effectsSource.volume = DataContainer.SfxVolume;
      playEffect += OnPlayEffect;
      changeBg += OnChangeBg;
   }

   private void OnDestroy() {
      playEffect -= OnPlayEffect;
      changeBg -= OnChangeBg;
      _mneuManager?.GetMusicVolumeSlider()?.onValueChanged.RemoveListener(OnMusicVolumeChanged);
      _mneuManager?.GetSfxVolumeSlider()?.onValueChanged.RemoveListener(OnEffectsVolumeChanged);
   }

   private void OnChangeBg() {
      _bgImage.sprite = _bgSprites.Find(x => x.name == DataContainer.BgName);
   }

   private void LateUpdate() {
      if (_mneuManager == null) {
         _mneuManager = FindObjectOfType<MneuManager>();
         if (_mneuManager != null) {
            _mneuManager.GetMusicVolumeSlider().onValueChanged.AddListener(OnMusicVolumeChanged);
            _mneuManager.GetSfxVolumeSlider().onValueChanged.AddListener(OnEffectsVolumeChanged);

         }
      }

      if (_bgImage == null) {
         _bgImage = GameObject.Find("BG").GetComponent<Image>();
         _bgImage.sprite = _bgSprites.Find(x => x.name == DataContainer.BgName);
      }
   }

   private void OnPlayEffect(AudioClip clip) {
      _effectsSource.PlayOneShot(clip);
   }
   
   private void OnEffectsVolumeChanged(float volume) {
      DataContainer.SfxVolume = volume;
      _effectsSource.volume = DataContainer.SfxVolume;
   }

   private void OnMusicVolumeChanged(float volume) {
      DataContainer.MusicVolume = volume;
      _musicSource.volume = DataContainer.MusicVolume;
   }
}
