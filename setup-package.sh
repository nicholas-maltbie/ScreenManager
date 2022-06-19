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

git lfs install

git config --global user.email "github-actions[bot]@users.noreply.github.com"
git config --global user.name "github-actions[bot]"

git commit -m "Moved ./Assets/Samples to ./Packages/com.nickmaltbie.screenmanager/Samples~"

# Reset all other changes
git rm -rf .
git checkout HEAD -- ./Packages/com.nickmaltbie.screenmanager

git commit -m "Filtered for only package files"

# Move files from _keep to root folder
git mv ./Packages/com.nickmaltbie.screenmanager/* .

git commit -m "Setup files for release"
