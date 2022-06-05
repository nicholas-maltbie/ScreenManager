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
using nickmaltbie.ScreenManager.TestCommon;
using nickmaltbie.ScreenManager.Text;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.TestTools;

namespace nickmaltbie.ScreenManager.Tests.EditMode.Text
{
    [TestFixture]
    public class TMProUGUIHyperlinksTests : TestBase
    {
        TextMeshProUGUI text;
        TMProUGUIHyperlinks links;

        private int overrideLinkIndex = -1;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            // Ensure a camera exists in the scene
            GameObject camera = new GameObject();
            camera.AddComponent<Camera>();
            camera.tag = "MainCamera";

            GameObject canvas = new GameObject();
            canvas.AddComponent<Canvas>();

            yield return null;

            GameObject go = new GameObject();
            go.transform.SetParent(canvas.transform);
            text = go.AddComponent<TextMeshProUGUI>();
            links = go.AddComponent<TMProUGUIHyperlinks>();
            text.material = new Material(Shader.Find("TextMeshPro/Mobile/Distance Field"));

            text.ForceMeshUpdate();

            links.Awake();

            links.getLinkIndex = () => overrideLinkIndex;
            text.text = "<link=\"https://nickmaltbie.com\">https://nickmaltbie.com</link>";

            go.SetActive(true);

            RegisterGameObject(go);
            RegisterGameObject(camera);
        }

        [UnityTest]
        public IEnumerator Validate_LoadHyperlink()
        {
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            overrideLinkIndex = 0;
            text.ForceMeshUpdate();

            UnityEngine.Debug.Log(text.textInfo.characterCount);

            yield return null;
            yield return null;
            yield return null;
            yield return null;
        }
    }
}
