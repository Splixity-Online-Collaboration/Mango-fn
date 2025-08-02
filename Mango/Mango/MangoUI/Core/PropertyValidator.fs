module PropertyValidator

open MangoUI.Core

type UIElementKind =
    | Button | TextBlock | TextBox | CheckBox
    | RadioButton | ToggleSwitch | Calendar
    | ToggleButton | Row | Column | Border
    | Identifier

type PropertyKind = 
    | Hidden | Margin | Width | Height | Id
    | Color | BackgroundColor | FontFamily
    | FontSize | FontWeight | FontStyle
    | LineHeight | TextAlign | TextTrim 
    | TextWrap | Corner | Density | Wrap

let commonProps = set [Hidden; Margin; Width; Height; Id]

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
    |                       _ -> failwith "This should never happen..."

let elementPropertyMap : Map<UIElementKind, Set<PropertyKind>> =
    Map.ofList [
        Button, commonProps
        TextBlock, Set.union commonProps (set [Color; BackgroundColor; FontFamily; FontSize; FontWeight; FontStyle; LineHeight; TextAlign; TextTrim; TextWrap])
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

let validateProperties (element : AbSyn.UIElement) =
    let kind, props, name =
        match element with
        | AbSyn.Button (name, ps, _) -> Button, ps, name
        | AbSyn.TextBlock (name, ps, _) -> TextBlock, ps, name
        | AbSyn.TextBox (name, _) -> TextBox, Some [], name
        | AbSyn.CheckBox (name, _) -> CheckBox, Some [], name
        | AbSyn.RadioButton (name, _) -> RadioButton, Some [], name
        | AbSyn.ToggleSwitch (name, _) -> ToggleSwitch, Some [], name
        | AbSyn.Calendar _ -> Calendar, Some [], ""
        | AbSyn.ToggleButton _ -> ToggleButton, Some [], ""
        | AbSyn.Row (ps, _, _) -> Row, ps, ""
        | AbSyn.Column (ps, _, _) -> Column, ps, ""
        | AbSyn.Border (ps, _, _) -> Border, ps, ""
        | AbSyn.Identifier _ -> failwith "This should never happen..."

    let allowed = elementPropertyMap.[kind]

    match props with
    | Some ps -> 
        ps
        |> List.exists (fun p ->
            let pk = propertyKind p
            if Set.contains pk allowed then false
            else 
                do printf $"Property {pk} not allowed on {name}"
                true)
        |> not
    | None -> true