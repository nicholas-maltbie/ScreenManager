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

using System.Linq;
using nickmaltbie.ScreenManager.TestCommon;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace nickmaltbie.ScreenManager.Tests.EditMode
{
    [TestFixture]
    public class DisplayControlsTests : TestBase
    {
        [Test]
        public void Validate_DisplayControls()
        {
            GameObject go = new GameObject();
            RegisterGameObject(go);

            DisplayControls dc = go.AddComponent<DisplayControls>();

            var keyboard = InputSystem.AddDevice<Keyboard>();
            var input = go.AddComponent<PlayerInput>();
            var inputActionAsset = ScriptableObject.CreateInstance<InputActionAsset>();
            var actionMap = inputActionAsset.AddActionMap("testMap");
            var testAction1 = actionMap.AddAction("testAction1", InputActionType.Button, keyboard.qKey.path, interactions: "Hold");
            var testAction2 = actionMap.AddAction("testAction2", InputActionType.Button, keyboard.eKey.path, interactions: "Hold");
            var testComposite = actionMap.AddAction("testComposite", InputActionType.Value);
            testComposite.AddCompositeBinding("Axis")
                .With("Positive", "<Keyboard>/w")
                .With("Negative", "<Keyboard>/s");
            input.actions = inputActionAsset;

            // Setup rebinding object
            dc.inputActions = new [] 
            {
                InputActionReference.Create(testAction1),
                InputActionReference.Create(testAction2),
                InputActionReference.Create(testComposite)
            };

            dc.Start();
            var lines = go.GetComponent<UnityEngine.UI.Text>().text.Split('\n');

            Debug.Log(string.Join("\n", lines));

            Assert.IsTrue(lines.Any(line => line.StartsWith(testAction1.name)));
            Assert.IsTrue(lines.Any(line => line.StartsWith(testAction2.name)));
            Assert.IsTrue(lines.Any(line => line.StartsWith(testComposite.name)));
            
            dc.OnScreenLoaded();
            dc.OnScreenUnloaded();

            InputSystem.RemoveDevice(keyboard);
        }
    }
}
