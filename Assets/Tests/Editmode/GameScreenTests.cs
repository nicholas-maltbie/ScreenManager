using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace nickmaltbie.ScreenManager.Tests.EditMode.Actions
{
    [TestFixture]
    public class GameScreenTests
    {
        public class TestScreenComponent : MonoBehaviour, IScreenComponent
        {
            public int loaded = 0;
            public int unloaded = 0;

            public void OnScreenLoaded() => loaded++;
            public void OnScreenUnloaded() => unloaded++;
        }

        [Test]
        public void TestGameScreenLoading()
        {
            GameObject go = new GameObject();
            GameScreen screen = go.AddComponent<GameScreen>();

            PlayerInput playerInput = go.AddComponent<PlayerInput>();
            CanvasGroup canvasGroup = go.AddComponent<CanvasGroup>();

            TestScreenComponent attachedComponent = go.AddComponent<TestScreenComponent>();
            GameObject childObject = new GameObject();
            TestScreenComponent childComponent = childObject.AddComponent<TestScreenComponent>();
            childObject.transform.parent = go.transform;

            InputSystemUIInputModule uiInputModule = go.AddComponent<InputSystemUIInputModule>();

            // Test setup screen
            screen.SetupScreen(uiInputModule);

            screen.DisplayScreen();
            // assert that sub components are loaded
            Assert.That(attachedComponent.loaded, Is.EqualTo(1));
            Assert.That(childComponent.loaded, Is.EqualTo(1));

            screen.HideScreen();
            // Assert that components are unloaded correctly
            Assert.That(attachedComponent.unloaded, Is.EqualTo(1));
            Assert.That(childComponent.unloaded, Is.EqualTo(1));

            GameObject.DestroyImmediate(go);
        }
    }
}