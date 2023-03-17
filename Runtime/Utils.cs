// Copyright 2023 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

using UnityEngine;

namespace Google.LibraryWrapper.Java
{
    public static class Utils
    {
        private static AndroidJavaClass _unityPlayerClass;

        private static AndroidJavaObject _unityActivityObject;

        static Utils()
        {
            _unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _unityActivityObject =
                _unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        }

        public static T CreateGenericInstance<T>(IntPtr rawObject)
        {
            #if ENABLE_MONO && !NET_STANDARD
            return (T) (dynamic) rawObject;
            #else
            return (T) Activator.CreateInstance(typeof(T), new object[] { rawObject });
            #endif
        }

        public static AndroidJavaObject GetUnityActivity()
        {
            return _unityActivityObject;
        }

        public static AndroidJavaObject ToAndroidJavaObject(object obj)
        {
            // Boxes C# null, primitives, string, and JavaObject instances into AndroidJavaObject.
            if (obj == null)
            {
                return null;
            }
            else if (obj is JavaObject javaObject)
            {
                return javaObject.ToAndroidJavaObject();
            }
            else if (obj is string)
            {
                return new AndroidJavaObject("java.lang.String", obj);
            }
            else if (obj is byte)
            {
                return new AndroidJavaObject("java.lang.Byte", obj);
            }
            else if (obj is short)
            {
                return new AndroidJavaObject("java.lang.Short", obj);
            }
            else if (obj is int)
            {
                return new AndroidJavaObject("java.lang.Integer", obj);
            }
            else if (obj is long)
            {
                return new AndroidJavaObject("java.lang.Long", obj);
            }
            else if (obj is float)
            {
                return new AndroidJavaObject("java.lang.Float", obj);
            }
            else if (obj is double)
            {
                return new AndroidJavaObject("java.lang.Double", obj);
            }
            else if (obj is char)
            {
                return new AndroidJavaObject("java.lang.Character", obj);
            }
            else if (obj is bool)
            {
                return new AndroidJavaObject("java.lang.Boolean", obj);
            }

            return null;
        }
    }
}
