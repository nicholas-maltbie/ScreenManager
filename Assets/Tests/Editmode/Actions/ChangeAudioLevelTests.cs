using nickmaltbie.ScreenManager.Actions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace nickmaltbie.ScreenManager.Tests.EditMode.Actions
{
    [TestFixture]
    public class ChangeAudioLevelTests
    {
        [Test]
        public void TestChangeAudioLevel()
        {
            // Setup the object
            var audioSettings = new GameObject().AddComponent<SoundAdjustActions>();
            var slider = audioSettings.gameObject.AddComponent<Slider>();
            AudioMixer audioMixer = AssetDatabase.LoadAssetAtPath<AudioMixer>("Assets/ScreenManager/Sounds/Settings/AudioMixer.mixer");
            audioSettings.settingsGroups = new SoundAdjustActions.AudioMixerSettingsGroup[1];
            audioSettings.settingsGroups[0] = new SoundAdjustActions.AudioMixerSettingsGroup
            {
                mixerGroup = audioMixer.FindMatchingGroups("Master")[0],
                slider = slider
            };

            // Test setup
            audioSettings.Start();

            // Test setting slider level
            slider.onValueChanged?.Invoke(0.45f);
            // Test setting slider level to muted
            slider.onValueChanged?.Invoke(0.0f);
            // Test setting slider level to maxed
            slider.onValueChanged?.Invoke(1.0f);

            // Test reading slider level below zero
            SoundAdjustActions.GetSliderValue(SoundAdjustActions.minVolume - 10.0f);
            SoundAdjustActions.GetSliderValue(SoundAdjustActions.mutedVolume);
            // Test reading slider level above max
            SoundAdjustActions.GetSliderValue(SoundAdjustActions.maxVolume + 10.0f);

            // Cleanup
            GameObject.DestroyImmediate(audioSettings.gameObject);
        }
    }
}