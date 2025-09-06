module MangoUI.Frontend.ParserWrapper

open System.Text
open FSharp.Text.Lexing
open FSharp.Text.Parsing
open MangoUI.Core.Types

let keywords =
    [ "window"
      "column"
      "row"
      "button"
      "text"
      "textbox"
      "checkbox"
      "radiobutton"
      "toggleswitch"
      "calendar"
      "togglebutton"
      "border"
      "true"
      "false"
      "hidden"
      "wrap"
      "fontfamily"
      "fontsize"
      "fontweight"
      "fontstyle"
      "lineheight"
      "textalign"
      "textwrap"
      "texttrim"
      "color"
      "bgcolor"
      "margin"
      "width"
      "height"
      "id"
      "label"
      "onclick"
      "corner"
      "density"
      "fill"
      "hug"
      "italic"
      "underline"
      "strikethrough"
      "center"
      "left"
      "right"
      "word"
      "character"
      "notrim"
      "red"
      "blue"
      "green"
      "yellow"
      "pink"
      "black"
      "white"
      "let"
      "function"
      "update"
      "set" ]

// See: https://en.wikipedia.org/wiki/Levenshtein_distance
let rec levenshtein_distance (a: string) (b: string) =
    if b.Length = 0 then
        a.Length
    else if a.Length = 0 then
        b.Length
    else if a[0] = b[0] then
        levenshtein_distance a[1..] b[1..]
    else
        1
        + min (min (levenshtein_distance a[1..] b) (levenshtein_distance a b[1..])) (levenshtein_distance a[1..] b[1..])

// Naive DSA solution for finding the most likely word, however this method is very naive and does not
// really approximate how close words are to each other.
let rec get_most_likely_keyword (identifier: string) (keywords: string list) (searchStr: string) : string =
    if keywords.Length = 1 then
        keywords[0]
    else
        let searchStr' = searchStr + identifier[searchStr.Length].ToString()
        let new_keywords = List.filter (fun (s: string) -> s.StartsWith searchStr') keywords
        get_most_likely_keyword identifier new_keywords searchStr'

// Use the levenshtein distance to calculate the most probable word
// and use pure fsharp piping operators to be really cool! 😎
let get_most_likely_keyword_V2 (identifier: string) (keywords: string list) : string =
    keywords
    |> List.map (fun s -> (levenshtein_distance s identifier, s))
    |> List.sortBy fst
    |> List.map snd
    |> List.head

let parseString (source: string) =
    try
        let lexbuf = LexBuffer<_>.FromBytes(Encoding.UTF8.GetBytes source)
        Ok(Parser.Prog Lexer.Token lexbuf)
    with
    | Lexer.LexicalError(msg, (line, col)) -> Error $"Lexical error: {msg} at line {line}, column {col}"
    | SyntaxError boxed ->
        let ctx = unbox<ParseErrorContext<Parser.token>> boxed

        match ctx.CurrentToken with
        | Some(Parser.ID(name, (line, col))) ->
            let most_likely_keyword = get_most_likely_keyword_V2 name keywords

            Error
                $"Unknown identifier '{name}' at line {line}, col {col}, maybe you meant to write {most_likely_keyword}"
        | Some tok -> Error $"Unexpected token {tok}"
        | None -> Error "Syntax error at beginning of file"
