module FunctionalWeek4

open CommonLatex
open LatexDefinition
open CodeDefinitionLambda
open Interpreter

let LambdaCodeBlock(x,y) = LambdaCodeBlock(x,y,false)
let FSharpCodeBlock(x,y) = FSharpCodeBlock(x,y,false)


let slides =
  [
    Section("Introduction")
    SubSection("Lecture topics")
    ItemsBlock
      [
        ! @"Recursion (\texttt{let-rec})"
        ! @"F\# translations of lambda programs so far"
      ]

    Section( @"Recursive functions and  \texttt{let-rec}")
    SubSection("Idea")
    ItemsBlock
      [
        ! @"The lambda calculus has no \texttt{while} loops"
        ! @"This means that we need to emulate them with recursion"
    ]

    ItemsBlock
      [
        ! @"This is a bit of an issue"
        ! @"A function is just a lambda term, which does not have a name"
        ! @"If the function does not have a name, how do we call it from its own body?"
      ]

    ItemsBlock
      [
        ! @"We can define a recursive function as a function with an extra parameter"
        ! @"Calling the extra parameter will result in calling the function itself"
      ]

    VerticalStack
      [
        TextBlock @"For example, the factorial function becomes"
        LambdaCodeBlock(TextSize.Small, -"f" ==> (-"n" ==> ((If >> (IsZero >> !!"n") >> !!"1") >> ((Mult >> (!!"f" >> ((Minus >> !!"n") >> !!"1"))) >> !!"n"))))
      ]

    VerticalStack
      [
        ItemsBlock
          [
            ! @"We now need an external operator that handles recursive functions properly"
            ! @"This must ensure that a recursive function gets itself as a parameter, in an endless chain"
            ! @"This combinator is known as \texttt{fixpoint} operator"
          ]

        LambdaCodeBlock(TextSize.Small, (deltaRules Fix).Value)
      ]

    LambdaStateTrace(TextSize.Small, (Fix >> (-"f" ==> (-"n" ==> ((If >> (IsZero >> !!"n") >> !!"1") >> ((!!"f" >> ((Minus >> !!"n") >> !!"1"))))))) >> !!"2", 
                      Option.None, false, false, false, false, true, true)

    VerticalStack
      [
        TextBlock "We can now try our hand at a factorial computation"

        LambdaCodeBlock(TextSize.Small, (Fix >> (-"f" ==> (-"n" ==> ((If >> (IsZero >> !!"n") >> !!"1") >> ((Mult >> (!!"f" >> ((Minus >> !!"n") >> !!"1"))) >> !!"n"))))) >> !!"2")
      ]
    LambdaStateTrace(TextSize.Small, (Fix >> (-"f" ==> (-"n" ==> ((If >> (IsZero >> !!"n") >> !!"1") >> ((Mult >> (!!"f" >> ((Minus >> !!"n") >> !!"1"))) >> !!"n"))))) >> !!"2", 
                      Option.None, false, false, false, false, true, true)

    Section( @"Translating to F\#")
    SubSection("Overview")
    ItemsBlock
      [
        ! @"Each and every one of the constructs we have seen so far has a direct translation in F\#\footnote{Haskell is a bit different, so we leave it for later}"
        ! @"All constructs have exactly the same behaviour as in the lambda calculus, but with a slightly less mathematical syntax for ASCII keyboards"
        ! @"A few operators are menemonically friendlier or just plain more readable, but the essence remains exactly the same"
        ! @"F\# is indentation-sensitive, like Python: pay a lot of attention to how terms are indented!"
      ]

    SubSection("Basic expressions")
    VerticalStack
      [
        TextBlock @"Integers, booleans, floats, strings have the usual meaning, both in the lambda calculus, F\#, and the languages you are used to:"

        FSharpCodeBlock(TextSize.Small, (Minus >> ((Plus >> !!"2") >> !!"3")) >> !!"4")
        FSharpCodeBlock(TextSize.Small, (And >> True) >> False)
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

        FSharpCodeBlock(TextSize.Small, ((If >> ((And >> True) >> False)) >> !!"0") >> !!"1")
      ]

    VerticalStack
      [
        TextBlock @"\textbf{FSharp}"

        FSharpCodeBlock(TextSize.Small, ((If >> ((And >> True) >> False)) >> !!"0") >> !!"1")

        TextBlock @"\textbf{Lambda calculus}"
        LambdaCodeBlock(TextSize.Small, ((If >> ((And >> True) >> False)) >> !!"0") >> !!"1")
        LambdaCodeBlock(TextSize.Small, (deltaRules If).Value >> ((And >> True) >> False) >> !!"0" >> !!"1")
      ]

    SubSection("Anonymous functions")
    VerticalStack
      [
        TextBlock @"Functions look very similar, with \texttt{fun} instead of $\lambda$"
        FSharpCodeBlock(TextSize.Small, -"x" ==> (-"f" ==> (!!"f" >> !!"x")))

        Pause

        TextBlock @"Just like function application"
        FSharpCodeBlock(TextSize.Small, ((-"x" ==> (-"f" ==> (!!"f" >> !!"x"))) >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x")))
      ]

    VerticalStack
      [
        TextBlock @"\textbf{FSharp}"
        FSharpCodeBlock(TextSize.Small, ((-"x" ==> (-"f" ==> (!!"f" >> !!"x"))) >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x")))

        TextBlock @"\textbf{Lambda calculus}"
        LambdaCodeBlock(TextSize.Small, ((-"x" ==> (-"f" ==> (!!"f" >> !!"x"))) >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x")))
        
      ]

    SubSection("Named functions")
    VerticalStack
      [
        TextBlock @"We can give names to functions, and code becomes much prettier as a result"

        FSharpCodeBlock(TextSize.Small, Let(-"apply", -"x" ==> (-"f" ==> (!!"f" >> !!"x")), (!!"apply" >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x"))))

        //LambdaCodeBlock(TextSize.Small, Let(-"apply", -"x" ==> (-"f" ==> (!!"f" >> !!"x")), (!!"apply" >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x"))))
      ]

    VerticalStack
      [
        TextBlock @"\textbf{FSharp}"
        FSharpCodeBlock(TextSize.Small, Let(-"apply", -"x" ==> (-"f" ==> (!!"f" >> !!"x")), (!!"apply" >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x"))))

        TextBlock @"\textbf{Lambda calculus}"
        LambdaCodeBlock(TextSize.Small, Let(-"apply", -"x" ==> (-"f" ==> (!!"f" >> !!"x")), (!!"apply" >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x"))))
        LambdaCodeBlock(TextSize.Small, (deltaRules (Let(-"apply",-"x" ==> (-"f" ==> (!!"f" >> !!"x")), (!!"apply" >> !!"3") >> (-"x" ==> ((Plus >> !!"3") >> !!"x"))))).Value)

      ]


    SubSection("Recursive functions")
    VerticalStack
      [
        TextBlock @"We can also give names to recursive functions by using \texttt{let rec} instead of \texttt{let}, and code becomes much more readable than with \texttt{fix}"

        FSharpCodeBlock(TextSize.Small, 
          Let(-"fact", 
              (Fix >> (-"f" ==> (-"n" ==> ((If >> (IsZero >> !!"n") >> !!"1") >> ((Mult >> (!!"f" >> ((Minus >> !!"n") >> !!"1"))) >> !!"n"))))), 
              !!"fact" >> !!"2"))
      ]

    VerticalStack
      [
        TextBlock @"\textbf{FSharp}"
        FSharpCodeBlock(TextSize.Small, 
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
        TextBlock @"We can define tuples by just putting a comma between the values, with or without nesting for more than two values is done for us"

        FSharpCodeBlock(TextSize.Small, (MakePair >> !!"1") >> True)
      ]

    VerticalStack
      [
        TextBlock @"\textbf{FSharp}"
        FSharpCodeBlock(TextSize.Small, (MakePair >> !!"1") >> True)

        TextBlock @"\textbf{Lambda calculus}"
        LambdaCodeBlock(TextSize.Small, (MakePair >> !!"1") >> True)
        LambdaCodeBlock(TextSize.Small, ((deltaRules MakePair).Value >> !!"1") >> True)
      ]

    ItemsBlock
      [
        ! @"Nested tuples are the same as in the lambda calculus, and of course they also work in F\#"
        ! @"They are, in essence, pairs of pairs, such as: \texttt{(1,(2,(3,4)} or \texttt{(((1,2),3),4)}, which are not the same"
        ! @"Non-nested tuples are a slight extension for practical ease of work in F\#"
        ! @"They are, in essence, tuples with as many elements as we want, such as: \texttt{(1,2,3,4)}"
        ! @"All the examples in this slide are valid in F\#, but are not identical objects"
      ]

    VerticalStack
      [
        ItemsBlock
          [
            ! @"Functions such as $\pi_1$ and $\pi_2$, which both extract one item of a pair, also exist in F\#"
            ! @"They are called, respectively, \texttt{fst} and \texttt{snd}"
          ]

        FSharpCodeBlock(TextSize.Small, First >> ((MakePair >> !!"1") >> True))
        FSharpCodeBlock(TextSize.Small, Second >> ((MakePair >> !!"1") >> True))
      ]

    VerticalStack
      [
        TextBlock @"\textbf{FSharp}"
        FSharpCodeBlock(TextSize.Small, First >> ((MakePair >> !!"1") >> True))
        FSharpCodeBlock(TextSize.Small, Second >> ((MakePair >> !!"1") >> True))

        TextBlock @"\textbf{Lambda calculus}"
        LambdaCodeBlock(TextSize.Small, First >> ((MakePair >> !!"1") >> True))
        LambdaCodeBlock(TextSize.Small, (deltaRules First).Value >> ((MakePair >> !!"1") >> True))
        LambdaCodeBlock(TextSize.Small, Second >> ((MakePair >> !!"1") >> True))
        LambdaCodeBlock(TextSize.Small, (deltaRules Second).Value >> ((MakePair >> !!"1") >> True))
      ]

    VerticalStack
      [
        ItemsBlock 
          [
            ! @"Of course F\#, just like the lambda calculus, imposes no limitation on composition"
            ! @"Why not build a tuple of functions, then? Why not indeed!"
          ]

        Pause

        FSharpCodeBlock(TextSize.Small, (MakePair >> (-"x" ==> ((Plus >> !!"1") >> !!"x")) >> (-"x" ==> ((Mult >> !!"2") >> !!"x"))))

        Pause

        TextBlock "Could we also build a tuple of tuples of numbers and functions returning tuples of functions?"

        Pause

        TextBlock "Yes, but we are not going to do it :)"
      ]

    SubSection("Discriminated unions")
    VerticalStack
      [
        ItemsBlock
          [
            ! @"F\# also offers built-in discriminated unions"
            ! @"Functions such as \texttt{inl} and \texttt{inr}, which both embed one item into the union, also exist in F\#"
            ! @"They are called, respectively, \texttt{Choice1Of2} and \texttt{Choice2Of2}"
          ]

        FSharpCodeBlock(TextSize.Small, Inl >> !!"1")
        FSharpCodeBlock(TextSize.Small, Inr >> True)
      ]

    VerticalStack
      [
        TextBlock @"\textbf{FSharp}"
        FSharpCodeBlock(TextSize.Small, Inl >> !!"1")
        FSharpCodeBlock(TextSize.Small, Inr >> True)

        TextBlock @"\textbf{Lambda calculus}"
        LambdaCodeBlock(TextSize.Small, Inl >> !!"1")
        LambdaCodeBlock(TextSize.Small, (deltaRules Inl).Value >> !!"1")
        LambdaCodeBlock(TextSize.Small, Inr >> True)
        LambdaCodeBlock(TextSize.Small, (deltaRules Inr).Value >> True)
      ]

    VerticalStack
      [
        TextBlock @"We can, of course, perform matches on discriminated unions"

        FSharpCodeBlock(TextSize.Small, (Match >> (Inl >> !!"1") >> (-"x" ==> (Inl >> !!"x"))) >> (-"x" ==> (Inr >> !!"x")))
        
        Pause

        FSharpCodeBlock(TextSize.Small, Let(-"i", ((Match >> (Inl >> !!"1") >> (-"x" ==> !!"x")) >> (-"x" ==> !!"0")), (Mult >> !!"i") >> !!"2"))
      ]

    VerticalStack
      [
        TextBlock @"Discriminated unions, and their corresponding matches, can be nested as deep as we need"

        FSharpCodeBlock(
          TextSize.Small, 
          (Match >> (Inl >> (Inr >> True)) >> 
            (-"x" ==> (((Match >> !!"x") >> (-"x" ==> (Inl >> !!"x"))) >> (-"x" ==> (Inr >> !!"x"))))) >> 
            (-"y" ==> (Inr >> !!"y")))
      ]

    SubSection("Structural equality")
    ItemsBlock
      [
        ! @"Composable data types make it possible for F\# to build a series of useful functions automatically"
        ! @"These functions allow automatic comparison of data structures with the same shape (for example, tuples with tuples or lists with lists)"
        ! @"Comparison can work with values made up of primitive expressions, tuples, unions, lists, records, and any composition of them"
        ! @"With the only exclusion of functions: functions, or composite values containing functions, may not be compared"
    ]

    SubSection("Interoperability")
    ItemsBlock
      [
        ! @"One of the practical strengths of F\# is its ability to fully interoperate with the .Net framework"
        ! @"Any feature or library available, even if written in other languages like C\#, can be used from inside F\#"
        ! @"Moreover, any F\# library can be used from other languages like C\#"
        ! @"This means that applications could be the internal logic in F\#, and the UI and database store in C\#"
      ]

    ItemsBlock
      [
        ! @"For example, we can call functions such as \texttt{System.Console.WriteLine}, \texttt{System.Console.ReadLine}, \texttt{System.Int32.Parse}, etc."
        ! @"This gives a whole other dimension of usefulness to F\#"
        ! @"Ranging from games to web applications, the language knows no intrinsic limitation"
        ! @"This also comes with full IDE, autocompletion, and debugger support in Visual Studio and a few other IDE's"
        ! @"Scala is to some extent comparable"
      ]

    TextBlock @"A short F\# demo might be in order"

    SubSection("Forbidden features")
    ItemsBlock
      [
        ! @"F\# is a hybrid language"
        ! @"It also contains OO features, mutability (variables), interfaces, classes, inheritance, etc."
        ! @"You may only use these features in the assignments if there is no alternative to access a library you need"
        ! @"Mutability may only be used in the main program, which then calls regular functions that only use the lambda calculus derived core"
        ! @"Failure to comply will result in an automatic insufficient grade"
      ]

    SubSection("About types")
    ItemsBlock
      [
        ! @"A major difference between F\# and the lambda calculus is that F\# is, in fact, a statically typed language"
        ! @"Even though the language does not require type declarations like Java or C\#, the compiler will still ensure that the types make sense"
        ! @"This means that we will get compiler errors because we mix types"
        ! @"Code like \texttt{fst 1} will not compile, for example, because \texttt{1} is not a pair and therefore \texttt{fst} cannot be applied to it"
        ! @"We will cover the type system of F\# in the next lecture"
      ]

    Section "Homework"
    SubSection(@"Ways to exercise")
    ItemsBlock[
        ! @"We have added a ton of F\# homework, with solutions"
        ! @"It is all on GitHub"
        ! @"It is not mandatory, but it is a good idea to do it until you feel more sure"
        ! @"You may discuss it during one of the two practicums"
      ]

    Section "Conclusion"
    SubSection(@"Recap")
    ItemsBlock[
        ! @"The lambda calculus can be translated, term to term, into F\#"
        ! @"F\# is therefore just a practical lambda calculus with a series of handy extensions and slightly more readable"
      ]

    Section "Practicum begins now"
  ]

