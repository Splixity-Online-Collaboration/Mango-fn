# Design Principles

## 1. Joy of Creation First
"It should feel fun, not technical."

- Mango is about **playful creativity**.
- The moment you type `window "Hello"` and it opens - you should **smile**.
- Prioritize **fast results** and **visual feedback** over technical perfection.

Example: Always favor 1-line simple window creation even if it means hiding some complexity inside.

## 2. Minimalism Wins
"If you can leave it out, leave it out."

- Mango scripts should feel **short, readable, and almost poetic**.
- Don't overload Mango with types, modifiers, complex inheritance, etc.
- **Less is more**.

Example: Instead of `Button { text = "OK"; size = (100,30); }`, just write:

```mg
button "OK" at (10,10)
```
## 3. Predictable Behavior
"No magic. What you see is what happens."

- Every line of Mango should behave in a **straightforward** way.

- No hidden lifecycle hooks, weird metaprogramming, implicit updates.

- **Simplicity over cleverness**.

Example: If you write `on click -> show_message("hi")`, you know it will exactly bind to a click — no surprises.

## 4. Fast Edit-Run Cycle
"Edit a file, save it, see the change immediately."

- Mango apps should reload quickly when code changes.

- Support **hot reload** or **instant restart** mode.

- Encourage experimentation!

Example: Mango editor or CLI could have a `mango myapp.mg` and instantly relaunch window.

## 5. Visual Hierarchy Maps to Code Hierarchy
"Nested things in code look like nested things on screen."

- Mango syntax should *mirror the GUI tree*.

- If a `button` is inside a `window` block, it appears inside the window — simple.

Example:
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

Example:

- If no position is specified, Mango auto-stacks buttons vertically.

- But writing `at (100, 200)` manually places it.

## 7. Built-in Kindness
"Mango programs should never crash ugly."

- Always have graceful error messages.

- Show friendly hints when users mess up syntax.

Example: If the user forgets a `{ }`, show:\
`Syntax Error: Missing { after window definition.`

## 8. Designed for Extensibility
"Simple now, powerful later."

- Mango should start small, but there should be room for:

    - Adding simple **state** (e.g., counter = 0).

    - Binding **functions** to events (e.g., custom behavior).

    - Defining **layouts** (rows, columns, grids).

    - **Theming** (colors, dark mode).

Example:

```mango
theme dark
window "Main" {
  label "Welcome!"
}
```