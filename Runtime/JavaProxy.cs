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
    public abstract class JavaProxy : AndroidJavaProxy
    {
        private IntPtr _rawObject;

        static JavaProxy()
        {
            AndroidJNI.AttachCurrentThread();
        }

        public JavaProxy(string javaInterface) : base(javaInterface) {}

        ~JavaProxy()
        {
            if (_rawObject != IntPtr.Zero)
            {
                AndroidJNI.DeleteGlobalRef(_rawObject);
            }
        }

        public IntPtr GetRawObject()
        {
            if (_rawObject == IntPtr.Zero)
            {
                IntPtr rawObject = AndroidJNIHelper.CreateJavaProxy(this);
                _rawObject = AndroidJNI.NewGlobalRef(rawObject);
                AndroidJNI.DeleteLocalRef(rawObject);
            }
            return AndroidJNI.NewLocalRef(_rawObject);
        }

        public override AndroidJavaObject Invoke(string methodName, object[] args)
        {
            if (methodName == "equals" && args.Length == 1)
            {
                return new AndroidJavaObject(
                        "java.lang.Boolean", Equals(((AndroidJavaObject) args[0]).GetRawObject()));
            }
            else if (methodName == "getHashCode" && args.Length == 0)
            {
                return new AndroidJavaObject("java.lang.Integer", GetHashCode());
            }
            else if (methodName == "toString" && args.Length == 0)
            {
                return new AndroidJavaObject("java.lang.String", ToString());
            }
            return null;
        }

        public override sealed AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] args)
        {
            AndroidJavaObject result = base.Invoke(methodName, args);
            foreach (AndroidJavaObject arg in args)
            {
                if (arg != null)
                {
                    arg.Dispose();
                }
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            return (obj is JavaProxy && _rawObject.Equals(((JavaProxy)obj)._rawObject))
                    || (obj is IntPtr && obj.Equals(_rawObject));
        }

        public override int GetHashCode()
        {
            return _rawObject.ToInt32();
        }

        public override string ToString()
        {
            return GetType() + ": " + _rawObject + " <C# proxy Java object>";
        }
    }
}
