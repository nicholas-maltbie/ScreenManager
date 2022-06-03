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
using UnityEngine;
using UnityEngine.UI;

namespace nickmaltbie.ScreenManager.Tests.EditMode.Actions
{
    [TestFixture]
    public class ChangeQualityActionsTests
    {
        [Test]
        public void TestChangeQuality()
        {
            // Setup the object
            ChangeQualityActions qualitySettings = new GameObject().AddComponent<ChangeQualityActions>();
            qualitySettings.qualityDropdown = qualitySettings.gameObject.AddComponent<Dropdown>();

            // Test setup
            qualitySettings.Awake();
            Assert.IsTrue(QualitySettings.GetQualityLevel() == PlayerPrefs.GetInt(ChangeQualityActions.qualityLevelPlayerPrefKey, QualitySettings.GetQualityLevel()));
            Assert.IsTrue(qualitySettings.qualityDropdown.options.Count == QualitySettings.names.Length);
            // Test change value
            qualitySettings.OnQualityLevelChange(1);
            Assert.IsTrue(QualitySettings.GetQualityLevel() == 1);
            qualitySettings.OnQualityLevelChange(2);
            Assert.IsTrue(QualitySettings.GetQualityLevel() == 2);

            // Cleanup
            GameObject.DestroyImmediate(qualitySettings.gameObject);
        }
    }
}
