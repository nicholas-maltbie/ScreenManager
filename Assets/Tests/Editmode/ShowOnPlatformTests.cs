﻿// Copyright (C) 2022 Nicholas Maltbie
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

using nickmaltbie.ScreenManager.TestCommon;
using NUnit.Framework;
using UnityEngine;

namespace nickmaltbie.ScreenManager.Tests.EditMode
{
    [TestFixture]
    public class ShowOnPlatformTests : TestBase
    {
        [Test]
        public void Validate_ShowOnPlatform()
        {
            RuntimePlatform appPlatform = RuntimePlatform.WebGLPlayer;
            var go = new GameObject();
            ShowOnPlatform show = go.AddComponent<ShowOnPlatform>();
            show.getPlatform = () => appPlatform;

            show.showPlatform = RuntimePlatform.WebGLPlayer;
            show.Awake();

            Assert.IsTrue(go.activeSelf);

            show.showPlatform = RuntimePlatform.LinuxPlayer;
            show.Awake();

            Assert.IsFalse(go.activeSelf);

            appPlatform = RuntimePlatform.LinuxPlayer;
            show.Awake();

            Assert.IsTrue(go.activeSelf);

            RegisterGameObject(go);
        }
    }
}