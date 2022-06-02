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
            var scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, UnityEditor.SceneManagement.NewSceneMode.Single);
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
            var scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, UnityEditor.SceneManagement.NewSceneMode.Single);
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
                RebindingGroup group = new RebindingGroup();

                GameObject bindingPlaceholder = new GameObject();
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