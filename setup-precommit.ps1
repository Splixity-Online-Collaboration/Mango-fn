New-Item .\.git\hooks\pre-commit.bat -type file
echo "@echo off" > .\.git\hooks\pre-commit.bat
echo "git diff --cached --name-only --diff-filter=ACM -z | xargs -0 dotnet fantomas" >> .\.git\hooks\pre-commit.bat
echo "git diff --cached --name-only --diff-filter=ACM -z | xargs -0 git add" >> .\.git\hooks\pre-commit.bat