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
using nickmaltbie.ScreenManager.TestCommon;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static nickmaltbie.ScreenManager.Actions.PerformActionOnButtonPress;
using static UnityEngine.InputSystem.InputAction;

namespace nickmaltbie.ScreenManager.Editor.Tests.Actions
{
    public class UnityEventTest : UnityEvent<CallbackContext>
    {

    }

    [TestFixture]
    public class PerformActionOnButtonPressTests : ScreenManagerTestBase
    {
        [Test]
        public void Validate_PerformActionOnButtonPressTests()
        {
            var go = new GameObject();
            PerformActionOnButtonPress perform = go.AddComponent<PerformActionOnButtonPress>();
            RegisterGameObject(go);

            Gamepad gamepad = InputSystem.AddDevice<Gamepad>();
            PlayerInput input = go.AddComponent<PlayerInput>();
            InputActionAsset inputActionAsset = ScriptableObject.CreateInstance<InputActionAsset>();
            InputActionMap actionMap = inputActionAsset.AddActionMap("testMap");
            InputAction testAction1 = actionMap.AddAction("testAction1", InputActionType.Value, gamepad.aButton.path);
            InputAction testAction2 = actionMap.AddAction("testAction2", InputActionType.Button, gamepad.bButton.path);

            var evt1 = new UnityEventTest();
            var evt2 = new UnityEventTest();

            int action1 = 0;
            int action2 = 0;

            evt1.AddListener(_ => action1++);
            evt2.AddListener(_ => action2++);

            perform.actionEvents = new[]
            {
                new ActionEvent
                {
                    inputAction = InputActionReference.Create(testAction1),
                    OnPressed = evt1,
                },
                new ActionEvent
                {
                    inputAction = InputActionReference.Create(testAction2),
                    OnPressed = evt2,
                },
            };

            testAction1.Enable();
            testAction2.Enable();

            Press(gamepad.aButton);
            Assert.AreEqual(0, action1);
            Assert.AreEqual(0, action2);
            Release(gamepad.aButton);

            perform.OnScreenLoaded();

            Press(gamepad.aButton);
            Assert.AreEqual(1, action1);
            Assert.AreEqual(0, action2);
            Release(gamepad.aButton);

            perform.OnScreenUnloaded();

            Press(gamepad.aButton);
            Assert.IsTrue(action1 == 1);
            Assert.IsTrue(action2 == 0);
            Release(gamepad.aButton);

            InputSystem.RemoveDevice(gamepad);
        }
    }
}
