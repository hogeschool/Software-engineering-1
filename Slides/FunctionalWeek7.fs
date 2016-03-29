module FunctionalWeek7

open CommonLatex
open LatexDefinition
open CodeDefinitionLambda
open Interpreter

let LambdaCodeBlock(x,y) = LambdaCodeBlock(x,y,false)
let HaskellCodeBlock(x,y) = HaskellCodeBlock(x,y,false)

let fact =
  -"n" ==> ((If >> (IsZero >> !!"n") >> !!"1") >> ((Mult >> (!!"fact" >> ((Minus >> !!"n") >> !!"1"))) >> !!"n"))


let slides =
  [
    Section( @"Translating Lambda calculus to Haskell")
    SubSection("Overview")
    ItemsBlock
      [
        ! @"Haskell can be mapped to the lambda calculus as we did for F\#"
        ! @"It is slightly different than F\# with respect to let-bindings"
        ! @"It uses a different evaluation strategy than the straightforward beta reduction seen so far"
      ]

    SubSection("Basic expressions")
    VerticalStack
      [
        TextBlock @"Integers, booleans, floats, strings have the usual meaning, both in the lambda calculus, Haskell, and the languages you are used to:"

        HaskellCodeBlock(TextSize.Small, (Minus >> ((Plus >> !!"2") >> !!"3")) >> !!"4")
        HaskellCodeBlock(TextSize.Small, (And >> True) >> False)
      ]

    SubSection("\texttt{if-then-else}")
    VerticalStack
      [
        ItemsBlock 
          [
            ! @"Conditionals behave just like in the lambda calculus"
            ! @"This means that they return the evaluation of either of the two branches"
            ! @"This differs from imperative languages, where we just jump into either of the two branches"
          ]

        HaskellCodeBlock(TextSize.Small, ((If >> ((And >> True) >> False)) >> !!"0") >> !!"1")
      ]

    VerticalStack
      [
        TextBlock @"\textbf{Haskell}"

        HaskellCodeBlock(TextSize.Small, ((If >> ((And >> True) >> False)) >> !!"0") >> !!"1")

        TextBlock @"\textbf{Lambda calculus}"
        LambdaCodeBlock(TextSize.Small, ((If >> ((And >> True) >> False)) >> !!"0") >> !!"1")
        LambdaCodeBlock(TextSize.Small, (deltaRules If).Value >> ((And >> True) >> False) >> !!"0" >> !!"1")
      ]

    SubSection("Anonymous functions")
    VerticalStack
      [
        TextBlock @"Functions look very similar, with \texttt{\textbackslash} instead of $\lambda$"
        HaskellCodeBlock(TextSize.Small, -"x" ==> (-"f" ==> (!!"f" >> !!"x")))

        Pause

        TextBlock @"Just like function application"
        HaskellCodeBlock(TextSize.Small, ((-"x" ==> (-"f" ==> (!!"f" >> !!"x"))) >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x")))
      ]

    VerticalStack
      [
        TextBlock @"\textbf{Haskell}"
        HaskellCodeBlock(TextSize.Small, ((-"x" ==> (-"f" ==> (!!"f" >> !!"x"))) >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x")))

        TextBlock @"\textbf{Lambda calculus}"
        LambdaCodeBlock(TextSize.Small, ((-"x" ==> (-"f" ==> (!!"f" >> !!"x"))) >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x")))
        
      ]

    SubSection("Named functions")
    VerticalStack
      [
        TextBlock @"We can give names to functions, and code becomes much prettier as a result"

        HaskellCodeBlock(TextSize.Small, Let(-"apply", -"x" ==> (-"f" ==> (!!"f" >> !!"x")), (!!"apply" >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x"))))

        //LambdaCodeBlock(TextSize.Small, Let(-"apply", -"x" ==> (-"f" ==> (!!"f" >> !!"x")), (!!"apply" >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x"))))
      ]

    VerticalStack
      [
        TextBlock @"\textbf{Haskell}"
        HaskellCodeBlock(TextSize.Small, Let(-"apply", -"x" ==> (-"f" ==> (!!"f" >> !!"x")), (!!"apply" >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x"))))

        TextBlock @"\textbf{Lambda calculus}"
        LambdaCodeBlock(TextSize.Small, Let(-"apply", -"x" ==> (-"f" ==> (!!"f" >> !!"x")), (!!"apply" >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x"))))
        LambdaCodeBlock(TextSize.Small, (deltaRules (Let(-"apply",-"x" ==> (-"f" ==> (!!"f" >> !!"x")), (!!"apply" >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x"))))).Value)

      ]


    SubSection("Recursive functions")
    VerticalStack
      [
        ItemsBlock 
            [
              ! @"In Haskell, unlike F\#, there is no distinction between \texttt{let} and \texttt{let rec}. Everything is \texttt{let rec}."
              ! @"\textt{let-in} is used to define bindings locally into a function body"
              ! @"Global let bindings are simply defined by defining the function name and can be used recursively"
            ]

        HaskellCodeBlock(TextSize.Small, 
          Let(-"fact", 
              (Fix >> (-"f" ==> (-"n" ==> ((If >> (IsZero >> !!"n") >> !!"1") >> ((Mult >> (!!"f" >> ((Minus >> !!"n") >> !!"1"))) >> !!"n"))))), 
              !!"fact" >> !!"2"))
      ]

    VerticalStack
      [
        TextBlock @"\textbf{Haskell}"
        HaskellCodeBlock(TextSize.Small, 
          Let(-"fact", 
              (Fix >> (-"f" ==> (-"n" ==> ((If >> (IsZero >> !!"n") >> !!"1") >> ((Mult >> (!!"f" >> ((Minus >> !!"n") >> !!"1"))) >> !!"n"))))), 
              !!"fact" >> !!"2"))

        TextBlock @"\textbf{Lambda calculus}"
        LambdaCodeBlock(TextSize.Small,
          (Let(-"fact", 
                        (Fix >> (-"f" ==> (-"n" ==> ((If >> (IsZero >> !!"n") >> !!"1") >> ((Mult >> (!!"f" >> ((Minus >> !!"n") >> !!"1"))) >> !!"n"))))), 
                        !!"fact" >> !!"2")))
      ]

    SubSection("Pairs")
    VerticalStack
      [
        TextBlock @"We can define tuples by just putting a comma between the values, with or without nesting for more than two values is done for us. Unlike F\#, brackets are mandatory when defining tuples in Haskell"

        HaskellCodeBlock(TextSize.Small, (MakePair >> !!"1") >> True)
      ]

    VerticalStack
      [
        TextBlock @"\textbf{Haskell}"
        HaskellCodeBlock(TextSize.Small, (MakePair >> !!"1") >> True)

        TextBlock @"\textbf{Lambda calculus}"
        LambdaCodeBlock(TextSize.Small, (MakePair >> !!"1") >> True)
        LambdaCodeBlock(TextSize.Small, ((deltaRules MakePair).Value >> !!"1") >> True)
      ]

    VerticalStack
      [
        ItemsBlock
          [
            ! @"Functions such as $\pi_1$ and $\pi_2$, which both extract one item of a pair, also exist in Haskell"
            ! @"They are called, respectively, \texttt{fst} and \texttt{snd}"
          ]

        HaskellCodeBlock(TextSize.Small, First >> ((MakePair >> !!"1") >> True))
        HaskellCodeBlock(TextSize.Small, Second >> ((MakePair >> !!"1") >> True))
      ]

    VerticalStack
      [
        TextBlock @"\textbf{Haskell}"
        HaskellCodeBlock(TextSize.Small, First >> ((MakePair >> !!"1") >> True))
        HaskellCodeBlock(TextSize.Small, Second >> ((MakePair >> !!"1") >> True))

        TextBlock @"\textbf{Lambda calculus}"
        LambdaCodeBlock(TextSize.Small, First >> ((MakePair >> !!"1") >> True))
        LambdaCodeBlock(TextSize.Small, (deltaRules First).Value >> ((MakePair >> !!"1") >> True))
        LambdaCodeBlock(TextSize.Small, Second >> ((MakePair >> !!"1") >> True))
        LambdaCodeBlock(TextSize.Small, (deltaRules Second).Value >> ((MakePair >> !!"1") >> True))
      ]

    SubSection("Discriminated unions")
    VerticalStack
      [
        ItemsBlock
          [
            ! @"Haskell also offers built-in discriminated unions"
            ! @"Functions such as \texttt{inl} and \texttt{inr}, which both embed one item into the union, also exist in Haskell"
            ! @"They are called, respectively, \texttt{Left} and \texttt{Right}"
          ]

        HaskellCodeBlock(TextSize.Small, Inl >> !!"1")
        HaskellCodeBlock(TextSize.Small, Inr >> True)
      ]

    VerticalStack
      [
        TextBlock @"\textbf{Haskell}"
        HaskellCodeBlock(TextSize.Small, Inl >> !!"1")
        HaskellCodeBlock(TextSize.Small, Inr >> True)

        TextBlock @"\textbf{Lambda calculus}"
        LambdaCodeBlock(TextSize.Small, Inl >> !!"1")
        LambdaCodeBlock(TextSize.Small, (deltaRules Inl).Value >> !!"1")
        LambdaCodeBlock(TextSize.Small, Inr >> True)
        LambdaCodeBlock(TextSize.Small, (deltaRules Inr).Value >> True)
      ]

    VerticalStack
      [
        TextBlock @"We can, of course, perform matches on discriminated unions"

        HaskellCodeBlock(TextSize.Small, (Match >> (Inl >> !!"1") >> (-"x" ==> (Inl >> !!"x"))) >> (-"x" ==> (Inr >> !!"x")))
        
        Pause

        HaskellCodeBlock(TextSize.Small, Let(-"i", ((Match >> (Inl >> !!"1") >> (-"x" ==> !!"x")) >> (-"x" ==> !!"0")), (Mult >> !!"i") >> !!"2"))
      ]

    VerticalStack
      [
        TextBlock @"Discriminated unions, and their corresponding matches, can be nested as deep as we need"

        HaskellCodeBlock(
          TextSize.Small, 
          (Match >> (Inl >> (Inr >> True)) >> 
            (-"x" ==> (((Match >> !!"x") >> (-"x" ==> (Inl >> !!"x"))) >> (-"x" ==> (Inr >> !!"x"))))) >> 
            (-"y" ==> (Inr >> !!"y")))
      ]

    SubSection("Type annotations")
    VerticalStack
      [
        ItemsBlock
          [
            ! @"Type annotations in haskell are quite different from F\#"
            ! @"The declaration of the function types is separated from the body"
          ]
        HaskellCodeBlock(TextSize.Small,
                          HaskellFuncDef("fact",Some [!!"Integral";!!"Integral"],
                                         [fact]))

      ]

    VerticalStack
      [
        ItemsBlock
          [
            ! @"It is possible to use pattern matching to define functions instead of using a \texttt{match} or an \texttt{if-then-else}"
            ! @"It is done just repeating the function definition with the specific arguments"
          ]
        HaskellCodeBlock(TextSize.Small,
                          HaskellFuncDef("length",None,
                                         [-"[]" ==> !!"0";-"(x:xs)" ==> (Plus >> !!"1" >> (!!"length" >> !!"xs"))]))

      ]
    VerticalStack
      [
        TextBlock @"Since the type declaration is separated from the function body, type variables for generics can be written as normal variables"
        TextBlock @"Note that in Haskell the type of a list is written as [a] where a is a concrete type or a type variable"
        HaskellCodeBlock(TextSize.Small,
                          HaskellFuncDef("length", Some [!!"[a]";!!"Integral"],
                                          [-"[]" ==> !!"0";-"(x:xs)" ==> (Plus >> !!"1" >> (!!"length" >> !!"xs"))]))
      ]
    Section("Lazy evaluation")
    SubSection("Overview")
    VerticalStack
      [
        ItemsBlock
          [
            ! @"Haskell uses a mechanism of evaluation for expressions called \textit{lazy evaluation}"
            ! @"When binding an expression to a variable the expression is not evaluated immediately"
            ! @"The binding contains a ``recipe'' to evaluate the expression"
            ! @"The evaluation is delayed until the binding is actually used in the program"
            ! @"Unevaluated values are called \textit{thunks}"
          ]
      ]
    SubSection("Examples")
    VerticalStack
      [
        TextBlock @"Consider the following code:"       
        HaskellCodeBlock(TextSize.Small, Let(-"(x,y)",(MakePair >> (!!"length" >> !!"[1,2]") >> (!!"reverse" >> !!"[1,2]")),Hidden(!!"something")))
        ItemsBlock 
          [
            ! @"The variables \texttt{x} and \texttt{y} initially contain thunks, until at some point in the \texttt{in} body they are used"
            ! @"If the values are never used, they will never be evaluated"
          ]
      ]
    VerticalStack
      [
        TextBlock @"Consider the following code:"
        GenericCodeBlock(TextSize.Small, true, "let \n  z = (length [1,2],reverse [1,2]) \n  (n,s) = z \nin ...")
        ItemsBlock
          [
            ! @"At line 1 line \texttt{z} is simply a thunk"
            ! @"At line 2 the compiler must know if \texttt{z} is actually a pair, because the pattern must match the let binding"
            ! @"The compiler does not need to evaluate the content of the pair"
            ! @"Thus \texttt{(n,s)} becomes a pair of thunks, i.e. \texttt{z = (thunk,thunk)}"
          ]
      ]
    VerticalStack
      [
        TextBlock @"Consider the following code:"
        GenericCodeBlock(TextSize.Small, true, "let \n  z = (length [1,2],reverse [1,2]) \n  (n,s) = z\n  (1::ss) = s\nin ...")
        ItemsBlock
          [
            ! @"At line 4 the compiler must know if \texttt{s} is a list with the number 1 as head to match the pattern"
            ! @"The compiler needs to know if \texttt{s} is a list, thus it evaluates the result of \textttt{reverse} as a list with a thunk as a head and another thunk as a tail, so we have thunk:thunk"
            ! @"The compiler needs to know if the head of \texttt{s} matches the number 1, thus we have 1:thunk"
          ]
      ]
    VerticalStack
      [
        ItemsBlock
          [
            ! @"The figure below shows the possible evaluation of (2, [1,2])"
            ! @"WHNF = \textit{Weak head normal form}, i.e. when the evaluation contains both values and thunks"
            ! @"NF = \textit{Normal form}, i.e. when the evaluation contains only values and no thunks"
          ]
        Figure("thunks",0.2)
      ]
    SubSection("Lazy evaluation is a weirdo")
    VerticalStack
      [
        ItemsBlock
          [
            ! @"In the Haskell standard library we have a value called \texttt{undefined} which is used to capture errors in the program"
            ! @"When the program evaluates \texttt{undefined}, execution halts and an error is returned"
            ! @"Now consider the following code:"
          ]
        GenericCodeBlock(TextSize.Small, false, "let \n  failMiserably = \\x -> undefined \n  (x,y) = (4,failMiserably \"Please crash\") \nin\n  x")
        TextBlock @"Does it crash?"
      ]
    VerticalStack
      [
        GenericCodeBlock(TextSize.Small, true, "let \n  failMiserably = \\x -> undefined \n  (x,y) = (4,failMiserably \"Please crash\") \nin\n  x")
        TextBlock @"The answer is no!"
        ItemsBlock
          [
            ! @"At line 3  \texttt{(x,y) = (thunk,thunk)}"
            ! @"At line 5 the expression only uses \texttt{x}, thus only 4 is evaluated." 
            ! @"\texttt{y} is still a thunk, so the program will never know that it contains undefined"
            ! @"You might have an evaluation that actually failed but you will never know because of the lazy evaluation!"
          ]
      ]
    Section("The \"do\" notation and IO monad")
    SubSection("Overview")
    VerticalStack
      [
        ItemsBlock
          [
            ! @"Haskell, unlike F\#, is a pure funcional language"
            ! @"This means we cannot make calls to imperative functions just like in F\#. For example we cannot call something like \texttt{printf} because that is an imperative function"
            ! @"How can we print a value to the standard output?"
          ]
      ]
    VerticalStack
      [
        ItemsBlock
          [
            ! @"In Haskell the main function is always\footnote{For a large enough value of \textit{always}} defined as a \texttt{do} block"
            ! @"For example:"
          ]
        GenericCodeBlock(TextSize.Small, false, "main = do\n  putStr(\"Velociraptor\\n\")\n  print (velociraptor 30.0 10)")
        TextBlock @"The code allows you to print things on the shell"
      ]
    VerticalStack
      [
        TextBlock @"\textbf{You are cheating!!! That is imperative code!!! So all this course is about nothing because you cannot have pure functional programming!}"
        Pause
        ItemsBlock
          [
            ! @"No. The \texttt{do} notation is syntax to hide a functional structure called \textit{Monad}"
            ! @"We do not have time to explain monads in detail in this course, but they are structures that only use lambdas and a composition of lambdas to produce a result."
            ! @"It is possible to express imperative behaviours only with monads."
            ! @"If you are interested take a look at the State monad."
            ! @"In particular the IO Monad allows you to handle side effects, thus print on the shell"
          ]
      ]

    Section( @"Conclusion")
    SubSection("Closing up")
    ItemsBlock
      [
        ! @"Haskell can be mapped to the lambda calculus as we did for F\#: it looks mostly the same"
        ! @"It does not feature non-recursive let-bindings"
        ! @"It uses a lazy evaluation strategy that delays expanding values for as long as possible"
      ]
  ]

