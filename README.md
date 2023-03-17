# Library Wrapper Unity Common Library

[Library Wrapper](https://developer.android.com/games/develop/custom/wrapper) is
a code generator that parses Java API and produces wrapper APIs in Unity C#. It
does this by creating wrappers around every class in the Java API, handling the
details of JNI calls, signatures, and memory management.

This Unity package contains library code shared by all Library Wrapper generated
C# code. Every Unity package containing Library Wrapper generated code should
list this package as a dependency by adding the following in the `package.json`
file:

```json
{
  ...
  "dependencies": {
    ...
    "com.google.librarywrapper.java": "0.1.0",
    ...
  },
  ...
}
```

## Installation

This package can be installed from the Unity Package Manager. See
[this guide](https://docs.unity3d.com/Manual/upm-ui-giturl.html) for details.

This package can be installed by downloading this repo and extract its content
to your project's `Packages/` folder
([embedded package](https://docs.unity3d.com/Manual/upm-ui-local.html)).

We are working to support this package on OpenUPM.
