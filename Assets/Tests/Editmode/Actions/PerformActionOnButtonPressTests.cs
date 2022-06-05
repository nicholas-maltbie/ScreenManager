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

using System.Collections;
using System.Linq;
using nickmaltbie.ScreenManager.Actions;
using nickmaltbie.ScreenManager.Events;
using nickmaltbie.ScreenManager.TestCommon;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using static nickmaltbie.ScreenManager.Actions.PerformActionOnButtonPress;
using static UnityEngine.InputSystem.InputAction;

namespace nickmaltbie.ScreenManager.Tests.EditMode.Actions
{
    [TestFixture]
    public class PerformActionOnButtonPressTests : TestBase
    {
        [UnityTest]
        public IEnumerator Validate_PerformActionOnButtonPressTests()
        {
            GameObject go = new GameObject();
            var perform = go.AddComponent<PerformActionOnButtonPress>();
            RegisterGameObject(go);

            var gamepad = InputSystem.AddDevice<Gamepad>();
            var input = go.AddComponent<PlayerInput>();
            var inputActionAsset = ScriptableObject.CreateInstance<InputActionAsset>();
            var actionMap = inputActionAsset.AddActionMap("testMap");
            var testAction1 = actionMap.AddAction("testAction1", InputActionType.Button, gamepad.aButton.path);
            var testAction2 = actionMap.AddAction("testAction2", InputActionType.Button, gamepad.bButton.path);

            var evt1 = new UnityEvent<CallbackContext>();
            var evt2 = new UnityEvent<CallbackContext>();

            int action1 = 0;
            int action2 = 0;

            evt1.AddListener(_ => action1++);
            evt2.AddListener(_ => action2++);

            perform.actionEvents = new []
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

            Set(gamepad.aButton, 1);
            yield return null;
            Assert.IsTrue(action1 == 0);
            Assert.IsTrue(action2 == 0);
            Set(gamepad.aButton, 0);
            yield return null;

            perform.OnScreenLoaded();

            Set(gamepad.aButton, 1);
            yield return null;
            Assert.IsTrue(action1 == 1);
            Assert.IsTrue(action2 == 0);
            Set(gamepad.aButton, 0);
            yield return null;

            perform.OnScreenUnloaded();

            Set(gamepad.aButton, 1);
            yield return null;
            Assert.IsTrue(action1 == 1);
            Assert.IsTrue(action2 == 0);
            Set(gamepad.aButton, 0);
            yield return null;

            InputSystem.RemoveDevice(gamepad);
        }
    }
}
