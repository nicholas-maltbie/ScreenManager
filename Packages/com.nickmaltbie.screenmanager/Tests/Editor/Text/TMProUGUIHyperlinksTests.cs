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
using System.Linq;
using com.nickmaltbie.ScreenManager.TestCommon;
using com.nickmaltbie.ScreenManager.Text;
using NUnit.Framework;
using TMPro;
using UnityEngine;

namespace com.nickmaltbie.ScreenManager.Editor.Tests.Text
{
    [TestFixture]
    public class TMProUGUIHyperlinksTests : TestBase
    {
        private TextMeshProUGUI text;
        private TMProUGUIHyperlinks links;

        private int overrideLinkIndex = -1;
        private string lastPressedLink = string.Empty;

        [SetUp]
        public void SetUp()
        {
            // Ensure a camera exists in the scene
            var camera = new GameObject();
            camera.AddComponent<Camera>();
            camera.tag = "MainCamera";

            var canvas = new GameObject();
            canvas.AddComponent<Canvas>();

            var go = new GameObject();
            go.transform.SetParent(canvas.transform);
            text = go.AddComponent<TextMeshProUGUI>();
            links = go.AddComponent<TMProUGUIHyperlinks>();
            text.material = new Material(Shader.Find("TextMeshPro/Mobile/Distance Field"));

            links.getLinkIndex = () => overrideLinkIndex;
            links.openLink = (link) => lastPressedLink = link;

            links.Awake();

            lastPressedLink = string.Empty;

            RegisterGameObject(canvas);
            RegisterGameObject(go);
            RegisterGameObject(camera);
        }

        private void SetText(string text)
        {
            this.text.text = text;
            this.text.ForceMeshUpdate();
        }

        private Color? GetLinkColor(int linkIndex)
        {
            TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];

            var oldVertexColors = new List<Color32[]>(); // Store the old character colors
            Color? color = null;
            int underlineIndex = -1;

            for (int i = 0; i < linkInfo.linkTextLength; i++)
            {
                // For each character in the link string
                int characterIndex = linkInfo.linkTextfirstCharacterIndex + i; // The current character index
                TMP_CharacterInfo charInfo = text.textInfo.characterInfo[characterIndex];
                int meshIndex = charInfo.materialReferenceIndex; // Get the index of the material/subtext object used by this character.
                int vertexIndex = charInfo.vertexIndex; // Get the index of the first vertex of this character.

                // This array contains colors for all vertices of the mesh (might be multiple chars)
                Color32[] vertexColors = text.textInfo.meshInfo[meshIndex].colors32;
                oldVertexColors.Add(new Color32[] { vertexColors[vertexIndex + 0], vertexColors[vertexIndex + 1], vertexColors[vertexIndex + 2], vertexColors[vertexIndex + 3] });
                if (charInfo.isVisible)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        Color32 c1 = vertexColors[vertexIndex];
                        color ??= c1;
                        Assert.IsTrue(c1 == color);
                    }
                }
                // Each line will have its own underline mesh with different index, index == 0 means there is no underline
                if (charInfo.isVisible && charInfo.underlineVertexIndex > 0 && charInfo.underlineVertexIndex != underlineIndex && charInfo.underlineVertexIndex < vertexColors.Length)
                {
                    underlineIndex = charInfo.underlineVertexIndex;
                    for (int j = 0; j < 12; j++) // Underline seems to be always 3 quads == 12 vertices
                    {
                        Color32 c1 = vertexColors[underlineIndex + j];
                        color ??= c1;
                        Assert.IsTrue(c1 == color);
                    }
                }
            }

            return color;
        }

        [Test]
        public void Validate_PressWanderAwayAndBackPressed()
        {
            SetText("<link=\"L1\">L1</link>");
            overrideLinkIndex = 0;
            links.OnPointerDown(null);

            // Now wander away
            overrideLinkIndex = -1;
            links.LateUpdate();
            // Now wander back
            overrideLinkIndex = 0;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.pressedColor);
            links.OnPointerUp(null);
            Assert.IsTrue(lastPressedLink == "L1");
            lastPressedLink = string.Empty;

            overrideLinkIndex = 0;
            links.OnPointerDown(null);

            // Now wander away
            overrideLinkIndex = -1;
            links.LateUpdate();
            // Now wander back
            overrideLinkIndex = 0;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.usedPressedColor);

            links.OnPointerUp(null);
            Assert.IsTrue(lastPressedLink == "L1");
        }

        [Test]
        public void Validate_PressWanderAwayAndBack()
        {
            SetText("<link=\"L1\">L1</link><link=\"L2\">L2</link>");
            overrideLinkIndex = 1;
            links.OnPointerDown(null);
            overrideLinkIndex = 0;

            // Now wander away
            overrideLinkIndex = -1;
            links.LateUpdate();
            // Now wander back
            overrideLinkIndex = 0;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.hoveredColor);
            links.OnPointerUp(null);

            overrideLinkIndex = 0;
            links.OnPointerDown(null);
            links.OnPointerUp(null);
            Assert.IsTrue(lastPressedLink == "L1");
            lastPressedLink = string.Empty;

            overrideLinkIndex = 1;
            links.OnPointerDown(null);
            overrideLinkIndex = 0;

            // Now wander away
            overrideLinkIndex = -1;
            links.LateUpdate();
            // Now wander back
            overrideLinkIndex = 0;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.usedHoveredColor);

            links.OnPointerUp(null);
            Assert.IsTrue(lastPressedLink == string.Empty);
        }

        [Test]
        public void Validate_PressAlreadyPressed()
        {
            SetText("<link=\"L1\">L1</link>");
            overrideLinkIndex = 0;
            links.OnPointerDown(null);
            links.OnPointerUp(null);
            Assert.IsTrue(lastPressedLink == "L1");

            lastPressedLink = string.Empty;

            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.usedHoveredColor);

            SetText("<link=\"L1\">L1</link>");
            overrideLinkIndex = 0;
            links.OnPointerDown(null);
            Assert.IsTrue(GetLinkColor(0) == links.usedPressedColor);
            links.OnPointerUp(null);
            Assert.IsTrue(lastPressedLink == "L1");

            lastPressedLink = string.Empty;

            overrideLinkIndex = -1;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.usedColor);

            SetText("<link=\"L1\">L1</link>");
            overrideLinkIndex = 0;
            links.OnPointerDown(null);
            Assert.IsTrue(GetLinkColor(0) == links.usedPressedColor);
            links.OnPointerUp(null);
            Assert.IsTrue(lastPressedLink == "L1");
        }

        [Test]
        public void Validate_PressCancel()
        {
            SetText("<link=\"L1\">L1</link>");

            // First press nothing
            overrideLinkIndex = -1;
            links.OnPointerDown(null);
            Assert.IsTrue(links.pressedLinkIndex == -1);

            // Start pressing link
            overrideLinkIndex = 0;
            links.OnPointerDown(null);
            Assert.IsTrue(links.pressedLinkIndex == 0);

            // Move off link
            overrideLinkIndex = -1;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.startColors[0].FirstOrDefault());

            // Release cursor
            links.OnPointerUp(null);
            Assert.IsTrue(links.pressedLinkIndex == -1);
            Assert.IsTrue(lastPressedLink == string.Empty);
        }

        [Test]
        public void Validate_HoverOverPressedLink()
        {
            SetText("<link=\"L1\">L1</link>");
            overrideLinkIndex = 0;
            links.OnPointerDown(null);
            links.OnPointerUp(null);
            Assert.IsTrue(lastPressedLink == "L1");

            overrideLinkIndex = -1;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.startColors[0].FirstOrDefault());

            overrideLinkIndex = 0;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.usedHoveredColor);
        }

        [Test]
        public void Validate_MoveBetweenLinks()
        {
            SetText("<link=\"L1\">L1</link>  <link=\"L2\">L2</link>");

            // Hover over first link
            overrideLinkIndex = 0;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.hoveredColor);
            Assert.IsTrue(GetLinkColor(1) == links.startColors[1].FirstOrDefault());

            // Move to second link
            overrideLinkIndex = 1;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.startColors[0].FirstOrDefault());
            Assert.IsTrue(GetLinkColor(1) == links.hoveredColor);

        }

        [Test]
        public void Validate_PressHyperLink()
        {
            SetText("<link=\"https://nickmaltbie.com\"><u>https://nickmaltbie.com</u></link>");

            Assert.IsTrue(GetLinkColor(0) != links.hoveredColor);

            // Hover over link
            overrideLinkIndex = 0;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.hoveredColor);

            // Start pressing link
            links.OnPointerDown(null);
            Assert.IsTrue(lastPressedLink == string.Empty);
            Assert.IsTrue(GetLinkColor(0) == links.pressedColor);
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.pressedColor);

            // Release link
            links.OnPointerUp(null);
            Assert.IsTrue(lastPressedLink == "https://nickmaltbie.com");
            Assert.IsTrue(GetLinkColor(0) == links.usedHoveredColor);
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.usedHoveredColor);

            // Move off link
            overrideLinkIndex = -1;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.usedColor);
        }

        [Test]
        public void Validate_HyperlinkHoveWithUnderline()
        {
            SetText("<link=\"https://nickmaltbie.com\"><u>https://nickmaltbie.com</u></link>");

            Assert.IsTrue(GetLinkColor(0) != links.hoveredColor);

            // Hover over link
            overrideLinkIndex = 0;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.hoveredColor);

            // Stay over link
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.hoveredColor);

            // Move off link
            overrideLinkIndex = -1;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) != links.hoveredColor);
        }

        [Test]
        public void Validate_HyperlinkHover()
        {
            SetText("<link=\"https://nickmaltbie.com\">https://nickmaltbie.com</link>");

            Assert.IsTrue(GetLinkColor(0) != links.hoveredColor);

            // Hover over link
            overrideLinkIndex = 0;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.hoveredColor);

            // Stay over link
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) == links.hoveredColor);

            // Move off link
            overrideLinkIndex = -1;
            links.LateUpdate();
            Assert.IsTrue(GetLinkColor(0) != links.hoveredColor);
        }

        [Test]
        public void Validate_LoadHyperlink()
        {
            SetText("<link=\"https://nickmaltbie.com\">https://nickmaltbie.com</link>");
            Assert.IsTrue(text.textInfo.linkCount == 1, $"Expected to find one link but instead found {text.textInfo.linkCount}");
        }
    }
}
