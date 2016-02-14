module FunctionalWeek3

open CommonLatex
open SlideDefinition
open CodeDefinitionLambda
open Interpreter

// highlighting must split terms which are too long

let slides =
  [
    Section("Introduction")
    SubSection("Lecture topics")
    ItemsBlock
      [
        ! @"Let-in"
        ! @"Tuples"
        ! @"Discriminated unions (polymorphism)"
        ! @"Lists"
        ! @"Let-rec"
      ]

    TextBlock @"From now on we will start ignoring the reduction steps for simple terms such as \texttt{3+3}"

    Section( @"\texttt{Let-in}")
    SubSection("Idea")
    ItemsBlock
      [
        ! @"Sometimes we wish to give a name to a value or a computation, to reuse later"
        ! @"This construct is called \texttt{let-in}"
        ! @"We could then say something like \texttt{let age = 9 in age + age}"
        ! @"We can nest \texttt{let-in} constructs, and then say something like \texttt{let age = 9 in (let x = 2 in age * x)}"
        Pause
        ! @"This makes code significantly more readable, as it looks like a series of declarations top-to-bottom"
    ]

    LambdaStateTrace(TextSize.Small, Let("age",!!"9",(Plus >>> !!"age") >>> !!"age"), None, false, false, true)

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
        LambdaCodeBlock(TextSize.Small, "f" ==> ("n" ==> ((If >>> (IsZero >>> !!"n") >>> !!"1") >>> ((Mult >>> (!!"f" >>> ((Minus >>> !!"n") >>> !!"1"))) >>> !!"n"))))
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

    LambdaStateTrace(TextSize.Small, (Fix >>> ("f" ==> ("n" ==> ((If >>> (IsZero >>> !!"n") >>> !!"1") >>> ((!!"f" >>> ((Minus >>> !!"n") >>> !!"1"))))))) >>> !!"2", 
                      Option.None, false, false, false)

    VerticalStack
      [
        TextBlock "We can now try our hand at a factorial computation"

        LambdaCodeBlock(TextSize.Small, (Fix >>> ("f" ==> ("n" ==> ((If >>> (IsZero >>> !!"n") >>> !!"1") >>> ((Mult >>> (!!"f" >>> ((Minus >>> !!"n") >>> !!"1"))) >>> !!"n"))))) >>> !!"2")
      ]
    LambdaStateTrace(TextSize.Small, (Fix >>> ("f" ==> ("n" ==> ((If >>> (IsZero >>> !!"n") >>> !!"1") >>> ((Mult >>> (!!"f" >>> ((Minus >>> !!"n") >>> !!"1"))) >>> !!"n"))))) >>> !!"2", 
                      Option.None, false, false, false)


// tuples
// discriminated unions
// lists


//    Section "Conclusion"
//    SubSection(@"Recap")
//    ItemsBlock[
//        ! @"Lambda terms can be used to encode arbitrary basic data types"
//        ! @"The terms are always lambda expression which, when they get parameters passed in, identify themselves somehow"
//        ! @"Identification can be done by applying something (possibly even a given number of times), or returning one of the parameters"
//      ]
//
//    SubSection(@"Recap")
//    ItemsBlock[
//        ! @"There are many encodings of data types, but they all behave in the same way by producing the same outputs for the same inputs"
//        ! @"From now on we will start ignoring the reduction steps for simple terms such as \texttt{3+3}"
//        ! @"We will instead focus on more complex data structures, such as tuples, discriminated unions, and even lists"
//      ]

  ]


// A series of tests
//    LambdaStateTrace(TextSize.Small, ((If >>> ((And >>> (IsZero >>> !!"0")) >>> False) >>> !!"0") >>> ((Plus >>> !!"1") >>> !!"3")), None, false, false, false)
//    LambdaStateTrace(TextSize.Small, Let("age",!!"9",(Plus >>> !!"age") >>> !!"age"), None, false, false, true)
//    TextBlock "Shortened test" ////////////////////////////////////////////////////////////////////////
//    LambdaStateTrace(TextSize.Small, Let("age",(Plus >>> !!"9") >>> !!"1",(Plus >>> !!"age") >>> !!"age"), None, false, false, false)

