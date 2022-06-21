# Changelog

All notable changes to this project will be documented in this file.

## In Progress

## [3.0.0] - 2022-06-08

Minor changes to documentation
* Included information on how to reference a specific tag of the project
    when importing the library.
* Added changelog to auto-generated doc fx website.

Minor Bugfixes
* Corrected some import errors when project is imported

Major Refactoring
* Updated names of assemblies from `com.nickmaltbie.ScreenManager` to just
    `nickmaltbie.ScreenManager` to be more consistent with naming conventions.
    Project folder is still called `com.nickmaltbie.ScreenManager` to follow
    convention from other unity packages with names `com.unity.xyz`.
* Setup sub assembly for `nickmaltbie.ScreenManager.Actions` to sort out the
    actions and resolve some import errors that I ran into earlier.
* Regenerated meta files to stop some broken import errors for package.

## [2.0.1] - 2022-06-07

Minor changes to documentation
* Improved documentation on how to setup project form git URL.
* Also added testables section to package manifest.

## [2.0.0] - 2022-06-06

### Refactored

Adjusted project to follow proper package layout so it can be imported from the
git URL.
