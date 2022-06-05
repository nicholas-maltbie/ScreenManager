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
using nickmaltbie.ScreenManager.Actions;
using nickmaltbie.ScreenManager.TestCommon;
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.ScreenManager.Tests.EditMode.Actions
{
    internal class ValidationBinding : MonoBehaviour, IBindingControl
    {
        public int resetCount = 0;
        public int updateCount = 0;
        public void ResetBinding()
        {
            resetCount++;
        }

        public void UpdateDisplay()
        {
            updateCount++;
        }
    }

    [TestFixture]
    public class ResetBindingButtonTests : TestBase
    {
        [Test]
        public void Validate_ResetBindings()
        {
            var go = new GameObject();
            ResetBindingsButton reset = go.AddComponent<ResetBindingsButton>();
            UnityEngine.UI.Button button = go.AddComponent<UnityEngine.UI.Button>();
            reset.button = button;
            RegisterGameObject(go);
            var bindings = Enumerable.Range(1, 10).Select(_ =>
            {
                var child = new GameObject();
                child.transform.SetParent(go.transform);
                RegisterGameObject(child);
                return child.AddComponent<ValidationBinding>();
            }).ToList();

            reset.Start();
            button.onClick?.Invoke();
            Assert.IsTrue(bindings.All(b => b.resetCount == 1 && b.updateCount == 1));
            button.onClick?.Invoke();
            Assert.IsTrue(bindings.All(b => b.resetCount == 2 && b.updateCount == 2));
        }
    }
}
