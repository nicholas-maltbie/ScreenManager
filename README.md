# ScreenManager

ScreenManager is an open source project hosted at
[https://github.com/nicholas-maltbie/ScreenManager](https://github.com/nicholas-maltbie/ScreenManager)

This is an open source project licensed under a [MIT License](LICENSE.txt).
Feel free to use a build of the project for your own work. If you see an error
in the project or have any suggestions, write an issue or make a pull request,
I'll happy include any suggestions or ideas into the project.

You can see a demo of the project running here:
[https://nickmaltbie.com/ScreenManager/](https://nickmaltbie.com/ScreenManager/).
The project hosted on the website is up to date with the most recent
version on the `main` branch of this github repo
and is automatically deployed with each update to the codebase.

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

If you want to reference a specific tag of the project such as version `v3.0.3`,
add a `#release/v3.0.3` to the end of the git URL. An example of importing `v3.0.3`
would look like this:
`https://github.com/nicholas-maltbie/ScreenManager.git#release/v3.0.3`

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

## Documentation

Documentation on the project and scripting API is found at
[https://nickmaltbie.com/ScreenManager/docs/](https://nickmaltbie.com/ScreenManager/docs/)
for the latest version of the codebase.

To view the documentation from a local build of the project install
[DocFX](https://dotnet.github.io/docfx/), use the
following command from the root of the repo.

```bash
Documentation/build.sh
```

(Or this for windows)

```cmd
.\Documentation\build.cmd
```

The documentation for the project is stored in the folder `/Documentation`
and can be modified and changed to update with the project.

_This documentation project is inspired by the project by Norman Erwan's
[DocFxForUnity](https://github.com/NormandErwan/DocFxForUnity)_

## Development

If you want to help with the project, feel free to make some
changes and submit a PR to the repo.

This project is developed using Unity Release [2021.1.19f1](https://unity3d.com/unity/whats-new/2021.1.19).
Install this version of Unity from Unity Hub using this link:
[unityhub://2021.1.19f1/d0d1bb862f9d](unityhub://2021.1.19f1/d0d1bb862f9d).

### Git LFS Setup

Ensure that you also have git lfs installed. It should be setup to auto-track
certain types of files as determined in the `.gitattributes` file. If the
command to install git-lfs `git lfs install` is giving you trouble, try
looking into the [installation guide](https://git-lfs.github.com/).

Once git lfs is installed, from in the repo, run the following command to pull
objects for development.

```bash
git lfs pull
```

### Githooks Setup

When working with the project, make sure to setup the `.githooks` if
you want to edit the code in the project. In order to do this, use the
following command to reconfigure the `core.hooksPath` for your repository

```bash
git config --local core.hooksPath .githooks
```
