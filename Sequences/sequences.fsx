open System.Collections.Generic
(*
  Define a function returning a sequence
  
  sequenceUsing : int -> (int->int) -> int -> seq<int>
   
  that generates a sequence of integers such that:
  * the length of the sequence is defined by the first argument;
  * the sequence is computed based on the function give as the second argument, and
  * the first element is computed by applying the function given in the second argument to the initial value given as the third argument.
*)

let sequenceUsing (len:int) (foo:int->int) (first:int) : seq<int> =
    let rec loop (foo:int->int) (res:int) : seq<int> = 
        seq { yield foo res; yield! loop foo (foo res) }
    first |> loop foo |> Seq.take len

(*
  Define a function returning a sequence
  
  pseudoRandom : int -> seq<int> -> seq<int>
  
  that generates pseudo random numbers based on the first argument as seed and 
  second argument as an infinite sequence of values of int type.

  Use the built in hash function to combine the seed and input integers.
  Experiment with ways how to combine the seed and input value in such a way 
  that the order of the values in the output sequence differs from that of the input
  sequence.
*)

let pseudoRandom (seed:int) (s:seq<int>) : seq<int> =
    let rand = System.Random()
    seq {for i in s -> rand.Next(hash(seed, i))}

(*
  Define a function
  
  cacheObserver : seq<'a> -> seq<'a>
  
  that will cache the values of a sequence and print "Cached"
  to standard output every time the value requested from the sequence is actually cached.
*)

let cacheObserver (s:seq<'a>) : seq<'a> = 
    let cache = Dictionary<_, _>()
    seq {for i in s do
            match cache.TryGetValue i with
            | true, v -> printfn v
            | _ -> cache.Add(i ,"Cached")
            yield i
      }

(*
  A function from a type 'env to a type 'a can be seen as a computation that
  computes a value of type 'a based on an environment of type 'env. We call such
  a computation a reader computation, since compared to ordinary computations,
  it can read the given environment. Below you find the following:

    • the definition of a builder that lets you express reader computations
      using computation expressions

    • the definition of a reader computation ask : 'env -> 'env that returns the
      environment

    • the definition of a function runReader : ('env -> 'a) -> 'env -> 'a that
      runs a reader computation on a given environment

    • the definition of a type Expr of arithmetic expressions

  Implement a function eval : Expr -> Map<string, int> -> int that evaluates
  an expression using an environment which maps identifiers to values.
  
  NB! Use computation expressions for reader computations in your implementation.
  
  Note that partially applying eval to just an expression will yield a function of
  type map <string, int> -> int, which can be considered a reader computation.
  This observation is the key to using computation expressions.

  The expressions are a simplified subset based on
  Section 18.2.1 of the F# 4.1 specification:
  https://fsharp.org/specs/language-spec/4.1/FSharpSpec-4.1-latest.pdf

*)

type ReaderBuilder () =
    member this.Bind   (reader, f) = fun env -> f (reader env) env
    member this.Return x           = fun _   -> x

let reader = new ReaderBuilder ()

let ask = id

let runReader = (<|)

type Expr =
  | Const  of int          // constant
  | Ident  of string       // identifier
  | Neg    of Expr         // unary negation, e.g. -1
  | Sum    of Expr * Expr  // sum 
  | Diff   of Expr * Expr  // difference
  | Prod   of Expr * Expr  // product
  | Div    of Expr * Expr  // division
  | DivRem of Expr * Expr  // division remainder as in 1 % 2 = 1
  | Let    of string * Expr * Expr // let expression, the string is the identifier.

let d m k v =
  m |> Map.add(k,v)
