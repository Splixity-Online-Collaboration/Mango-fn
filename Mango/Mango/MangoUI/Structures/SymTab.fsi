(*
A polymorphic symbol table. 
A symbol table is a data structure associating names (strings) with values. It
is useful for keeping track of binidngs. Bindings can be shadowed --- the
active binding is the one made most recently.
*)

module MangoUI.SymTab
  (* A symbol table with values of type 'a. *)
  type SymTab<'a> = SymTab of (string * 'a) list 
  // when 'a : equality

  (* Create an empty symbol table. *)
  val empty : unit -> SymTab<'a> when 'a : equality 

  (* Look up the active binding for the name. *)
  val lookup : string -> SymTab<'a> -> Option<'a>

  (* Bind the name to a value, shadowing any existing
     binidngs with the same name. *)
  val bind : string -> 'a -> SymTab<'a> -> SymTab<'a>

  (* Remove all existing bindings of the given name. *)
  val remove : string -> SymTab<'a > -> SymTab<'a>

  (* Remove all existing bindings of all the given names. *)
  val removeMany : string list -> SymTab<'a > -> SymTab<'a >

  (* Combine two symbol tables. The first table shadows the second. *)
  val combine : SymTab<'a > -> SymTab<'a > -> SymTab<'a >

  (* Create a symbol table from a list of name-value pairs.       
       In case of duplicates, the bindings are shadowed in reverse order from
       the head of the list. That is, the active binding will ne the one
       closest to the head of the list. *)
  val fromList : (string * 'a) list -> SymTab<'a >
  val toList   : SymTab<'a > -> (string * 'a) list

