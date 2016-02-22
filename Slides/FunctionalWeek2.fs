module FunctionalWeek2

open CommonLatex
open LatexDefinition
open CodeDefinitionLambda
open Interpreter

let LambdaStateTrace(x,y,z) = LambdaStateTrace(x,y,z,true,true,true,true,true,true)
let LambdaCodeBlock(x,y) = LambdaCodeBlock(x,y,false)

let slides =
  [
    Section("Introduction")
    SubSection("Lecture topics")
    ItemsBlock
      [
        !"Make it pretty: delta rules"
        !"Booleans, boolean logic operators, if-then-else"
        !"Naturals, arithmetic operators, comparison operators"
      ]

    Section("Encoding boolean logic")
    SubSection("Introduction")
    ItemsBlock
      [
        ! @"We can decide that some specific lambda terms have special meanings"
        ! @"For example, we could decide that a given lambda term means \texttt{TRUE}, another \texttt{FALSE}, etc."
        ! @"The important thing is that we choose terms that behave as we wish"
      ]

    SubSection("As we wish?")
    ItemsBlock
      [
        ! @"Suppose we define some lambda terms for \texttt{TRUE}, \texttt{FALSE}, and \texttt{AND}"
        ! @"We expect these terms to reduce\footnote{That is, computed according to $\rightarrow_\beta$} following our expectations of boolean logic"
        ! @"We can use truth tables to encode our expectations"
      ]

    SubSection("Specification with, for example, truth tables")
    VerticalStack
      [
        TextBlock @"We want to formulate \texttt{TRUE}, \texttt{FALSE}, and \texttt{AND} so that"
        ItemsBlock
          [
            ! @"\texttt{TRUE} $\wedge$ \texttt{TRUE} $\rightarrow_\beta$ \texttt{TRUE}"
            ! @"\texttt{TRUE} $\wedge$ \texttt{FALSE} $\rightarrow_\beta$ \texttt{FALSE}"
            ! @"\texttt{FALSE} $\wedge$ \texttt{TRUE} $\rightarrow_\beta$ \texttt{FALSE}"
            ! @"\texttt{FALSE} $\wedge$ \texttt{FALSE} $\rightarrow_\beta$ \texttt{FALSE}"
          ]
      ]

    Section("Defining terms with special meaning")
    SubSection("Choice terms")
    ItemsBlock
      [
        ! @"Terms with special meaning essentially make a choice when given parameters"
        ! @"The choice is expressed by either returning, or applying, the parameters"
      ]

    SubSection("Delta rules")
    ItemsBlock
      [
        ! @"We wish to use special symbols to these terms with special meaning"
        ! @"We define a series of delta rules, which are transformation from pretty symbols into lambda terms (and vice-versa)"
      ]

    TextBlock @"This means that we will be able to write lambda programs such as \texttt{5+3}, that will then be translated into the appropriate lambda terms"

    Section("Booleans")
    SubSection("Idea")
    ItemsBlock
      [
        ! @"Boolean operators such as \texttt{TRUE} and \texttt{FALSE} must be defined so as to identify themselves"
        ! @"The choice is expressed by returning their identity from a choice of two options"
      ]

    SubSection(@"\texttt{TRUE} and \texttt{FALSE}")
    VerticalStack
      [
        TextBlock @"\texttt{TRUE} is defined as a selector of the representative for true, that is the first argument\footnote{by arbitrary convention}"
        LambdaCodeBlock(TextSize.Small, (CodeDefinitionLambda.deltaRules Term.True).Value)
        TextBlock @"\texttt{FALSE} is defined as a selector of the representative for false, that is the second argument\footnote{by arbitrary convention, as long as different from the previous}"
        LambdaCodeBlock(TextSize.Small, (CodeDefinitionLambda.deltaRules Term.False).Value)
      ]

    LambdaStateTrace(TextSize.Small, True >> !!"bit1" >> !!"bit0", None)

    SubSection(@"\texttt{AND}")
    ItemsBlock
      [
        ! @"The conjunction\footnote{\texttt{AND}, or $\wedge$} of two terms is a function that takes as input two booleans and returns a boolean"
        ! @"Since we just defined booleans to be two-parameter functions, we know that the two input booleans can be applied to each other"
        ! @"Given two booleans \texttt{p} and \texttt{q}, their conjunction is \texttt{q} if \texttt{p} was \texttt{true}, or \texttt{false} otherwise"
        LambdaCodeBlock(TextSize.Small, (CodeDefinitionLambda.deltaRules Term.And).Value)
      ]

    TextBlock(@"Let us begin to with \texttt{TRUE} $\wedge$ \texttt{TRUE} $\rightarrow_\beta$ \texttt{TRUE}")
    LambdaStateTrace(TextSize.Small, ((And >> True) >> True), None)

    VerticalStack
      [
        TextBlock(@"It works, but it is probably only because of black magic.")
        Pause
        TextBlock(@"Or is it? Let's see if we can get lucky again...")
      ]

    SubSection(@"\texttt{OR}")
    ItemsBlock
      [
        ! @"The disjunction\footnote{\texttt{OR}, or $\vee} of two terms is a function that takes as input two booleans and returns a boolean"
        ! @"Like with conjunction, remember that the two input booleans can be applied to one another"
        ! @"Given two booleans \texttt{p} and \texttt{q}, their disjunction is \texttt{true} if \texttt{p} was \texttt{true}, or \texttt{q} otherwise"
        LambdaCodeBlock(TextSize.Small, (CodeDefinitionLambda.deltaRules Term.Or).Value)
      ]

    TextBlock(@"Let us begin to with \texttt{TRUE} $\vee$ \texttt{TRUE} $\rightarrow_\beta$ \texttt{TRUE}")
    LambdaStateTrace(TextSize.Small, ((Or >> True) >> True), None)

    SubSection(@"\texttt{if-then-else}")
    ItemsBlock
      [
        ! @"The conditional operator \texttt{if-then-else} chooses one of two parameters based on the value of the input condition"
        ! @"Given a boolean \texttt{c} and two values \texttt{th} and \texttt{el}, the result is \texttt{th} if \texttt{c} was \texttt{true}, or \texttt{el} otherwise"
        ! @"Since \texttt{c} is a boolean, it already performs this choice!"
        LambdaCodeBlock(TextSize.Small, (CodeDefinitionLambda.deltaRules Term.If).Value)
      ]

    TextBlock(@"Let us try with \texttt{if TRUE} $\vee$ \texttt{FALSE then A else B} $\rightarrow_\beta$ \texttt{A}")
    LambdaStateTrace(TextSize.Small, (((If >> True) >> !!"A") >> !!"B"), None)

    Section("Natural numbers")
    SubSection("Idea")
    ItemsBlock
      [
        ! @"Natural numbers such as \texttt{3} and \texttt{0} must be defined so as to identify themselves"
        ! @"Their identity is determined by how many times they perform an action"
        ! @"The only action we have available is applying a function to a term"
    ]

    ItemsBlock
      [
        ! @"We will use unary numbers"
        ! @"A number is defined by how many times it applies a function to a given term"
        ! @"Zero applications are also possible, in this case we default to the given term"
    ]

    SubSection(@"\texttt{0}, \texttt{1}, etc.")
    TextBlock @"A number is defined as an applicator of a term identifying as successor to another term identifying as zero\footnote{first and second arguments by arbitrary convention}"

    VerticalStack
      [
        TextBlock @"0 will thus look like"
        LambdaCodeBlock(TextSize.Small, (CodeDefinitionLambda.deltaRules !!"0").Value)
        TextBlock @"1 will look like"
        LambdaCodeBlock(TextSize.Small, (CodeDefinitionLambda.deltaRules !!"1").Value)
        TextBlock @"7 will look like"
        LambdaCodeBlock(TextSize.Small, (CodeDefinitionLambda.deltaRules !!"7").Value)
        TextBlock @"etc."
      ]

    SubSection(@"Addition")
    ItemsBlock[
        ! @"Adding numbers is a function that takes as input two numbers (say \texttt{m} and \texttt{n}), and returns a number"
        ! @"The first number applies its first parameter \texttt{m} times to its second parameter"
        ! @"The second number applies its first parameter \texttt{n} times to its second parameter"
        ! @"We can use the second number as the second parameter to the first, therefore obtaining something that applies \texttt{m+n} times"
        LambdaCodeBlock(TextSize.Small, (CodeDefinitionLambda.deltaRules Plus).Value)
      ]

    TextBlock(@"Let us try it out to \texttt{2} $+$ \texttt{1} $\rightarrow_\beta$ \texttt{3}")
    LambdaStateTrace(TextSize.Small, ((Plus >> !!"2") >> !!"1"), None)

    SubSection(@"Multiplication")
    ItemsBlock[
        ! @"Multiplying numbers is a function that takes as input two numbers (say \texttt{m} and \texttt{n}), and returns a number"
        ! @"The first number applies its first parameter \texttt{m} times to its second parameter"
        ! @"The second number applies its first parameter \texttt{n} times to its second parameter"
        ! @"We can use the second number as the first parameter to the first, therefore obtaining something that applies \texttt{n+} \texttt{m} times, starting from \texttt{z}"
        LambdaCodeBlock(TextSize.Small, (CodeDefinitionLambda.deltaRules Mult).Value)
      ]

    TextBlock(@"Let us try it out to \texttt{2} $\times$ \texttt{2} $\rightarrow_\beta$ \texttt{4}")
    LambdaStateTrace(TextSize.Small, ((Mult >> !!"2") >> !!"2"), None)

    SubSection(@"Zero checking")
    ItemsBlock[
        ! @"We might wish to verify whether or not a number is zero"
        ! @"We can simply pass the number parameters that fail the check (\texttt{s}) and pass it (\texttt{z})"
        LambdaCodeBlock(TextSize.Small, (CodeDefinitionLambda.deltaRules IsZero).Value)
      ]

    TextBlock(@"Let us try it out to \texttt{0 = 2} $\rightarrow_\beta$ \texttt{FALSE}")
    LambdaStateTrace(TextSize.Small, IsZero >> !!"2", None)

    SubSection(@"Other arithmetic operators")
    ItemsBlock[
        ! @"Division, subtraction, and all manners of comparison operators can be defined similarly"
        ! @"The level of detail of the specification can be compared to that of a very high level CPU"
        ! @"This means that we are, to an extent, programming in a sort of assembly"
        ! @"This is the reason why the traces have been so verbose so far"
      ]

    ItemsBlock[
        ! @"We could also define numbers in base two instead of base one"
        ! @"This would save processing time, but would result in a slighter more complex specification"
        ! @"We will just ignore these engineering details: we only focus on \textbf{what} can be done, not the best way to do it"
      ]

    Section "Conclusion"
    SubSection(@"Recap")
    ItemsBlock[
        ! @"Lambda terms can be used to encode arbitrary basic data types"
        ! @"The terms are always lambda expression which, when they get parameters passed in, identify themselves somehow"
        ! @"Identification can be done by applying something (possibly even a given number of times), or returning one of the parameters"
      ]

    SubSection(@"Recap")
    ItemsBlock[
        ! @"There are many encodings of data types, but they all behave in the same way by producing the same outputs for the same inputs"
        ! @"From now on we will start ignoring the reduction steps for simple terms such as \texttt{3+3}"
        ! @"We will instead focus on more complex data structures, such as tuples, discriminated unions, and even lists"
      ]

    Section "Appendix"
    SubSection("False derivation")
    LambdaStateTrace(TextSize.Small, False >> !!"bit1" >> !!"bit0", None)

    SubSection(@"Remaining and derivations")
    TextBlock(@"Let us move to \texttt{TRUE} $\wedge$ \texttt{FALSE} $\rightarrow_\beta$ \texttt{FALSE}")
    LambdaStateTrace(TextSize.Small, ((And >> True) >> False), None)

    TextBlock(@"Let us move to \texttt{FALSE} $\wedge$ \texttt{TRUE} $\rightarrow_\beta$ \texttt{FALSE}")
    LambdaStateTrace(TextSize.Small, ((And >> False) >> True), None)

    TextBlock(@"Let us move to \texttt{FALSE} $\wedge$ \texttt{FALSE} $\rightarrow_\beta$ \texttt{FALSE}")
    LambdaStateTrace(TextSize.Small, ((And >> False) >> False), None)

    SubSection("Remaining or derivations")
    TextBlock(@"Let us begin to with \texttt{TRUE} $\vee$ \texttt{FALSE} $\rightarrow_\beta$ \texttt{TRUE}")
    LambdaStateTrace(TextSize.Small, ((Or >> True) >> False), None)
    TextBlock(@"Let us begin to with \texttt{False} $\vee$ \texttt{TRUE} $\rightarrow_\beta$ \texttt{TRUE}")
    LambdaStateTrace(TextSize.Small, ((Or >> False) >> True), None)
    TextBlock(@"Let us begin to with \texttt{FALSE} $\vee$ \texttt{FALSE} $\rightarrow_\beta$ \texttt{FALSE}")
    LambdaStateTrace(TextSize.Small, ((Or >> False) >> False), None)

    SubSection("Remaining numeral derivations")
    TextBlock(@"Let us try out \texttt{0 = 0} $\rightarrow_\beta$ \texttt{TRUE}")
    LambdaStateTrace(TextSize.Small, IsZero >> !!"0", None)
  ]

