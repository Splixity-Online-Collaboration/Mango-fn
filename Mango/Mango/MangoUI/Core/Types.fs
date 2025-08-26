module MangoUI.Core.Types

open MangoUI.SymTab
open AbSyn

type TreeEnv = SymTab<UIElement>

type FuncEnv = SymTab<Stmt list>