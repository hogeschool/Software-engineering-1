module FunctionalWeek5

open CommonLatex
open LatexDefinition
open CodeDefinitionLambda
open Interpreter

let slides =
  [
    Section("Introduction")
    SubSection("Lecture topics")
    ItemsBlock
      [
        ! @"The typed lambda calculus"
        ! @"Type checking"
        ! @"Type inference"
        ! @"F\# basic types"
        ! @"F\# custom advanced types"
      ]

    Section( @"The typed lambda calculus")
    SubSection("Idea")
    TextBlock @"We can add types to the lambda calculus!"

    TextBlock @"This makes the lambda calculus safer to compose, as we run no more the risk of passing a parameter of the wrong shape (like an integer where a function is expected)"

    SubSection("Shapes of types")
    VerticalStack [
        TextBlock @"Types in the lambda calculus follow the same basic syntactic shape of the lambda calculus itself."
        
        TextBlock @"Consider any type $\tau$; it will be made up, recursively, out of any of the following shapes:"

        ItemsBlock
          [
            ! @"a type variable (like generic type parameters, such as \texttt{T}): $\alpha$"
            ! @"a generic type definition (like the definition of \texttt{List<T>}: $\forall \alpha \Rightarrow \tau$"
            ! @"a function type: $\tau \rightarrow \tau$"
            ! @"a type application (like \texttt{List<Customer>}): $\tau \tau$"
          ]
      ]

    SubSection("Shapes of programs")
    VerticalStack [
        TextBlock @"Lambda calculus programs are now extended to include type declarations:"
        
        TextBlock @"Consider any term \texttt{t}; it will be made up, recursively, out of any of the following shapes:"

        ItemsBlock
          [
            ! @"a free variable: \texttt{x}"
            ! @"a lambda definition: \texttt{$\lambda$x$\rightarrow$t}"
            ! @"a generic parameter introduction: \texttt{$\Lambda \alpha \Rightarrow$t}"
            ! @"a value application: \texttt{t t}"
            ! @"a type application: \texttt{t $\tau$}"
          ]
      ]

    SubSection("Typed booleans")
    VerticalStack
      [
        TextBlock @"True and false now become generic with respect to the inputs \texttt{t} and \texttt{f}:"

        LambdaCodeBlock(TextSize.Small, (deltaRules True).Value, true)
        LambdaCodeBlock(TextSize.Small, (deltaRules False).Value, true)
      ]
    
    SubSection("Typed naturals")
    VerticalStack
      [
        TextBlock @"Numbers now become generic with respect to the data manipulated through \texttt{z} and \texttt{s}:"

        LambdaCodeBlock(TextSize.Small, (deltaRules !!"0").Value, true)
        LambdaCodeBlock(TextSize.Small, (deltaRules !!"2").Value, true)
      ]

    SubSection("Typed pairs")
    ItemsBlock[
        ! @"A pair takes as input the two values, \texttt{x} and \texttt{y}"
        ! @"It then expects a function, \texttt{g}, which will consume \texttt{x} and \texttt{y} and produce some final result"
        ! @"\texttt{x} and \texttt{y} have generic types, $\alpha$ and $\beta$"
        ! @"\texttt{g} also has generic type, it takes as input an $\alpha$ and a $\beta$, and returns a $\gamma$"
      ]

    VerticalStack
      [
        TextBlock @"Pairs become generic with respect to the types of the input, but also the generated output:"

        LambdaCodeBlock(TextSize.Small, (deltaRules MakePair).Value, true)
      ]

    VerticalStack
      [
        TextBlock @"Pairs become generic with respect to the types of the input, but also the generated output:"

        LambdaCodeBlock(TextSize.Small, (deltaRules First).Value, true)
        LambdaCodeBlock(TextSize.Small, (deltaRules Second).Value, true)
      ]
      
    SubSection("Typed union")
    ItemsBlock[
        ! @"A union is built from one of two possible values, \texttt{x} and \texttt{y}"
        ! @"It then expects two functions, \texttt{f} and \texttt{g}, which will consume \texttt{x} or \texttt{y} and produce some final result"
        ! @"\texttt{x} and \texttt{y} have generic types, $\alpha$ and $\beta$"
        ! @"\texttt{f} and \texttt{g} also have generic types, they takes as input an $\alpha$ or a $\beta$, but both return a $\gamma$"
      ]

    VerticalStack
      [
        TextBlock @"Unions become generic with respect to the types of the inputs, but also the generated output:"

        LambdaCodeBlock(TextSize.Small, (deltaRules Inl).Value, true)
        LambdaCodeBlock(TextSize.Small, (deltaRules Inr).Value, true)
      ]

    VerticalStack
      [
        TextBlock @"Unions become generic with respect to the types of the inputs, but also the generated output:"

        LambdaCodeBlock(TextSize.Small, (deltaRules Match).Value, true)
      ]

    Section(@"Type checking")
    SubSection("Is a term well-typed")
    VerticalStack
      [
        TextBlock @"Thanks to types, we can now verify that terms are well-composed: this can prevent various forms of bugs"

        ItemsBlock[
            ! @"Passing an argument to a variable that is not a function"
            ! @"Adding an integer and a list"
            ! @"Performing the logical or between lists and trees"
            ! @"Performing the logical and between functions"
            ! @"..."
          ]
      ]

    VerticalStack
      [
        ItemsBlock[
          ! @"This verification is called type-checking"
          ! @"We perform a fake execution of a term, making sure that all function applications are matching"
          ! @"As we go, we replace parts of the term with its type"
          ! @"When the whole term has been replaced with its type, we know the return type of the original program"
          ! @"If the process gets stuck somewhere, we stop and give back an error\footnote{usually a compiler error}"
        ]
      ]

    TextBlock @"Type checking uses a series of typing rules that look a bit like execution rules."

    VerticalStack
      [
        TextBlock @"When we encounter a variable, we simply return its type from $\Gamma$, which contains all variable declarations found so far"
                
        TypingRules
          [
            {     
              Premises = [ @"x:\sigma \in \Gamma" ]
              Conclusion = @"\Gamma \vdash x : \sigma"
            }
          ]
      ]
    
    VerticalStack
      [
        TextBlock @"When we encounter a function declaration, we simply add the type of the parameter to $\Gamma$, and type check the function body"
                
        TypingRules
          [
            {     
              Premises = [ @"\Gamma, x:\sigma \vdash t : \tau" ]
              Conclusion = @"\Gamma \vdash (\lambda x:\sigma \rightarrow t) : (\sigma \rightarrow \tau)"
            }
          ]
      ]

    VerticalStack
      [
        TextBlock @"When we encounter a function application, we simply make sure that the input of the function has the same type as the parameter"
                
        TypingRules
          [
            {     
              Premises = [ @"\Gamma \vdash t : \sigma \rightarrow \tau\ \ \ \ \Gamma \vdash u : \sigma" ]
              Conclusion = @"\Gamma \vdash (t\ u) : \tau"
            }
          ]
      ]

    TextBlock @"Thanks to these simple rules, which fully define how the type checker of a compiler works, we can find the types of a series of terms."

    TextBlock @"Let's begin with the type of \texttt{True}:"
    LambdaTypeTrace(TextSize.Small, (deltaRules True).Value)

    TextBlock @"Let's then see the type of boolean \texttt{And}:"
    LambdaTypeTrace(TextSize.Small, (deltaRules And).Value)

    TextBlock @"Similarly we can type check numbers:"
    LambdaTypeTrace(TextSize.Small, (deltaRules !!"0").Value)

    Section(@"Type inference")
    SubSection("Are type annotations necessary?")
    ItemsBlock[
        ! @"Type annotations are not strictly necessary"
        ! @"This means that, even if there are types, we might not need to write them all down"
        ! @"Modern functional programming languages, like F\#, Haskell, and Scala, are capable of guessing the types autonomously"
        ! @"This is called type inference"
      ]

    SubSection("Are type annotations pointless?")
    ItemsBlock[
        ! @"Type annotations are still useful at times"
        ! @"This means that, even if there is type inference, sometimes we write the types explicitly"
        ! @"Type inference might sometimes fail, so type annotations might solve the issue"
        ! @"Type annotations act as documentation, and make complex code clearer"
      ]

    SubSection("Type annotations in practice")
    TextBlock @"In practice you will see some type annotations, especially on global symbols and functions, and for smaller local values and anonymous functions we let the type inference do its job."

    Section(@"F\# basic types")
    SubSection("The essential types of F\#")
    VerticalStack [
        TextBlock @"Types in F\# are exactly the same as those in the lambda calculus:"
        
        ItemsBlock
          [
            ! @"the empty type \texttt{Unit}, with its only value \texttt{()}"
            ! @"primitive machine types, such as \texttt{int}, \texttt{bool}, \texttt{string}, etc."
            ! @"type variable, which instead of greek letters such as $\alpha$ have an apostrophe as prefix: \texttt{'a}"
            ! @"function types: \texttt{t $\rightarrow$ t}"
            ! @"tuples, in the form: \texttt{'a * 'b}"
            ! @"discriminated unions, in the form: \texttt{Choice<'a,'b>}"
          ]
      ]

    SubSection("F\# type declarations")
    VerticalStack [
        TextBlock @"Type declarations in F\# are the same as those in the lambda calculus."
        TextBlock @"An identifier, a colon, and a type:"

        FSharpCodeBlock(TextSize.Small, (("x",Nat) ==> ((Plus >> !!"x") >> !!"1")), true)
      ]

    VerticalStack [
        TextBlock @"The same applies to \texttt{let} bindings:"

        FSharpCodeBlock(TextSize.Small, Let(("x",Nat), !!"0", ((Plus >> !!"x") >> !!"1")), true)
      ]

    VerticalStack [
        TextBlock @"Pairs (and tuples of size greater than two\footnote{Those can be matched, or extracted with \texttt{let (x,y,z,...) = t}}) are similarly handled:"

        FSharpCodeBlock(TextSize.Small, (("p",(Type.Product >>> Nat) >>> Boolean) ==> ((Plus >> (First >> !!"p")) >> !!"1")), true)
      ]

    VerticalStack [
        TextBlock @"Discriminated unions are also no surprise:"

        FSharpCodeBlock(TextSize.Small, (("p",(Type.Sum >>> Nat) >>> Boolean) ==> (((Match >> !!"p") >> (("x",Nat) ==> ((Plus >> !!"x") >> !!"1"))) >> (("y",Boolean) ==> !!"0"))), true)
      ]

    VerticalStack [
        TextBlock @"Of course nothing stops us from using function types:"

        FSharpCodeBlock(TextSize.Small, Let(("f",(Nat --> Nat)), (("x",Nat) ==> ((Plus >> !!"x") >> !!"1")), !!"f" >> !!"2"), true)
      ]

    TextBlock @"We can mix and match these types as much as we want: functions from tuples into function from discriminated unions to tuples of functions to ..."

    SubSection @"Demo"
    TextBlock @"A short demo might be in order"

    Section @"Named types"
    SubSection @"Giving names to types"
    VerticalStack [
        TextBlock @"We can use the \texttt{type} keyword to give a name to a type, for ease of later use:"

        FSharpCodeBlock(TextSize.Small, TypeLet("MyInt", Nat, Let(("f",(!!!!"MyInt" --> !!!!"MyInt")), (("x",!!!!"MyInt") ==> ((Plus >> !!"x") >> !!"1")), !!"f" >> !!"2")), true)
      ]

    VerticalStack [
        TextBlock @"This makes the most sense when the name of the type is a tad longer\footnote{Remember that a type might contain as many arrows, tuples, etc. as we might want!}:"

        FSharpCodeBlock(TextSize.Small, TypeLet("Int2", (Nat --> Nat), Let(("f",!!!!"Int2"), (("x",Nat) ==> ((Plus >> !!"x") >> !!"1")), !!"f" >> !!"2")), true)
      ]

    SubSection @"Demo"
    TextBlock @"Another short demo might be in order"

    SubSection @"Custom named types"

    ItemsBlock [
        ! @"Sometimes existing type definitions are not clear or readable enough."
        ! @"For example, it is hard to guess that a long tuple like \texttt{string * string * int * int} actually represents a person"
        ! @"Similarly, it is hard to guess that a long union like \texttt{Choice<Choice<int,int>, Choice<int, int>>} actually represents card and suit (hearts, diamonds, clubs, spades)"
      ]

    ItemsBlock [
        ! @"The \texttt{type} keyword can define brand new types."
        ! @"These new types can have additional structure and internal names."
        ! @"The new types are substantially identical to (nested) tuples and (nested) unions."
        ! @"The only difference is that the elements of the tuple and the cases of the union can be augmented with a mnemonic name."
      ]

    SubSection(@"Records")
    ItemsBlock [
        ! @"Tuples with named elements are called \textbf{records}."
        ! @"They are declared between curly brackets."
        ! @"Each field of the record has a name, followed by a colon and the type of the field."
      ]

    VerticalStack [
        TextBlock "An example record declaration for a person could look like:"

        FSharpCodeBlock(TextSize.Small, TypeLet("Person", Record([("Name",String); ("Surname",String)]), Unit), true)
      ]

    VerticalStack [
        TextBlock @"We can initialise a record with the values of the fields between curly brackets, each bound to the proper field name"

        FSharpCodeBlock(TextSize.Small, 
          TypeLet("Person", Record([("Name",String); ("Surname",String)]), 
            Let(("p",!!!!"Person"),MakeRecord([("Name",StringConst"Haskell"); ("Surname",StringConst"Curry")]),Unit)), true)
      ]

    VerticalStack [
        TextBlock @"We can access a record fields with the usual dot notation coming from C-style languages:"

        FSharpCodeBlock(TextSize.Small, 
          TypeLet("Person", Record([("Name",String); ("Surname",String)]), 
            Let(("p",!!!!"Person"),MakeRecord([("Name",StringConst"Haskell"); ("Surname",StringConst"Curry")]),Dot(!!"p", "Name"))), true)
      ]

    VerticalStack [
        TextBlock @"We can modify large parts of a record fields with the \texttt{with} operator, which copies over all fields besides those explicitly specified:"

        FSharpCodeBlock(TextSize.Small, 
          TypeLet("Person", Record([("Name",String); ("Surname",String)]), 
            Let(("p",!!!!"Person"),MakeRecord([("Name",StringConst"Haskell"); ("Surname",StringConst"Curry")]),
              RecordWith(!!"p", [("Name",StringConst"F\#")]))), true)
      ]

    SubSection @"Demo"
    TextBlock @"Yet another short demo might be in order"

    SubSection(@"Records")
    ItemsBlock [
        ! @"Unions with named cases are called \textbf{discriminated unions}."
        ! @"They are declared as a series of constructors, each with its own parameters."
        ! @"Matching is the only way to access the data inside the instance of the discriminated union."
      ]

    VerticalStack [
        TextBlock "An example record declaration for a widget size could look like:"

        FSharpCodeBlock(TextSize.Small, TypeLet("Size", Union([("Small",[]); ("Large",[]); ("Custom",[Nat])]), Unit), true)
      ]

    VerticalStack [
        TextBlock @"We can initialise a union with the values of a constructor between brackets, of no argument at all for parameterless constructors"

        FSharpCodeBlock(TextSize.Small, 
          TypeLet("Size", Union([("Small",[]); ("Large",[]); ("Custom",[Nat])]),
            Let(("s1",!!!!"Size"), !!"Small" ,
              Let(("s2",!!!!"Size"), !!"Custom" >> !!"10",Unit))), true)
      ]

    VerticalStack [
        TextBlock @"We can access a union values with an extended pattern matching syntax:"

        FSharpCodeBlock(TextSize.Small, 
          TypeLet("Size", Union([("Small",[]); ("Large",[]); ("Custom",[Nat])]),
            Let(("s1",!!!!"Size"), !!"Small",
              Let(("w",Nat), MatchCustom(!!"s1", [("Small", [], !!"0"); ("Large", [], !!"10"); ("Custom", ["c"], !!"c")]), Unit))), true)
      ]

    SubSection @"Demo"
    TextBlock @"Yet another short demo might be in order"

    SubSection "Lists"
    VerticalStack [
        TextBlock @"A very important data structure in functional languages is the list."
        TextBlock @"In F\# it is predefined\footnote{So you do not need to define it yourself!} as union of the empty list and the non-empty constructor:"

        FSharpCodeBlock(TextSize.Small, 
          TypeLet("List<'a>", Union([("[]",[]); ("(::)",[!!!"a"; !!!!"List<'a>"])]),
            Let(("l1",!!!!"List<int>"), !!"[]", 
              Let(("l2",!!!!"List<int>"), (!!"::" >> !!"1") >> ((!!"::" >> !!"2") >> ((!!"::" >> !!"3") >> !!"[]")), Unit))), true)
      ]

    VerticalStack [
        TextBlock @"We could recursively perform all sorts of operations on lists with simple matching."
        TextBlock @"Fortunately, we can make use of many of the existing available functions on lists:"

        ItemsBlock
          [
            ! @"\texttt{map}, which transforms all elements of a list according to a specific transformation function"
            ! @"\texttt{filter}, which removes some elements of a list according to a specific predicate function"
            ! @"\texttt{reduce}, which collapses a list onto a single value according to a specific aggregation function"
            ! @"\texttt{fold}, which collapses a list onto a single value according to a specific aggregation function and a starting aggregate value"
          ]
      ]

    SubSection @"Demo"
    TextBlock @"Yet another short demo might be in order"

    VerticalStack [
      TextBlock @"Lists are so important that they even get their own special syntax, called \textbf{list comprehensions}, to simplify their creation and manipulation."

      Pause
      TextBlock @"List comprehensions are substantially a way to map, filter, and cross lists easily."
      TextBlock @"Fold is excluded, and must be used for aggregate operations."
    ]

    SubSection @"Demo"
    TextBlock @"Yet another short demo might be in order"

    Section "Conclusion"
    SubSection(@"Recap")
    ItemsBlock[
        ! @"The lambda calculus can be translated, type to type, into F\#"
        ! @"F\# then adds handy primitive types, records, lists, discriminated unions, etc. to make it easier to build actual programs in practice"
      ]

    Section "Appendix"
    SubSection("Type checking of false")
    LambdaTypeTrace(TextSize.Small, (deltaRules False).Value)

    SubSection("Type checking of boolean or")
    LambdaTypeTrace(TextSize.Small, (deltaRules Or).Value)

    SubSection("Type checking of if-then-else")
    LambdaTypeTrace(TextSize.Small, (deltaRules If).Value)

    SubSection("Type checking of a number")
    LambdaTypeTrace(TextSize.Small, (deltaRules !!"3").Value)

    SubSection("Type checking of a number addition")
    LambdaTypeTrace(TextSize.Small, (deltaRules Plus).Value)

    SubSection("Type checking of pairs' second selector")
    LambdaTypeTrace(TextSize.Small, (deltaRules Second).Value)

    SubSection("Type checking of inr")
    LambdaTypeTrace(TextSize.Small, (deltaRules Inr).Value)

  ]
