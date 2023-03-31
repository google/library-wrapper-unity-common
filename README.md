# Library Wrapper Unity Common Library

[![openupm](https://img.shields.io/npm/v/com.google.librarywrapper.java?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.google.librarywrapper.java/)

[Library Wrapper](https://developer.android.com/games/develop/custom/wrapper) is
a code generator that parses Java API and produces wrapper APIs in Unity C#. It
does this by creating wrappers around every class in the Java API, handling the
details of JNI calls, signatures, and memory management.

This Unity package contains library code shared by all Library Wrapper generated
C# code. Every Unity package containing Library Wrapper generated code should
list this package as a dependency.

## Installation

This package can be installed with [OpenUPM](https://openupm.com/packages/com.google.librarywrapper.java/):

```shell
$ openupm add com.google.librarywrapper.java
```

This package can also be installed via the Unity Package Manager. See
[this guide](https://docs.unity3d.com/Manual/upm-ui-giturl.html) for details.

This package can be installed by downloading this repo and extract its content
to your project's `Packages/` folder
([embedded package](https://docs.unity3d.com/Manual/upm-ui-local.html)).

## How-tos

### Converting to `AndroidJavaObject`

Sometimes, a method you want to call is missing from the wrapper object. You can get around this by converting the wrapper object to `AndroidJavaObject` and call it manually:

```c#
SomeExternalType objectWrapper;  // Received from some method in your library.
AndroidJavaObject wrapper = Google.LibraryWrapper.Java.Utils.ToAndroidJavaObject(objectWrapper);
wrapper.Call("someMethod");
```

### Implementing callbacks

Every wrapped Java interface can be implemented on the C# side and passed back into the Java side to receive callbacks. Every interface `Foo` will have an accompanying `FooCallbackHelper`. Extend the callback helper class and pass it back via the appropriate method.

For a sample interface in Java:

```java
public interface Foo {
  int add(int a, int b);
}
```

We can implement it like this in C#:

```c#
public FooImpl : FooCallbackHelper {
  public int Add(int a, int b) {
    return a + b;
  }
}
```

Do not implement the interface directly.