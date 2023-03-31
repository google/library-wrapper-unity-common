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
        private static AndroidJavaObject _unityActivityObject;

        static Utils()
        {
            AndroidJNI.AttachCurrentThread();
        }

        public static T CreateGenericInstance<T>(IntPtr rawObject)
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { rawObject });
        }

        public static AndroidJavaObject GetUnityActivity()
        {
            if (_unityActivityObject == null)
            {
                AndroidJavaClass unityPlayerClass =
                        new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                _unityActivityObject =
                        unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return _unityActivityObject;
        }

        public static AndroidJavaObject ToAndroidJavaObject(object obj)
        {
            // Boxes C# null, primitives, string, and JavaObject instances into AndroidJavaObject.
            if (obj == null)
            {
                return null;
            }
            else if (obj is JavaObject)
            {
                return ((JavaObject)obj).ToAndroidJavaObject();
            }
            else if (obj is string)
            {
                return new AndroidJavaObject("java.lang.String", obj);
            }
            else if (obj is byte)
            {
                #if UNITY_2019_1_OR_NEWER
                return new AndroidJavaObject("java.lang.Byte", (sbyte)obj);
                #else
                return new AndroidJavaObject("java.lang.Byte", obj);
                #endif
            }
            else if (obj is sbyte)
            {
                #if UNITY_2019_1_OR_NEWER
                return new AndroidJavaObject("java.lang.Byte", obj);
                #else
                return new AndroidJavaObject("java.lang.Byte", (byte)obj);
                #endif
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

        public static sbyte CallStaticSByteMethod(IntPtr obj, IntPtr methodID, jvalue[] args)
        {
            #if UNITY_2019_1_OR_NEWER
            return AndroidJNI.CallStaticSByteMethod(obj, methodID, args);
            #else
            return (sbyte)AndroidJNI.CallStaticByteMethod(obj, methodID, args);
            #endif
        }

        public static sbyte CallSByteMethod(IntPtr obj, IntPtr methodID, jvalue[] args)
        {
            #if UNITY_2019_1_OR_NEWER
            return AndroidJNI.CallSByteMethod(obj, methodID, args);
            #else
            return (sbyte)AndroidJNI.CallByteMethod(obj, methodID, args);
            #endif
        }

        public static sbyte GetStaticSByteField(IntPtr obj, IntPtr fieldID)
        {
            #if UNITY_2019_1_OR_NEWER
            return AndroidJNI.GetStaticSByteField(obj, fieldID);
            #else
            return (sbyte)AndroidJNI.GetStaticByteField(obj, fieldID);
            #endif
        }

        public static sbyte GetSByteField(IntPtr obj, IntPtr fieldID)
        {
            #if UNITY_2019_1_OR_NEWER
            return AndroidJNI.GetSByteField(obj, fieldID);
            #else
            return (sbyte)AndroidJNI.GetByteField(obj, fieldID);
            #endif
        }

        public static void SetStaticSByteField(IntPtr obj, IntPtr fieldID, sbyte val)
        {
            #if UNITY_2019_1_OR_NEWER
            AndroidJNI.SetStaticSByteField(obj, fieldID, val);
            #else
            AndroidJNI.SetStaticByteField(obj, fieldID, (byte)val);
            #endif
        }

        public static void SetSByteField(IntPtr obj, IntPtr fieldID, sbyte val)
        {
            #if UNITY_2019_1_OR_NEWER
            AndroidJNI.SetSByteField(obj, fieldID, val);
            #else
            AndroidJNI.SetByteField(obj, fieldID, (byte)val);
            #endif
        }

        public static IntPtr NewString(string chars)
        {
            #if UNITY_2019_2_OR_NEWER
            return AndroidJNI.NewString(chars);
            #else
            return AndroidJNI.NewStringUTF(chars);
            #endif
        }
    }
}
