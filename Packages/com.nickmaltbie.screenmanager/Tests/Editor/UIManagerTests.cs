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

namespace com.nickmaltbie.ScreenManager.Editor.Tests
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using com.nickmaltbie.ScreenManager.TestCommon;
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools;

    /// <summary>
    /// Test screen component for evaluating the UIManager and game screens.
    /// </summary>
    public class TestScreenComponent : MonoBehaviour, IScreenComponent
    {
        /// <summary>
        /// Dictionary of loaded counts.
        /// </summary>
        public static ConcurrentDictionary<string, int> globalLoaded;

        /// <summary>
        /// Dictionary of unloaded counts.
        /// </summary>
        public static ConcurrentDictionary<string, int> globalUnloaded;

        /// <summary>
        /// Rest all counts.
        /// </summary>
        public static void ResetCounts()
        {
            globalLoaded = new ConcurrentDictionary<string, int>();
            globalUnloaded = new ConcurrentDictionary<string, int>();
        }

        /// <summary>
        /// Number of times this screen has been loaded.
        /// </summary>
        public int timesLoaded
        {
            get => globalLoaded.GetOrAdd(gameObject.name, 0);
            set => globalLoaded.AddOrUpdate(gameObject.name, _ => value, (a, b) => value);
        }

        /// <summary>
        /// Number of times this screen has been unloaded.
        /// </summary>
        public int timesUnloaded
        {
            get => globalUnloaded.GetOrAdd(gameObject.name, 0);
            set => globalUnloaded.AddOrUpdate(gameObject.name, _ => value, (a, b) => value);
        }

        /// <inheritdoc/>
        public void OnScreenLoaded()
        {
            timesLoaded += 1;
        }

        /// <inheritdoc/>
        public void OnScreenUnloaded()
        {
            timesUnloaded += 1;
        }
    }

    /// <summary>
    /// Test to validate actions of UIManager.
    /// </summary>
    [TestFixture]
    public class UIManagerTests : TestBase
    {
        /// <summary>
        /// UIManager for controlling ui actions.
        /// </summary>
        public UIManager uIManager;

        /// <summary>
        /// Various game screens stored in the UI.
        /// </summary>
        public GameScreen[] gameScreens;

        /// <summary>
        /// Screen component for each screen.
        /// </summary>
        public TestScreenComponent[] screenComponents;

        /// <summary>
        /// Initialize the UIManager tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            UIManager.Instance = null;
            TestScreenComponent.ResetCounts();

            int count = 3;
            gameScreens = new GameScreen[count];
            screenComponents = new TestScreenComponent[count];

            for (int i = 0; i < count; i++)
            {
                var go = new GameObject();
                gameScreens[i] = go.AddComponent<GameScreen>();
                var child = new GameObject();
                child.transform.SetParent(go.transform);
                screenComponents[i] = child.AddComponent<TestScreenComponent>();
                go.name = $"screen-{i}";
                child.name = $"child-{i}";

                RegisterGameObject(go);
                RegisterGameObject(child);
            }

            uIManager = new GameObject().AddComponent<UIManager>();
            uIManager.screenPrefabs = gameScreens.ToList();
            RegisterGameObject(uIManager.gameObject);
            uIManager.Start();
        }

        [TearDown]
        public override void TearDown()
        {
            uIManager.OnDestroy();
            base.TearDown();
        }

        /// <summary>
        /// Validate that there can only be one UIManager.
        /// </summary>
        [UnityTest]
        public IEnumerator Validate_UIManagerSingleton()
        {
            var go = new GameObject();
            UIManager secondUIManager = go.AddComponent<UIManager>();
            secondUIManager.Start();

            // Without expecting an error log, the test would fail
            LogAssert.Expect(LogType.Error, new Regex(".*Destroy may not be called from edit mode.*\n.*"));

            // Wait for second UIManager to destroy itself.
            int i = 0;
            while (go != null && i < 10)
            {
                yield return null;
                i++;
            }

            secondUIManager.OnDestroy();
            GameObject.DestroyImmediate(go);
        }

        /// <summary>
        /// Validate PreviousScreen works as expected.
        /// </summary>
        [Test]
        public void Validate_PreviousScreen()
        {
            // Go to previous screen if requested
            uIManager.ClearHistory();

            // Make some history
            uIManager.SetScreen(gameScreens[0].name);
            uIManager.SetScreen(gameScreens[1].name);
            Assert.IsTrue(uIManager.CurrentScreen == gameScreens[1].name);

            // Revert to previous screen
            uIManager.PreviousScreen();
            Assert.IsTrue(uIManager.CurrentScreen == gameScreens[0].name);
            Assert.AreEqual(2, screenComponents[0].timesLoaded);
            Assert.AreEqual(1, screenComponents[0].timesUnloaded);
            Assert.AreEqual(1, screenComponents[1].timesLoaded);
            Assert.AreEqual(2, screenComponents[1].timesUnloaded);

            // no screen change if history is empty
            uIManager.ClearHistory();
            TestScreenComponent.ResetCounts();
            uIManager.PreviousScreen();
            Assert.AreEqual(0, screenComponents[0].timesLoaded);
            Assert.AreEqual(0, screenComponents[0].timesUnloaded);

            // Go to previous screen if requested
            uIManager.SetScreen(gameScreens[1].name);
            uIManager.ClearHistory();
            TestScreenComponent.ResetCounts();

            // Make some history
            uIManager.SetScreen(gameScreens[0].name);
            uIManager.SetScreen(gameScreens[1].name);
            Assert.IsTrue(uIManager.CurrentScreen == gameScreens[1].name);

            // Revert to previous screen
            UIManager.PreviousScreen(this);
            Assert.IsTrue(uIManager.CurrentScreen == gameScreens[0].name);
            Assert.AreEqual(2, screenComponents[0].timesLoaded);
            Assert.AreEqual(1, screenComponents[0].timesUnloaded);
            Assert.AreEqual(1, screenComponents[1].timesLoaded);
            Assert.AreEqual(2, screenComponents[1].timesUnloaded);
        }

        /// <summary>
        /// Validate that the UIManager behaves correctly with a history at max length.
        /// </summary>
        [Test]
        public void Validate_MaxHistoryUIManager()
        {
            // Fill up the screen history
            for (int i = 0; i < uIManager.maxScreenHistory * 2; i++)
            {
                uIManager.SetScreen(gameScreens[i % gameScreens.Length].name);
            }

            // Validate that after maxScreenHistory reverts, there are no more updates
            for (int i = 0; i < uIManager.maxScreenHistory - 2; i++)
            {
                string previous = uIManager.CurrentScreen;
                Debug.Log($"Currently on {previous} screen, going back now");
                uIManager.PreviousScreen();
                Assert.AreNotEqual(previous, uIManager.CurrentScreen);
            }

            TestScreenComponent.ResetCounts();
            uIManager.PreviousScreen();
            Assert.AreEqual(0, screenComponents[0].timesLoaded);
            Assert.AreEqual(0, screenComponents[0].timesUnloaded);
        }

        /// <summary>
        /// Validate the screen change events.
        /// </summary>
        [Test]
        public void Validate_ScreenChangeEvents()
        {
            UIManager.Instance = uIManager;

            ScreenChangeEventArgs recent = null;
            uIManager.ScreenChangeOccur += (_, change) => recent = change;

            UIManager.RequestNewScreen(this, gameScreens[1].name);
            Assert.IsTrue(recent != null && recent.newScreen == gameScreens[1].name);

            recent = null;
            UIManager.RequestNewScreen(this, gameScreens[1].name);
            Assert.IsTrue(recent == null);
        }

        /// <summary>
        /// Validate that UIManager will cleanup properly with no screen count.
        /// </summary>
        [Test]
        public void Validate_UImanagerInvalidScreenCount()
        {
            UIManager.Instance = null;
            uIManager.screenPrefabs = new List<GameScreen>();
            LogAssert.Expect(LogType.Log, "No valid screens to display for UIManager");
            uIManager.Start();
        }

        /// <summary>
        /// Validate that UIManager will cleanup properly on destroy.
        /// </summary>
        [Test]
        public void Validate_UIManagerInvalidScreenStart()
        {
            UIManager.Instance = null;
            uIManager.initialScreen = -1;
            LogAssert.Expect(LogType.Log, $"Initial Screen {uIManager.initialScreen} invalid, defaulting to screen {0}");
            uIManager.Start();
        }

        /// <summary>
        /// Validate that UIManager will cleanup properly on destroy.
        /// </summary>
        [Test]
        public void Validate_UIManagerCleanup()
        {
            uIManager.OnDestroy();

            Assert.AreEqual(null, UIManager.Instance);
        }

        /// <summary>
        /// Validate that the player can switch between screens properly.
        /// </summary>
        [Test]
        public void Validate_InvalidScreenSwap()
        {
            Assert.AreEqual(1, screenComponents[0].timesLoaded);
            Assert.AreEqual(0, screenComponents[0].timesUnloaded);

            uIManager.SetScreen("NOT A SCREEN");

            Assert.AreEqual(1, screenComponents[0].timesLoaded);
            Assert.AreEqual(0, screenComponents[0].timesUnloaded);
        }

        /// <summary>
        /// Validate that the player can switch between screens properly.
        /// </summary>
        [Test]
        public void Validate_ScreenSwitch()
        {
            uIManager.SetScreen(gameScreens[1].name);

            TestScreenComponent.ResetCounts();

            uIManager.SetScreen(gameScreens[0].name);

            Assert.IsTrue(uIManager.CurrentScreen == gameScreens[0].name);
            Assert.AreEqual(1, screenComponents[0].timesLoaded);
            Assert.AreEqual(0, screenComponents[0].timesUnloaded);
            Assert.AreEqual(1, screenComponents[1].timesUnloaded);

            uIManager.SetScreen(gameScreens[0].name);
            Assert.AreEqual(1, screenComponents[0].timesLoaded);
            Assert.AreEqual(0, screenComponents[0].timesUnloaded);

            uIManager.SetScreen(gameScreens[2].name);
            Assert.AreEqual(1, screenComponents[2].timesLoaded);
            Assert.AreEqual(0, screenComponents[2].timesUnloaded);
            Assert.AreEqual(1, screenComponents[0].timesLoaded);
            Assert.AreEqual(1, screenComponents[0].timesUnloaded);

            uIManager.SetScreen(gameScreens[0].name);
            Assert.AreEqual(2, screenComponents[0].timesLoaded);
            Assert.AreEqual(1, screenComponents[0].timesUnloaded);
            Assert.AreEqual(1, screenComponents[2].timesLoaded);
            Assert.AreEqual(1, screenComponents[2].timesUnloaded);

            uIManager.SetScreen(gameScreens[1].name);
            Assert.AreEqual(1, screenComponents[1].timesLoaded);
            Assert.AreEqual(1, screenComponents[1].timesUnloaded);
            Assert.AreEqual(2, screenComponents[0].timesLoaded);
            Assert.AreEqual(2, screenComponents[0].timesUnloaded);
        }
    }
}
