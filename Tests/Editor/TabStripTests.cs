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

using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace nickmaltbie.ScreenManager.Editor.Tests
{
    [TestFixture]
    public class TabStripTests
    {
        public TabStrip tabStrip;
        public Button[] tabSelectors;
        public CanvasGroup[] tabPanels;

        [SetUp]
        public void Setup()
        {
            var go = new GameObject();
            tabStrip = go.AddComponent<TabStrip>();

            // Setup a tab collection
            int tabs = 3;
            var pairs = new TabPair[tabs];
            tabSelectors = new Button[tabs];
            tabPanels = new CanvasGroup[tabs];
            for (int i = 0; i < tabs; i++)
            {
                pairs[i] = new TabPair();
                pairs[i].TabButton = new GameObject().AddComponent<Button>();
                pairs[i].TabButton.image = pairs[i].TabButton.gameObject.AddComponent<Image>();
                pairs[i].TabContent = new GameObject().AddComponent<CanvasGroup>();

                tabPanels[i] = pairs[i].TabContent;
                tabSelectors[i] = pairs[i].TabButton;
            }

            tabStrip.TabCollection = pairs;
            tabStrip.DefaultTab = pairs[0].TabButton;

            // Setup tab strip
            tabStrip.Start();
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(tabStrip.gameObject);
            foreach (Button button in tabSelectors)
            {
                GameObject.DestroyImmediate(button.gameObject);
            }

            foreach (CanvasGroup group in tabPanels)
            {
                GameObject.DestroyImmediate(group.gameObject);
            }
        }

        [Test]
        public void TestSelectTabs()
        {
            // Simulate selecting first button
            tabSelectors[0].onClick?.Invoke();
            Assert.IsTrue(tabStrip.CurrentTabIndex == 0);
            // Simulate selecting second button
            tabSelectors[1].onClick?.Invoke();
            Assert.IsTrue(tabStrip.CurrentTabIndex == 1);
        }

        [Test]
        public void TestSetupWithDefault()
        {
            tabStrip.DefaultTab = new GameObject().AddComponent<Button>();
            tabStrip.Start();
            GameObject.DestroyImmediate(tabStrip.DefaultTab.gameObject);
        }
    }
}
