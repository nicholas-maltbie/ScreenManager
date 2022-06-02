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
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static nickmaltbie.ScreenManager.Actions.RebindCompositeInput;

namespace nickmaltbie.ScreenManager.Tests.EditMode.Actions
{
    [TestFixture]
    public class RebindInputButtonTests
    {
        [Test]
        public void TestRebindInputSettings()
        {
#if UNITY_EDITOR
            UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, UnityEditor.SceneManagement.NewSceneMode.Single);
#endif

            RebindInputButton rebinding = new GameObject().AddComponent<RebindInputButton>();

            // Create a sample player input to override
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            PlayerInput input = rebinding.gameObject.AddComponent<PlayerInput>();
            InputActionAsset inputActionAsset = ScriptableObject.CreateInstance<InputActionAsset>();
            InputAction testAction = inputActionAsset.AddActionMap("testMap").AddAction("testAction", InputActionType.Button, keyboard.qKey.path, interactions: "Hold");
            input.actions = inputActionAsset;

            // Setup rebinding object
            rebinding.inputAction = InputActionReference.Create(testAction);
            rebinding.menuController = rebinding.gameObject.AddComponent<MenuController>();
            rebinding.bindingDisplayNameText = rebinding.gameObject.AddComponent<UnityEngine.UI.Text>();
            rebinding.startRebinding = new GameObject().AddComponent<Button>();
            rebinding.waitingForInputObject = new GameObject();
            rebinding.startRebinding.transform.parent = rebinding.transform;
            rebinding.waitingForInputObject.transform.parent = rebinding.transform;

            // Save a sample rebinding information
            PlayerPrefs.SetString(rebinding.InputMappingKey, keyboard.eKey.path);

            // Test by reading the settings
            rebinding.Awake();
            rebinding.Start();

            rebinding.gameObject.SetActive(true);

            // Start a test rebinding
            rebinding.startRebinding.onClick?.Invoke();

            // End the test rebinding
            rebinding.rebindingOperation.Complete();

            // Cleanup
            GameObject.DestroyImmediate(rebinding);

            // Remove rebinding override
            InputSystem.RemoveDevice(keyboard);
            PlayerPrefs.DeleteKey(rebinding.InputMappingKey);

            ScriptableObject.DestroyImmediate(inputActionAsset);
        }

        [Test]
        public void TestRebindCompositeInputSettings()
        {
#if UNITY_EDITOR
            UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, UnityEditor.SceneManagement.NewSceneMode.Single);
#endif

            RebindCompositeInput rebinding = new GameObject().AddComponent<RebindCompositeInput>();

            // Create a sample player input to override
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            PlayerInput input = rebinding.gameObject.AddComponent<PlayerInput>();
            InputActionAsset inputActionAsset = ScriptableObject.CreateInstance<InputActionAsset>();
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

            // Save a sample rebinding information
            PlayerPrefs.SetString(rebinding.InputMappingKey(1), keyboard.eKey.path);

            // Test by reading the settings
            rebinding.Awake();
            rebinding.Start();

            rebinding.gameObject.SetActive(true);

            // Start a test rebinding
            rebinding.rebindingGroups[0].startRebinding.onClick?.Invoke();

            // End the test rebinding
            rebinding.rebindingOperation.Complete();

            // Cleanup
            GameObject.DestroyImmediate(rebinding);

            // Remove rebinding override
            InputSystem.RemoveDevice(keyboard);
            PlayerPrefs.DeleteKey(rebinding.InputMappingKey(1));

            ScriptableObject.DestroyImmediate(inputActionAsset);
        }
    }
}
