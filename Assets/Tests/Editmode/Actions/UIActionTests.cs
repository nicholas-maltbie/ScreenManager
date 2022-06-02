using nickmaltbie.ScreenManager.Actions;
using nickmaltbie.ScreenManager.Events;
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.ScreenManager.Tests.EditMode.Actions
{
    /// <summary>
    /// Tests for various UI Actions such as connect, disconnect, quit game actions
    /// </summary>
    [TestFixture]
    public class UIActionTests
    {
        private GameObject uiHolder;

        [SetUp]
        public void Setup()
        {
            this.uiHolder = new GameObject();
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(this.uiHolder);
        }

        [Test]
        public void QuitGameActionTests()
        {
            this.uiHolder.AddComponent<QuitGameAction>();
            QuitGameAction action = this.uiHolder.GetComponent<QuitGameAction>();
            // Just call the method
            action.QuitGame();
        }

        [Test]
        public void CursorStateOnMenuLoadTests()
        {
            // Setup menu cursor state that reveals and unlock the cursor
            CursorStateOnMenuLoad cursorStateOnMenuLoad = this.uiHolder.AddComponent<CursorStateOnMenuLoad>();
            cursorStateOnMenuLoad.cursorLockMode = CursorLockMode.None;
            cursorStateOnMenuLoad.cursorVisible = true;
            // set current state
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // Check to ensure cursor state has been properly updated after enabling this component
            cursorStateOnMenuLoad.OnScreenLoaded();
            Assert.IsTrue(Cursor.lockState == CursorLockMode.None);
            Assert.IsTrue(Cursor.visible == true);
            cursorStateOnMenuLoad.OnScreenUnloaded();
        }
    }
}