/// <summary>
/// Provides lightweight logging utilities for MangoUI, with support for
/// informational, warning, and error messages.
///
/// The logger is intentionally minimal and console-based, designed primarily
/// for development and debugging purposes rather than production-grade logging.
/// </summary>
module MangoUI.Util.Logger

/// <summary>
/// Controls whether informational log messages are printed.
/// When set to <c>true</c>, <see cref="info" /> messages will be displayed.
/// </summary>
///
/// <remarks>
/// The <c>verbose</c> flag can be toggled at runtime:
/// <code>
/// MangoUI.Util.Logger.verbose <- true
/// info "Debug mode enabled"
/// </code>
/// </remarks>
let mutable verbose = false

/// <summary>
/// Logs an informational message to the console, if verbose mode is enabled.
/// </summary>
/// <param name="o">The object or message to log. Printed using <c>%A</c> formatting.</param>
/// <remarks>
/// This is typically used for non-critical runtime diagnostics that can be toggled
/// on or off during development.
/// </remarks>
/// <example>
/// <code>
/// verbose <- true
/// info "UI tree successfully rebuilt"
/// </code>
/// </example>
let info o =
    if verbose then
        do printfn "[INFO] %A" o

/// <summary>
/// Logs a warning message to the console.
/// </summary>
/// <param name="o">The object or message to log. Printed using <c>%A</c> formatting.</param>
/// <remarks>
/// Use this for potentially problematic conditions that are not fatal,
/// such as deprecated usage or missing optional data.
/// </remarks>
/// <example>
/// <code>
/// warn "Fallback layout applied due to missing dimensions"
/// </code>
/// </example>
let warn o = do printfn "[WARN] %A" o

/// <summary>
/// Logs an error message to the console.
/// </summary>
/// <param name="o">The object or message to log. Printed using <c>%A</c> formatting.</param>
/// <remarks>
/// This is intended for critical or unexpected runtime issues that
/// may affect program correctness or stability.
/// </remarks>
/// <example>
/// <code>
/// error "Failed to parse Mango-fn expression: unexpected token"
/// </code>
/// </example>
let error o = do printfn "[ERROR] %A" o
