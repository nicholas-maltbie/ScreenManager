using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace nickmaltbie.ScreenManager.Tests.EditMode
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
            GameObject go = new GameObject();
            this.tabStrip = go.AddComponent<TabStrip>();

            // Setup a tab collection
            int tabs = 3;
            TabPair[] pairs = new TabPair[tabs];
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
            this.tabStrip.TabCollection = pairs;
            this.tabStrip.DefaultTab = pairs[0].TabButton;

            // Setup tab strip
            this.tabStrip.Start();
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(this.tabStrip.gameObject);
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
            this.tabSelectors[0].onClick?.Invoke();
            Assert.IsTrue(this.tabStrip.CurrentTabIndex == 0);
            // Simulate selecting second button
            this.tabSelectors[1].onClick?.Invoke();
            Assert.IsTrue(this.tabStrip.CurrentTabIndex == 1);
        }

        [Test]
        public void TestSetupWithDefault()
        {
            this.tabStrip.DefaultTab = new GameObject().AddComponent<Button>();
            this.tabStrip.Start();
            GameObject.DestroyImmediate(this.tabStrip.DefaultTab.gameObject);
        }
    }
}