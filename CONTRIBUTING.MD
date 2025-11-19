# Contributing to Mango

🏗️ **Note:** we currently **do not accept any open contributions!**

Here are the [Design Principles](DESIGN.md)

## Unified Code Formatter

To keep the codebase consistent, this repo includes a formatter that runs automatically before every commit.

**Setup:**

* **Windows:** run `.\setup-precommit.ps1` in PowerShell
* **Linux/macOS:** run `./setup-precommit.sh`

That's it! Next time you commit, all changed files will be formatted automatically according to our config.

## Running with a File Argument

You can run the interpreter and specify a file like this:

```bash
dotnet run -- examples/window.mg
```

If you don’t pass any arguments, it defaults to `examples/window.mg`.

## Project Structure

The project is organized into modules inside the main `Mango/Mango` folder. It’s built as a standard F# project.

```bash
cd Mango/Mango
```

### Namespaces

* **AvaloniaHelpers** - functions that help interpret and build UIs using Avalonia.

## Creating a Release

🏗️ **Note:** Mango is not yet stable (early alpha) - the release format is quite primitive!

```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```