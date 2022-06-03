// Copyright (C) 2022 Nicholas Maltbie
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
            SoundAdjustActions audioSettings = new GameObject().AddComponent<SoundAdjustActions>();
            Slider slider = audioSettings.gameObject.AddComponent<Slider>();
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
