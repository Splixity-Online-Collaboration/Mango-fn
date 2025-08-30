module MangoUI.Core.Types

open MangoUI.SymTab
open AbSyn

type TreeEnv = SymTab<UIElement>

type FuncEnv = SymTab<Stmt list>

type Msg =
    | UpdateFuncEnv of FuncEnv * string * Stmt list
    | UpdateTreeEnv of TreeEnv * string * UIElement
    | UpdateUIElements of UIElement list
    | EvalFunc of string
    | EvalLambda of Stmt list

type AppState = { treeEnv: TreeEnv; funcEnv: FuncEnv; uiElements: UIElement list }