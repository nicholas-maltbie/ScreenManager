using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.ScreenManager.Tests.EditMode.Actions
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
        private string currentScreen => this.uiManager.CurrentScreen;

        /// <summary>
        /// List of supported screen names
        /// </summary>
        private string[] screenNames;

        [SetUp]
        public void Setup()
        {
            this.menuControllerObject = new GameObject();
            this.uiManager = this.menuControllerObject.AddComponent<UIManager>();
            this.menuController = this.menuControllerObject.AddComponent<MenuController>();

            this.uiManager.screenPrefabs = new List<GameScreen>();
            this.menuController.actionDelay = -10.0f;

            this.screenNames = new string[10];
            for (int i = 0; i < screenNames.Length; i++)
            {
                this.screenNames[i] = "Screen " + i.ToString();
                GameObject screen = new GameObject();
                screen.name = this.screenNames[i];
                screen.transform.parent = this.menuControllerObject.transform;
                this.uiManager.screenPrefabs.Add(screen.AddComponent<GameScreen>());
            }

            this.uiManager.Start();
        }

        [TearDown]
        public void TearDown()
        {
            // Cleanup game object
            this.uiManager.OnDestroy();
            GameObject.DestroyImmediate(this.menuControllerObject);
        }

        [Test]
        public void SetScreenTests()
        {
            this.menuController.SetScreen(screenNames[0]);
            Assert.IsTrue(this.currentScreen == screenNames[0]);

            this.menuController.SetScreen(screenNames[1]);
            Assert.IsTrue(this.currentScreen == screenNames[1]);

            this.menuController.SetScreen(this.uiManager.screenPrefabs[2].gameObject);
            Assert.IsTrue(this.currentScreen == screenNames[2]);

            this.menuController.PreviousScreen();
            Assert.IsTrue(this.currentScreen == screenNames[1]);
        }

        [Test]
        public void TestRestrictScreenChanges()
        {
            this.menuController.SetScreen(screenNames[0]);
            Assert.IsTrue(this.currentScreen == screenNames[0]);

            this.menuController.allowInputChanges = false;
            this.menuController.SetScreen(screenNames[1]);
            Assert.IsTrue(this.currentScreen == screenNames[0]);

            this.menuController.allowInputChanges = true;
            this.menuController.SetScreen(screenNames[1]);
            Assert.IsTrue(this.currentScreen == screenNames[1]);

            this.menuController.allowInputChanges = false;
            this.menuController.PreviousScreen();
            Assert.IsTrue(this.currentScreen == screenNames[1]);

            this.menuController.allowInputChanges = true;
            this.menuController.PreviousScreen();
            Assert.IsTrue(this.currentScreen == screenNames[0]);
        }
    }
}