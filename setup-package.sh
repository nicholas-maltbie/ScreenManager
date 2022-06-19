# Check if there are changes
if [[ `git status --porcelain` ]]; then
  echo "Will not setup package if branch has changes" 1>&2
  exit 1
fi

# Move to temporary branch
git branch -D temp-branch
git checkout -b temp-branch

# Sets up unity package samples
git mv ./Assets/Samples ./Packages/com.nickmaltbie.screenmanager/Samples~
git mv ./Packages/com.nickmaltbie.screenmanager/* _keep/

git lfs install

git config --global user.email "github-actions[bot]@users.noreply.github.com"
git config --global user.name "github-actions[bot]"

git commit -m "Moved ./Assets/Samples to ./Packages/com.nickmaltbie.screenmanager/Samples~"

# Reset all other changes
git rm -rf .
git checkout HEAD -- _keep

# Move files from _keep to root folder
git mv _keep/* .# Check if there are changes
if [[ `git status --porcelain` ]]; then
  echo "Will not setup package if branch has changes" 1>&2
  exit 1
fi

# Move to temporary branch
git branch -D temp-branch
git checkout -b temp-branch

# Sets up unity package samples
git mv ./Assets/Samples ./Packages/com.nickmaltbie.screenmanager/Samples~
git mv ./Packages/com.nickmaltbie.screenmanager/* _keep/

git lfs install

git config --global user.email "github-actions[bot]@users.noreply.github.com"
git config --global user.name "github-actions[bot]"

git commit -m "Moved ./Assets/Samples to ./Packages/com.nickmaltbie.screenmanager/Samples~"

# Reset all other changes
git rm -rf .
git checkout HEAD -- _keep

# Move files from _keep to root folder
git mv _keep/* .

git commit -m "Setup files for release"
