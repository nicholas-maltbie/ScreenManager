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

using System.Collections;
using nickmaltbie.ScreenManager.Actions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace nickmaltbie.ScreenManager.Editor.Tests.Actions
{
    [TestFixture]
    public class ChangeScreenActionsTests
    {
        private ChangeScreenActions changeScreen;

        [SetUp]
        public void SetUp()
        {
            var go = new GameObject();
            ScreenLoading.setupDisplay = false;
            changeScreen = go.AddComponent<ChangeScreenActions>();
            changeScreen.runtimePlatform = () => RuntimePlatform.WindowsEditor;

            // Setup dropdowns
            changeScreen.displayDropdown = new GameObject().AddComponent<Dropdown>();
            changeScreen.windowedDropdown = new GameObject().AddComponent<Dropdown>();
            changeScreen.resolutionDropdown = new GameObject().AddComponent<Dropdown>();
            changeScreen.vsyncToggle = new GameObject().AddComponent<Toggle>();

            // Attach dropdons to objects
            changeScreen.displayDropdown.transform.parent = changeScreen.transform;
            changeScreen.windowedDropdown.transform.parent = changeScreen.transform;
            changeScreen.resolutionDropdown.transform.parent = changeScreen.transform;
            changeScreen.vsyncToggle.transform.parent = changeScreen.transform;

            // Attach confirm dialog
            var confirmDialog = new GameObject();
            var settingsPanel = new GameObject();
            confirmDialog.transform.parent = go.transform;
            settingsPanel.transform.parent = go.transform;

            changeScreen.settingsPage = settingsPanel.AddComponent<CanvasGroup>();
            changeScreen.confirmPanel = confirmDialog.AddComponent<CanvasGroup>();
            changeScreen.confirmDialogYes = new GameObject().AddComponent<Button>();
            changeScreen.confirmDialogNo = new GameObject().AddComponent<Button>();
            changeScreen.settingsMenuController = settingsPanel.AddComponent<MenuController>();
            changeScreen.confirmDialogYes.gameObject.transform.parent = confirmDialog.transform;
            changeScreen.confirmDialogNo.gameObject.transform.parent = confirmDialog.transform;
            changeScreen.confirmDialogText = confirmDialog.AddComponent<UnityEngine.UI.Text>();

            // Setup change screne object
            changeScreen.Awake();
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(changeScreen.gameObject);
        }

        [Test]
        public void ModifyScreenSettings()
        {
            // Simulate selecting an option from each control
            changeScreen.windowedDropdown.onValueChanged?.Invoke(0);
            changeScreen.displayDropdown.onValueChanged?.Invoke(0);
            changeScreen.resolutionDropdown.onValueChanged?.Invoke(0);
            changeScreen.vsyncToggle.onValueChanged?.Invoke(false);
        }

        [Test]
        public void TestSetupWebGLEmpty()
        {
            // Test setup on WebGL Platform
            changeScreen.runtimePlatform = () => RuntimePlatform.WebGLPlayer;
            changeScreen.Awake();
        }

        [Test]
        public void TestFullScreenLookup()
        {
            // Simulate translations of each setting
            ChangeScreenActions.GetFullScreenModeDropdownIndex(FullScreenMode.ExclusiveFullScreen);
            ChangeScreenActions.GetFullScreenModeDropdownIndex(FullScreenMode.FullScreenWindow);
            ChangeScreenActions.GetFullScreenModeDropdownIndex(FullScreenMode.MaximizedWindow);
            ChangeScreenActions.GetFullScreenModeDropdownIndex(FullScreenMode.Windowed);

            Assert.IsTrue(ChangeScreenActions.GetFullScreenMode(ScreenLoading.fullScreenModeName)
                == FullScreenMode.ExclusiveFullScreen);
            Assert.IsTrue(ChangeScreenActions.GetFullScreenMode(ScreenLoading.windowedModeName)
                == FullScreenMode.Windowed);
            Assert.IsTrue(ChangeScreenActions.GetFullScreenMode(ScreenLoading.borderlessWindowModeName)
                == FullScreenMode.FullScreenWindow);
            Assert.IsTrue(ChangeScreenActions.GetFullScreenMode("other")
                == FullScreenMode.ExclusiveFullScreen);
        }

        [Test]
        public void TestFilterResolutions()
        {
            var resolutions = new Resolution[]
            {
                new Resolution{width = 10, height = 20, refreshRate = 10},
                new Resolution{width = 20, height = 20, refreshRate = 10},
                new Resolution{width = 10, height = 20, refreshRate = 20},
                new Resolution{width = 20, height = 40, refreshRate = 10},
            };
            Resolution[] filtered = ChangeScreenActions.FilterResolutions(resolutions);
            Assert.IsTrue(resolutions.Length > filtered.Length);
        }

        [Test]
        public void ConfirmDialogSettings()
        {
            // Simulate waiting for a timeout
            // Create a confirm dialog
            IEnumerator confirm = changeScreen.OpenConfirmChangesDialog();
            // Wait for timeout and confirm revert
            while (confirm.MoveNext())
            {
                ;
            }

            // Wait for timeout but interrupt early by clicking the yes button
            confirm = changeScreen.OpenConfirmChangesDialog();
            confirm.MoveNext();
            confirm.MoveNext();
            confirm.MoveNext();
            // end early by cancel
            changeScreen.confirmDialogYes.onClick?.Invoke();
            // Finish the co-routine
            while (confirm.MoveNext())
            {
                ;
            }

            // Wait for timeout but interrupt early by clicking the no button
            confirm = changeScreen.OpenConfirmChangesDialog();
            confirm.MoveNext();
            confirm.MoveNext();
            confirm.MoveNext();
            // end early by cancel
            changeScreen.confirmDialogNo.onClick?.Invoke();
            // Finish the co-routine
            while (confirm.MoveNext())
            {
                ;
            }
        }
    }
}
