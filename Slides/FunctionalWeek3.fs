module FunctionalWeek3

open CommonLatex
open LatexDefinition
open CodeDefinitionLambda
open Interpreter

let FullLambdaStateTrace = LambdaStateTrace
let LambdaStateTrace(ts, c, ms, p, q) = LambdaStateTrace(ts, c, ms, false, false, false, false, p, q)
let LambdaCodeBlock(x,y) = LambdaCodeBlock(x,y,false)

let slides =
  [
    Section("Introduction")
    SubSection("Lecture topics")
    ItemsBlock
      [
        ! @"Let"
        ! @"Tuples"
        ! @"Discriminated unions (polymorphism)"
      ]

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

    ItemsBlock
      [
        ! @"Lets are simply translated to function applications"
        ! @"\texttt{let x = t in u  } simply becomes \texttt{  ($\lambda$x$\rightarrow$ u)\ t}"
      ]

    FullLambdaStateTrace(TextSize.Small, Let(-"age",!!"9",(Plus >> !!"age") >> !!"age"), None, false, false, false, true, true, true)

    Section( @"Data types")
    SubSection("Overview")
    ItemsBlock[
        ! @"We now move on to ways to define data types"
        ! @"The definitions will be both \textbf{minimal} and \textbf{composable}"
        ! @"Classes, polymorphism, etc. can all be rendered under our definitions, so we miss nothing substantial"
      ]

    TextBlock @"Notice: from now on we will start ignoring the reduction steps for simple terms such as \texttt{3+3}, \texttt{x = 0}, etc. for brevity"

    SubSection("Minimality")
    ItemsBlock[
        ! @"The lambda calculus has so far proven very powerful, despite its size"
        ! @"We do not need hundreds of different operators, we can simply build them\footnote{and more}"
        ! @"The only extension needed is purely syntactic in nature to make it more mnemonic, but this is only skin-deep and requires no change to the underlying mechanisms of the lambda calculus"
      ]

    ItemsBlock[
        ! @"In defining data types we wish to maintain this minimality"
        ! @"We do not want dozens of separate, competing data types all slightly overlapping"
      ]

    SubSection("Fundamental scenarios")
    ItemsBlock
      [
        ! @"\textbf{Tuples}: storing multiple things together at the same time, like the fields and methods in a class"
        ! @"\textbf{Unions}: storing either one of various things at a time, like an interface that is exactly one of its concrete implementors"
      ]

    SubSection("The importance of composition")
    ItemsBlock[
        ! @"We just need to cover the case of two items, higher numbers come through composition"
        ! @"For example, given the ability to store a pair, we can build a pair of pairs to create arbitrary tuples"
        ! @"Similarly, given the ability to store either of two values, we can build either of many values with nesting"
      ]

    Section( @"Tuples")
    SubSection("Pair of values")
    VerticalStack
      [
        ItemsBlock[
            ! @"A pair of values is defined simply as something that stores these two values"
            ! @"We can extract them by giving the pair a function that will receive the values"
          ]

        LambdaCodeBlock(TextSize.Small, (deltaRules MakePair).Value)
      ]

    LambdaStateTrace(TextSize.Small, (MakePair >> !!"1") >> !!"2", Option.None, true, true)

    VerticalStack
      [
        ItemsBlock[
            ! @"We can define two utility functions that, given a pair, extract the first or second value"
            ! @"They are usually called $\pi_1$ and $\pi_2$, or \texttt{fst} and \texttt{snd}"
          ]

        LambdaCodeBlock(TextSize.Small, (deltaRules First).Value)
        LambdaCodeBlock(TextSize.Small, (deltaRules Second).Value)
      ]

    LambdaStateTrace(TextSize.Small, First >> ((MakePair >> !!"1") >> !!"2"), Option.None, true, true)

    TextBlock @"We should expect that $\pi_1$ and $\pi_2$ are inverse operations to constructing a pair, as they destroy it"
    LambdaStateTrace(TextSize.Small, Let(-"p", (MakePair >> !!"1") >> !!"2", (MakePair >> (First >> !!"p")) >> (Second >> !!"p")), 
                     Option.None, false, true)

    Section( @"Discriminated unions")
    SubSection("Choice between a pair of values")
    VerticalStack
      [
        ItemsBlock[
            ! @"A choice of values is defined simply as something that stores either of two possible values"
            ! @"We call such a choice a \textbf{discriminated union}"
            ! @"We build a discriminated union with either of two functions to build the first or the second value"
            ! @"They are usually called \texttt{inl} and \texttt{inr}\footnote{\textit{in} stands for injection, and \textit{l} and \textit{r} stand for left and right}"
          ]

        LambdaCodeBlock(TextSize.Small, (deltaRules Inl).Value)
        LambdaCodeBlock(TextSize.Small, (deltaRules Inr).Value)
      ]

    LambdaStateTrace(TextSize.Small, Inl >> !!"1", Option.None, false, true)
    LambdaStateTrace(TextSize.Small, Inr >> !!"TRUE", Option.None, false, true)

    VerticalStack
      [
        ItemsBlock[
            ! @"Extracting the input of a discriminated union is a process known as \texttt{match}\footnote{which is a sort of \texttt{switch}, just on steroids}"
            ! @"Given a union and two functions (one per case), if the union was the first case we apply the first function, otherwise we apply the second function"
          ]

        LambdaCodeBlock(TextSize.Small, (deltaRules Match).Value)
      ]

    LambdaStateTrace(TextSize.Small, ((Match >> (Inl >> !!"1")) >> (-"x" ==> ((Plus >> !!"x") >> !!"1"))) >> (-"y" ==> ((And >> !!"y") >> False)), Option.None, false, true)

    TextBlock @"We should expect that \texttt{inl} and \texttt{inr} are inverse operations to \texttt{match}"
    LambdaStateTrace(TextSize.Small, (Match >> (Inl >> !!"1")) >> Inl >> Inr, Option.None, false, false)
    LambdaStateTrace(TextSize.Small, (Match >> (Inr >> True)) >> Inl >> Inr, Option.None, false, false)

    Section "Conclusion"
    SubSection(@"Recap")
    ItemsBlock[
        ! @"Lambda terms can be used to encode arbitrary basic data types"
        ! @"The terms are always lambda expression which, when they get parameters passed in, identify themselves somehow"
        ! @"Identification can be done by applying something (possibly even a given number of times), or returning one of the parameters"
      ]

    SubSection(@"Recap")
    ItemsBlock[
        ! @"The data types we have seen cover an impressive range of applications"
        ! @"Tuples cover grouping data together (like the fields of a class)"
        ! @"Unions cover choosing different things (like the polymorphism of an interface that might be implemented by various concrete classes)"
        ! @"Combining these two covers all possible programming needs, even for more complex data structures"
      ]
  ]
