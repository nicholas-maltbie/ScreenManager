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

using System.Collections.Generic;
using com.nickmaltbie.ScreenManager.Actions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static com.nickmaltbie.ScreenManager.Actions.RebindCompositeInput;

namespace com.nickmaltbie.ScreenManager.Editor.Tests.Actions
{
    [TestFixture]
    public class RebindCompositeInputButtonTests
    {
        private RebindCompositeInput rebinding;
        private Keyboard keyboard;
        private InputActionAsset inputActionAsset;

        public static IEnumerable<string> SubsetKeyPaths;

        [SetUp]
        public void SetUp()
        {
            rebinding = new GameObject().AddComponent<RebindCompositeInput>();

            // Create a sample player input to override
            keyboard = InputSystem.AddDevice<Keyboard>();
            PlayerInput input = rebinding.gameObject.AddComponent<PlayerInput>();
            inputActionAsset = ScriptableObject.CreateInstance<InputActionAsset>();
            InputAction testAction = inputActionAsset.AddActionMap("testMap").AddAction("testAction", InputActionType.Value);
            testAction.AddCompositeBinding("Axis")
                .With("Positive", "<Keyboard>/w")
                .With("Negative", "<Keyboard>/s");
            input.actions = inputActionAsset;

            // Setup rebinding object
            rebinding.inputAction = InputActionReference.Create(testAction);
            rebinding.menuController = rebinding.gameObject.AddComponent<MenuController>();
            rebinding.rebindingGroups = new RebindingGroup[2];
            for (int i = 0; i < rebinding.rebindingGroups.Length; i++)
            {
                var group = new RebindingGroup();

                var bindingPlaceholder = new GameObject();
                bindingPlaceholder.transform.parent = rebinding.transform;

                group.bindingDisplayNameText = bindingPlaceholder.AddComponent<UnityEngine.UI.Text>();
                group.startRebinding = new GameObject().AddComponent<Button>();
                group.waitingForInputObject = new GameObject();
                group.startRebinding.transform.parent = bindingPlaceholder.transform;
                group.waitingForInputObject.transform.parent = bindingPlaceholder.transform;

                rebinding.rebindingGroups[i] = group;
            }

            // Test by reading the settings
            rebinding.Awake();
            rebinding.Start();

            rebinding.ResetBinding();

            SubsetKeyPaths = new[]
            {
                keyboard.qKey.path,
                keyboard.wKey.path,
                keyboard.eKey.path,
                keyboard.rKey.path,
                keyboard.tKey.path,
                keyboard.yKey.path
            };
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(rebinding.gameObject);

            // Remove rebinding override
            InputSystem.RemoveDevice(keyboard);
            PlayerPrefs.DeleteKey(rebinding.InputMappingKey(1));

            ScriptableObject.DestroyImmediate(inputActionAsset);
        }

        [Test]
        public void TestRebindingInputLoadSettings([Values] bool overrideOne, [Values] bool overrideTwo)
        {
            // Save a sample rebinding information
            PlayerPrefs.SetString(rebinding.InputMappingKey(1), overrideOne ? keyboard.eKey.path : string.Empty);
            PlayerPrefs.SetString(rebinding.InputMappingKey(2), overrideTwo ? keyboard.qKey.path : string.Empty);

            // Test by reading the settings
            rebinding.Awake();
            rebinding.Start();

            Assert.IsTrue(
                overrideOne ?
                rebinding.inputAction.action.bindings[1].overridePath == keyboard.eKey.path :
                string.IsNullOrEmpty(rebinding.inputAction.action.bindings[1].overridePath));
            Assert.IsTrue(
                overrideTwo ?
                rebinding.inputAction.action.bindings[2].overridePath == keyboard.qKey.path :
                string.IsNullOrEmpty(rebinding.inputAction.action.bindings[2].overridePath));
        }

        [Test]
        public void TestRebindInputRebindAction([Values(1, 2)] int index)
        {
            foreach (string path in SubsetKeyPaths)
            {
                // Start a test rebinding
                rebinding.rebindingGroups[index - 1].startRebinding.onClick?.Invoke();

                // Simulate hitting key to override binding path
                rebinding.inputAction.action.ApplyBindingOverride(index, path);

                Assert.IsTrue(rebinding.rebindingOperation != null);

                // End the test rebinding
                rebinding.rebindingOperation.Complete();

                Assert.IsTrue(rebinding.inputAction.action.bindings[index].overridePath == path);
            }
        }

        [Test]
        public void TestRebindInputSettingsCancel([Values(1, 2)] int index)
        {
            // Start a test rebinding
            rebinding.rebindingGroups[index - 1].startRebinding.onClick?.Invoke();
            // Cancel the test rebinding
            rebinding.rebindingOperation.Cancel();
            // Assert that no override path has been set
            Assert.IsTrue(string.IsNullOrEmpty(rebinding.inputAction.action.bindings[index].overridePath));
        }

        [Test]
        public void TestRebindingReset()
        {
            // Simulate hitting key to override binding path
            rebinding.inputAction.action.ApplyBindingOverride(1, keyboard.rKey.path);
            Assert.IsTrue(rebinding.inputAction.action.bindings[1].overridePath == keyboard.rKey.path);
            rebinding.ResetBinding();

            // Assert that no override path has been set
            Assert.IsTrue(string.IsNullOrEmpty(rebinding.inputAction.action.bindings[0].overridePath));
        }

        [Test]
        public void TestRebindingUpdateDisplay([Values(1, 2)] int index)
        {
            foreach (string path in SubsetKeyPaths)
            {
                rebinding.ResetBinding();
                rebinding.UpdateDisplay();

                if (path == rebinding.inputAction.action.bindings[index].effectivePath)
                {
                    continue;
                }

                string previousName = rebinding.GetKeyReadableName(index);
                Assert.IsTrue(rebinding.rebindingGroups[index - 1].bindingDisplayNameText.text == rebinding.GetKeyReadableName(index));

                // Simulate hitting key to override binding path
                rebinding.inputAction.action.ApplyBindingOverride(index, path);
                rebinding.UpdateDisplay();
                Assert.IsTrue(rebinding.rebindingGroups[index - 1].bindingDisplayNameText.text == rebinding.GetKeyReadableName(index));

                string overrideName = rebinding.GetKeyReadableName(index);

                Assert.IsTrue(previousName != overrideName);
            }
        }
    }
}
