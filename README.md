# Mango-fn

# Passing file-arguments using `dotnet run` command
To pass an argument for which file to use consider the following example where we want to interpret
the window.mango file placed in the examples folder:
```
dotnet run -- examples/window.mango
```
if no argument is provided it automatically interprets the examples/window.mango file.
# Code Structure
The code is mostly build using module-based code structuring. The project is build as an fsharp project with the source code relying inside the innermost "Mango" folder.

## Namespaces
- AvaloniaHelpers - Helper functions for interpreting and constructing UI using Avalonia UI.

# Design Principles
## 1. Joy of Creation First
"It should feel fun, not technical."

- Mango is about **playful creativity**.
- The moment you type `window "Hello"` and it opens — you should **smile**.
- Prioritize **fast results** and **visual feedback** over technical perfection.

🛠️ Example: Always favor 1-line simple window creation even if it means hiding some complexity inside.

## 2. Minimalism Wins
"If you can leave it out, leave it out."

- Mango scripts should feel **short, readable, and almost poetic**.
- Don't overload Mango with types, modifiers, complex inheritance, etc.
- **Less is more**.

🛠️ Example: Instead of `Button { text = "OK"; size = (100,30); }`, just write:

```mango
button "OK" at (10,10)
```
## 3. Predictable Behavior
"No magic. What you see is what happens."

- Every line of Mango should behave in a **straightforward** way.

- No hidden lifecycle hooks, weird metaprogramming, implicit updates.

- **Simplicity over cleverness**.

🛠️ Example: If you write `on click -> show_message("hi")`, you know it will exactly bind to a click — no surprises.

## 4. Fast Edit-Run Cycle
"Edit a file, save it, see the change immediately."

- Mango apps should reload quickly when code changes.

- Maybe support **hot reload** or **instant restart** mode if you can (later).

- Encourage experimentation!

🛠️ Example: Mango editor or CLI could have a `mango run myapp.mango` and instantly relaunch window.

## 5. Visual Hierarchy Maps to Code Hierarchy
"Nested things in code look like nested things on screen."

- Mango syntax should *mirror the GUI tree*.

- If a `button` is inside a `window` block, it appears inside the window — simple.

🛠️ Example:
```mango
window "Main" {
  button "OK"
  button "Cancel"
}
```
is two buttons inside the window, no weird mappings.

## 6. Safe Defaults, Easy Overrides
"Smart guesses, but let me change it easily."

- Default positions, sizes, behaviors should *just work*.

- But if users want control, **it's easy** to override.

🛠️ Example:

- If no position is specified, Mango auto-stacks buttons vertically.

- But writing `at (100, 200)` manually places it.

## 7. Built-in Kindness
"Mango programs should never crash ugly."

- Always have graceful error messages.

- Show friendly hints when users mess up syntax.

🛠️ Example: If the user forgets a `{ }`, show:\
`Syntax Error: Missing { after window definition.`

## 8. Designed for Extensibility
"Simple now, powerful later."

- Mango should start small, but you should **leave room** for:

    - Adding simple **state** (e.g., counter = 0).

    - Binding **functions** to events (e.g., custom behavior).

    - Defining **layouts** (rows, columns, grids).

    - Maybe even **theming** later (colors, dark mode).

🛠️ Example:
Later, you might allow:

```mango
theme dark
window "Main" {
  label "Welcome!"
}
```
## **Poetic Syntax:** Allow slightly "natural" syntax like `when clicked ->` instead of `on click ->`.

## **Animation Friendly:** Support small animations without crazy setup.

## **Exportable:** Maybe allow Mango apps to export to standalone `.exe` or `.app` later (compile target?).