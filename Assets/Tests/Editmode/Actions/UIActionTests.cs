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
            uiHolder = new GameObject();
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(uiHolder);
        }

        [Test]
        public void QuitGameActionTests()
        {
            uiHolder.AddComponent<QuitGameAction>();
            QuitGameAction action = uiHolder.GetComponent<QuitGameAction>();
            // Just call the method
            action.QuitGame();
        }

        [Test]
        public void CursorStateOnMenuLoadTests()
        {
            // Setup menu cursor state that reveals and unlock the cursor
            CursorStateOnMenuLoad cursorStateOnMenuLoad = uiHolder.AddComponent<CursorStateOnMenuLoad>();
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
