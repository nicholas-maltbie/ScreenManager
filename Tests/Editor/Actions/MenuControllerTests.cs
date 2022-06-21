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
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.ScreenManager.Editor.Tests.Actions
{
    /// <summary>
    /// Tests for Menu Controller Tests
    /// </summary>
    [TestFixture]
    public class MenuControllerTests
    {
        /// <summary>
        /// Object to hold menu controller
        /// </summary>
        private GameObject menuControllerObject;

        /// <summary>
        /// Menu controller object
        /// </summary>
        private MenuController menuController;

        /// <summary>
        /// UIManager for managing previous screens
        /// </summary>
        private UIManager uiManager;

        /// <summary>
        /// Current screen
        /// </summary>
        private string currentScreen => uiManager.CurrentScreen;

        /// <summary>
        /// List of supported screen names
        /// </summary>
        private string[] screenNames;

        [SetUp]
        public void Setup()
        {
            menuControllerObject = new GameObject();
            uiManager = menuControllerObject.AddComponent<UIManager>();
            menuController = menuControllerObject.AddComponent<MenuController>();

            uiManager.screenPrefabs = new List<GameScreen>();
            menuController.actionDelay = -10.0f;

            screenNames = new string[10];
            for (int i = 0; i < screenNames.Length; i++)
            {
                screenNames[i] = "Screen " + i.ToString();
                var screen = new GameObject();
                screen.name = screenNames[i];
                screen.transform.parent = menuControllerObject.transform;
                uiManager.screenPrefabs.Add(screen.AddComponent<GameScreen>());
            }

            uiManager.Start();
        }

        [TearDown]
        public void TearDown()
        {
            // Cleanup game object
            uiManager.OnDestroy();
            GameObject.DestroyImmediate(menuControllerObject);
        }

        [Test]
        public void SetScreenTests()
        {
            menuController.SetScreen(screenNames[0]);
            Assert.IsTrue(currentScreen == screenNames[0]);

            menuController.SetScreen(screenNames[1]);
            Assert.IsTrue(currentScreen == screenNames[1]);

            menuController.SetScreen(uiManager.screenPrefabs[2].gameObject);
            Assert.IsTrue(currentScreen == screenNames[2]);

            menuController.PreviousScreen();
            Assert.IsTrue(currentScreen == screenNames[1]);
        }

        [Test]
        public void TestRestrictScreenChanges()
        {
            menuController.SetScreen(screenNames[0]);
            Assert.IsTrue(currentScreen == screenNames[0]);

            menuController.allowInputChanges = false;
            menuController.SetScreen(screenNames[1]);
            Assert.IsTrue(currentScreen == screenNames[0]);

            menuController.allowInputChanges = true;
            menuController.SetScreen(screenNames[1]);
            Assert.IsTrue(currentScreen == screenNames[1]);

            menuController.allowInputChanges = false;
            menuController.PreviousScreen();
            Assert.IsTrue(currentScreen == screenNames[1]);

            menuController.allowInputChanges = true;
            menuController.PreviousScreen();
            Assert.IsTrue(currentScreen == screenNames[0]);
        }
    }
}
