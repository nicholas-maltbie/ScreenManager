$current_sha=$(git rev-parse --verify HEAD)

# Checkout specific tag if one is provided
if [ ! -z "$1" ]
then
  echo "Attempting to make release for tag $1"
  if git rev-parse "$1" >/dev/null 2>&1; then
    echo "Found tag $1, checking out changes"
    git checkout "$1"
  else
    echo "Tag $1 does not exist, aborting changes" 1>&2
    exit 1
  fi
fi

# Check if there are changes
if [[ `git status --porcelain` ]]; then
  echo "Will not setup package if branch has changes" 1>&2
  exit 1
fi

# Move to temporary branch
git branch -D temp-branch
git checkout -b temp-branch

git lfs install

git config --global user.email "github-actions[bot]@users.noreply.github.com"
git config --global user.name "github-actions[bot]"

# Sets up unity package samples
git mv ./Assets/Samples ./Packages/com.nickmaltbie.screenmanager/Samples~

git commit -m "Moved ./Assets/Samples to ./Packages/com.nickmaltbie.screenmanager/Samples~"

# Reset all other changes
git rm -rf .
git checkout HEAD -- ./Packages/com.nickmaltbie.screenmanager

# Keep .gitattributes for lfs files
git checkout HEAD -- .gitattributes

git commit -m "Filtered for only package files"

# Move files from _keep to root folder
git mv ./Packages/com.nickmaltbie.screenmanager/* .

git commit -m "Setup files for release"

# Push changes to repo if tag was provided
if [ ! -z "$1" ]
then
  # Push changes to original repo
  git branch -D "release/$1"
  git branch -m "release/$1"
  git push --set-upstream origin "release/$1" --force

  # Cleanup any files in the repo we don't care about
  git checkout .
  git clean -xdf .
  git checkout "$current_sha"
fi
