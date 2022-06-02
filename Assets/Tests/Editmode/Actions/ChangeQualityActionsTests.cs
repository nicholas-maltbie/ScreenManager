using nickmaltbie.ScreenManager.Actions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace nickmaltbie.ScreenManager.Tests.EditMode.Actions
{
    [TestFixture]
    public class ChangeQualityActionsTests
    {
        [Test]
        public void TestChangeQuality()
        {
            // Setup the object
            var qualitySettings = new GameObject().AddComponent<ChangeQualityActions>();
            qualitySettings.qualityDropdown = qualitySettings.gameObject.AddComponent<Dropdown>();

            // Test setup
            qualitySettings.Awake();
            Assert.IsTrue(QualitySettings.GetQualityLevel() == PlayerPrefs.GetInt(ChangeQualityActions.qualityLevelPlayerPrefKey, QualitySettings.GetQualityLevel()));
            Assert.IsTrue(qualitySettings.qualityDropdown.options.Count == QualitySettings.names.Length);
            // Test change value
            qualitySettings.OnQualityLevelChange(1);
            Assert.IsTrue(QualitySettings.GetQualityLevel() == 1);
            qualitySettings.OnQualityLevelChange(2);
            Assert.IsTrue(QualitySettings.GetQualityLevel() == 2);

            // Cleanup
            GameObject.DestroyImmediate(qualitySettings.gameObject);
        }
    }
}