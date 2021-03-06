# ScreenManager

ScreenManager is an open source project hosted at
[https://github.com/nicholas-maltbie/ScreenManager](https://github.com/nicholas-maltbie/ScreenManager)

This is an open source project licensed under a [MIT License](LICENSE.md).
Feel free to use a build of the project for your own work. If you see an error
in the project or have any suggestions, write an issue or make a pull request,
I'll happy include any suggestions or ideas into the project.

## Installation

Install the latest version of the project by importing a project via git
at this URL:

```text
https://github.com/nicholas-maltbie/ScreenManager.git#release/latest
```

( Or use this path to import without samples )

```text
https://github.com/nicholas-maltbie/ScreenManager.git?path=/Packages/com.nickmaltbie.screenmanager/
```

If you want to reference a specific tag of the project such as version `v3.0.0`,
add a `#release/v3.0.0` to the end of the git URL. An example of importing `v3.0.0`
would look like this:
`https://github.com/nicholas-maltbie/ScreenManager.git#release/v3.0.0`

For a full list of all tags, check the [ScreenManager Tags](https://github.com/nicholas-maltbie/ScreenManager/tags)
list on github. I will usually associated a tag with each release of the project.

If you do not include a tag, this means that your project will update whenever
you reimport from main. This may cause some errors or problems due to
experimental or breaking changes in the project.

You can also import the project via a tarball if you download the source
code and extract it on your local machine. Make sure to import
via the package manifest defined at `Packages\com.nickmaltbie.screenmanager\package.json`
within the project.

For more details about installing a project via git, see unity's documentation
on [Installing form a Git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html#:~:text=%20Select%20Add%20package%20from%20git%20URL%20from,repository%20directly%20rather%20than%20from%20a%20package%20registry.).

### Scoped Registry Install

If you wish to install the project via a
[Scoped Registry](https://docs.unity3d.com/Manual/upm-scoped.html)
and npm, you can add a scoped registry to your project from all of the
`com.nickmaltbie` packages like this:

```json
"scopedRegistries": [
  {
    "name": "nickmaltbie",
    "url": "https://registry.npmjs.org",
    "scopes": [
      "com.nickmaltbie"
    ]
  }
]
```

Then, if you want to reference a version of the project, you simply
need to include the dependency with a version string and the unity package
manager will be able to download it from the registry at
`https://registry.npmjs.org`

```json
"dependencies": {
  "com.nickmaltbie.screenmanager": "3.0.0",
  "com.unity.inputsystem": "1.0.2",
  "com.unity.textmeshpro": "3.0.6"
  // ... other dependencies
}
```

## Sample

You can see a demo of the project running here:
[https://nickmaltbie.com/ScreenManager/](https://nickmaltbie.com/ScreenManager/).
The project hosted on the website is up to date with the most recent
version on the `main` branch of this github repo
and is automatically deployed with each update to the codebase.

## Documentation

Documentation on the project and scripting API is found at
[https://nickmaltbie.com/ScreenManager/docs/](https://nickmaltbie.com/ScreenManager/docs/)
for the latest version of the codebase.
