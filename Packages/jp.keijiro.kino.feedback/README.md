KinoFeedback2
=============

![gif](https://i.imgur.com/xiuJ77y.gif)
![gif](https://i.imgur.com/akNLG6m.gif)

**KinoFeedback2** is an example of a Unity HDRP custom pass that implements an
old-school frame feedback effect.

Installation
------------

This package uses the [scoped registry] feature to resolve package
dependencies. Please add the following sections to the manifest file
(Packages/manifest.json).

[scoped registry]: https://docs.unity3d.com/Manual/upm-scoped.html

To the `scopedRegistries` section:

```
{
  "name": "Keijiro",
  "url": "https://registry.npmjs.com",
  "scopes": [ "jp.keijiro" ]
}
```

To the `dependencies` section:

```
"jp.keijiro.kino.feedback": "1.0.1"
```

After changes, the manifest file should look like below:

```
{
  "scopedRegistries": [
    {
      "name": "Keijiro",
      "url": "https://registry.npmjs.com",
      "scopes": [ "jp.keijiro" ]
    }
  ],
  "dependencies": {
    "jp.keijiro.kino.feedback": "1.0.1",
    ...
```
