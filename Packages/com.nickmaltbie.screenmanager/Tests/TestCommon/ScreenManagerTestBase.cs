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

using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace nickmaltbie.ScreenManager.TestCommon
{
    /// <summary>
    /// Basic unity test base class.
    /// </summary>
    public class ScreenManagerTestBase : InputTestFixture
    {
        /// <summary>
        /// List of game objects created as part of the test.
        /// </summary>
        protected List<GameObject> gameObjects;

        /// <summary>
        /// Setup a basic test.
        /// </summary>
        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, UnityEditor.SceneManagement.NewSceneMode.Single);
            }
#endif

            gameObjects = new List<GameObject>();
        }

        /// <summary>
        /// Cleanup created objects in the basic test.
        /// </summary>
        [TearDown]
        public override void TearDown()
        {
            base.TearDown();

            while (gameObjects.Count > 0)
            {
                GameObject go = gameObjects[0];
                gameObjects.RemoveAt(0);

                GameObject.DestroyImmediate(go);
            }
        }

        /// <summary>
        /// Cleanup created objects in the basic test.
        /// </summary>
        [UnityTearDown]
        public virtual void UnityTearDown()
        {
            while (gameObjects.Count > 0)
            {
                GameObject go = gameObjects[0];
                gameObjects.RemoveAt(0);

                GameObject.DestroyImmediate(go);
            }
        }

        /// <summary>
        /// Create a game object and save it to cleanup at the end of the test.
        /// </summary>
        /// <param name="original">Original object to create from base.</param>
        /// <returns>Created game object.</returns>
        protected GameObject CreateGameObject(GameObject original)
        {
            var go = GameObject.Instantiate(original);
            RegisterGameObject(go);
            return go;
        }

        /// <summary>
        /// Create a game object and save it to cleanup at the end of the test.
        /// </summary>
        /// <returns>Created game object.</returns>
        protected GameObject CreateGameObject()
        {
            var go = new GameObject();
            RegisterGameObject(go);
            return go;
        }

        /// <summary>
        /// Register a game object as part of a test to cleanup once the test is completed.
        /// </summary>
        /// <param name="go">Game object to register.</param>
        protected void RegisterGameObject(GameObject go)
        {
            gameObjects.Add(go);
        }
    }
}
