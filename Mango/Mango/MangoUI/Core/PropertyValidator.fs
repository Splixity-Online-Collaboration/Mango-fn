module MangoUI.Core.PropertyValidator

open MangoUI.Core

/// <summary>
/// Represents the kinds of UI elements supported by Mango UI.
/// Used during semantic validation to determine which properties
/// are allowed for a given element.
/// </summary>
type UIElementKind =
    | Button | TextBlock | TextBox | CheckBox
    | RadioButton | ToggleSwitch | Calendar
    | ToggleButton | Row | Column | Border
    | Identifier

/// <summary>
/// Represents the kinds of properties that can be applied to UI elements.
/// Each case corresponds to a declarative attribute supported by Mango UI.
/// </summary>
type PropertyKind = 
    | Hidden | Margin | Width | Height | Id
    | Color | BackgroundColor | FontFamily
    | FontSize | FontWeight | FontStyle
    | LineHeight | TextAlign | TextTrim 
    | TextWrap | Corner | Density | Wrap
    | Label | OnClick

/// <summary>
/// A set of properties common to most UI elements,
/// such as layout and visibility controls.
/// </summary>
let commonProps = set [Hidden; Margin; Width; Height; Id; OnClick]

/// <summary>
/// Converts an abstract property (from the Mango AST) 
/// into its corresponding <see cref="PropertyKind"/>.
/// </summary>
/// <param name="property">The abstract syntax property to classify.</param>
/// <returns>The <see cref="PropertyKind"/> corresponding to the property.</returns>
/// <exception cref="System.Exception">
/// Thrown if the property cannot be mapped (should not occur in valid ASTs).
/// </exception>
let propertyKind property =
    match property with
    | AbSyn.Hidden          _ -> Hidden
    | AbSyn.Margin          _ -> Margin
    | AbSyn.Width           _ -> Width
    | AbSyn.Height          _ -> Height
    | AbSyn.Color           _ -> Color
    | AbSyn.BackgroundColor _ -> BackgroundColor
    | AbSyn.FontFamily      _ -> FontFamily
    | AbSyn.FontSize        _ -> FontSize
    | AbSyn.FontWeight      _ -> FontWeight
    | AbSyn.FontStyle       _ -> FontStyle
    | AbSyn.LineHeight      _ -> LineHeight
    | AbSyn.TextAlign       _ -> TextAlign
    | AbSyn.TextTrim        _ -> TextTrim
    | AbSyn.TextWrap        _ -> TextWrap
    | AbSyn.Corner          _ -> Corner
    | AbSyn.Density         _ -> Density
    | AbSyn.Property.Wrap   _ -> Wrap
    | AbSyn.Id              _ -> Id
    | AbSyn.Label           _ -> Label
    | AbSyn.Onclick         _ -> OnClick

/// <summary>
/// Maps each <see cref="UIElementKind"/> to the set of 
/// <see cref="PropertyKind"/> values allowed for that element.
/// Used by the validator to check semantic correctness.
/// </summary>
let elementPropertyMap : Map<UIElementKind, Set<PropertyKind>> =
    Map.ofList [
        Button, Set.union commonProps (set [Label])
        TextBlock, Set.union commonProps (set [Color; BackgroundColor; FontFamily; FontSize; FontWeight; FontStyle; LineHeight; TextAlign; TextTrim; TextWrap; Label])
        TextBox, set []
        CheckBox, set []
        RadioButton, set []
        ToggleSwitch, set []
        Calendar, set []
        ToggleButton, set []
        Border, Set.union commonProps (set [Color; Corner; Density])
        Row, Set.union commonProps (set [Wrap; BackgroundColor])
        Column, Set.union commonProps (set [Wrap; BackgroundColor])
    ]

/// <summary>
/// Validates that a UI element only contains properties 
/// permitted for its type. This forms part of Mango UI's 
/// semantic analysis phase, ensuring correctness beyond syntax.
/// </summary>
/// <param name="element">The UI element to validate, as parsed from the Mango AST.</param>
/// <returns>
/// <c>true</c> if all properties are valid for the element; 
/// <c>false</c> otherwise. Errors are reported to the console.
/// </returns>
/// <remarks>
/// This function is typically run after parsing and before interpretation.
/// It enforces Mango UI's declarative rules to prevent 
/// applying unsupported properties to elements.
/// </remarks>
/// <example>
/// <code>
/// let btn = AbSyn.Button("submit", Some [AbSyn.Width 200; AbSyn.Height 50], (0, 0))
/// if validateProperties btn then
///     printfn "Button is semantically valid."
/// else
///     printfn "Invalid button properties."
/// </code>
/// </example>
let validateProperties (element : AbSyn.UIElement) =
    let kind, props =
        match element with
        | AbSyn.Button (ps, _) -> Button, ps
        | AbSyn.TextBlock (ps, _) -> TextBlock, ps
        | AbSyn.TextBox (name, _) -> TextBox, Some []
        | AbSyn.CheckBox (name, _) -> CheckBox, Some []
        | AbSyn.RadioButton (name, _) -> RadioButton, Some []
        | AbSyn.ToggleSwitch (name, _) -> ToggleSwitch, Some []
        | AbSyn.Calendar _ -> Calendar, Some []
        | AbSyn.ToggleButton _ -> ToggleButton, Some []
        | AbSyn.Row (ps, _, _) -> Row, ps
        | AbSyn.Column (ps, _, _) -> Column, ps
        | AbSyn.Border (ps, _, _) -> Border, ps
        | AbSyn.Identifier _ -> failwith "This should never happen..."

    let allowed = elementPropertyMap.[kind]

    match props with
    | Some ps -> 
        ps
        |> List.exists (fun p ->
            let pk = propertyKind p
            if Set.contains pk allowed then false
            else 
                do printf $"Property {pk} not allowed on "
                true)
        |> not
    | None -> true

/// Replace or append a property in the list
let upsertProperty (newProp: AbSyn.Property) (props: AbSyn.Property list) =
    let kindToReplace = propertyKind newProp

    // Split into matching and non-matching
    let withoutMatch, matchFound =
        props 
        |> List.fold (fun (acc, found) p ->
            if propertyKind p = kindToReplace then (acc, true)
            else (p :: acc, found)
        ) ([], false)

    if matchFound then
        // If found, just replace by adding the newProp back
        newProp :: List.rev withoutMatch
    else
        // If not found, append newProp at the end
        props @ [newProp]

let createProp propertyKind exp =
    match propertyKind, exp with
    | Hidden, AbSyn.Constant (AbSyn.Bool b, _) -> AbSyn.Hidden (Some (b, (0,0)))
    | Label, AbSyn.Constant (AbSyn.String s, _) -> AbSyn.Label (Some (s, (0,0)))
    | _ -> failwith "Invalid property kind or expression or not implemented yet"