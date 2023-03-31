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
#if !UNITY_2022_2_OR_NEWER
using System.Reflection;
#endif

using UnityEngine;

namespace Google.LibraryWrapper.Java
{
    public class JavaObject
    {
        private static readonly IntPtr _classObject;

        private static readonly IntPtr _equalsMethodId;

        private static readonly IntPtr _hashCodeMethodId;

        private static readonly IntPtr _toStringMethodId;

        protected IntPtr _rawObject;

        static JavaObject()
        {
            AndroidJNI.AttachCurrentThread();
            IntPtr classObject = AndroidJNI.FindClass("java/lang/Object");
            _classObject = AndroidJNI.NewGlobalRef(classObject);
            AndroidJNI.DeleteLocalRef(classObject);
            _equalsMethodId =
                    AndroidJNI.GetMethodID(_classObject, "equals", "(Ljava/lang/Object;)Z");
            _hashCodeMethodId = AndroidJNI.GetMethodID(_classObject, "hashCode", "()I");
            _toStringMethodId =
                    AndroidJNI.GetMethodID(_classObject, "toString", "()Ljava/lang/String;");
        }

        public JavaObject(IntPtr rawObject) {}

        ~JavaObject()
        {
            if (_rawObject != IntPtr.Zero)
            {
                AndroidJNI.DeleteGlobalRef(_rawObject);
            }
        }

        public static IntPtr GetRawClass()
        {
            return AndroidJNI.NewLocalRef(_classObject);
        }

        public IntPtr GetRawObject()
        {
            return AndroidJNI.NewLocalRef(_rawObject);
        }

        public override bool Equals(object obj)
        {
            try
            {
                AndroidJNI.PushLocalFrame(0);
                if (obj is JavaObject)
                {
                    jvalue[] args = new jvalue[] { ToJvalue(obj) };
                    return AndroidJNI.CallBooleanMethod(_rawObject, _equalsMethodId, args);
                }
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return AndroidJNI.CallIntMethod(_rawObject, _hashCodeMethodId, null);
        }

        public override string ToString()
        {
            return AndroidJNI.CallStringMethod(_rawObject, _toStringMethodId, null);
        }

        protected static jvalue ToJvalue(object argument)
        {
            if (argument == null)
            {
                return new jvalue { l = IntPtr.Zero };
            }
            else if (argument is string)
            {
                return new jvalue { l = Utils.NewString((string)argument) };
            }
            else
            {
                IntPtr rawObject =
                        (IntPtr)argument
                                .GetType()
                                .GetMethod("GetRawObject", Type.EmptyTypes)
                                .Invoke(argument, null);
                return new jvalue { l = rawObject };
            }
        }

        protected internal AndroidJavaObject ToAndroidJavaObject()
        {
            #if UNITY_2022_2_OR_NEWER
            return new AndroidJavaObject(_rawObject);
            #else
            return
                    (AndroidJavaObject)typeof(AndroidJavaObject)
                            .GetConstructor(
                                BindingFlags.NonPublic | BindingFlags.Instance,
                                null,
                                new Type[] { typeof(IntPtr) },
                                null)
                            .Invoke(new object[] { _rawObject });
            #endif
        }
    }
}
