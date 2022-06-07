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
`https://github.com/nicholas-maltbie/ScreenManager.git`

For more details about installing a project via git, see unity's documentation
on [Installing form a Git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html#:~:text=%20Select%20Add%20package%20from%20git%20URL%20from,repository%20directly%20rather%20than%20from%20a%20package%20registry.).

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
