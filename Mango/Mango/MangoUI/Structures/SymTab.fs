(* A polymorphic symbol table. *)

module MangoUI.SymTab

type SymTab<'a> = SymTab of (string * 'a) list

(*
A symbol table is just a list of tuples identifiers and values. This allows for
easy shadowing, as a shadowing binding can be quickly placed at the head of the
list.
*)

let empty () = SymTab []

let rec lookup n tab =
  match tab with
    | SymTab [] -> None
    | SymTab ((n1,i1)::remtab) ->
        if n = n1
        then Some i1
        else lookup n (SymTab remtab)

let bind n i (SymTab stab) = SymTab ((n,i)::stab)

let remove n (SymTab stab) =
    SymTab (List.filter (fun (x, _) -> x <> n) stab)

let removeMany ns (SymTab stab) =
  SymTab (List.filter (fun (x, _) ->
            not (List.exists (fun y -> y = x) ns)) stab)

let combine (SymTab t1) (SymTab t2) = SymTab (t1 @ t2)

let fromList l = SymTab l

let toList (SymTab lst) = lst
