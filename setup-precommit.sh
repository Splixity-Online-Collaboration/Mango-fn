dotnet tool install fantomas
touch ./.git/hooks/pre-commit
echo "#!/bin/sh" > ./.git/hooks/pre-commit
echo "git diff --cached --name-only --diff-filter=ACM -z | xargs -0 dotnet fantomas" >> ./.git/hooks/pre-commit
echo "git diff --cached --name-only --diff-filter=ACM -z | xargs -0 git add" >> ./.git/hooks/pre-commit