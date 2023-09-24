using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMixerVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider m_slider;
    [SerializeField] private AudioMixerGroup m_mixerGroup;


    private void Start()
    {
        m_mixerGroup.audioMixer.GetFloat($"{m_mixerGroup.name}Volume", out var volume);
        volume = PlayerPrefs.GetFloat(m_mixerGroup.name, volume);

        volume = Mathf.Clamp(volume, -80f, 0f);

        m_slider.minValue = -80;
        m_slider.maxValue = 0;

        m_slider.SetValueWithoutNotify(volume);
        m_mixerGroup.audioMixer.SetFloat($"{m_mixerGroup.name}Volume", volume);
        Debug.Log(volume);

        m_slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged (float value)
    {
        // We should do some remapping of the values in order to have better control of the audio volume
        /*
        var normalized = (value + 80f) / 80f;
        var flipped = 1f - normalized;
        flipped = flipped * flipped * flipped;
        normalized = 1f - flipped;

        value = normalized * 80f - 80f;
        value = Mathf.Clamp(value, -80f, 0f);
        */

        m_mixerGroup.audioMixer.SetFloat($"{m_mixerGroup.name}Volume", value);

        PlayerPrefs.SetFloat(m_mixerGroup.name, value);
        PlayerPrefs.Save();
    }
}
