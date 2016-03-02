module FunctionalWeek6

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
        ! @"Types definition"
        ! @"Initial state"
        ! @"Visualisation"
        ! @"Traversal and update"
      ]

    Section(@"Designing a functional application")
    SubSection("Idea")
    ItemsBlock
      [
        ! @"We begin by designing the types of our application"
        ! @"We give their first instance"
        ! @"We then move on to the fundamental visualisation aspects to achieve a minimum level of debuggability"
        ! @"We slowly add the dynamics of the program"
      ]

    Section(@"Defining the types")
    SubSection("From what?")
    TextBlock @"We start from a sketch of what the application should do, for example a picture on paper of an asteroid shooter game."

    VerticalStack [
      TextBlock @"The sketch of an asteroid shooter game contains:"

      ItemsBlock
        [
          ! @"The player ship"
          ! @"The number of lives left"
          ! @"The asteroids on screen"
        ]

      Pause

      TextBlock @"These become the fields of the \texttt{GameState} record."
      ]

    VerticalStack [
      TextBlock @"Unions come into play when things might have multiple shapes"

      ItemsBlock
        [
          ! @"The current player weapon, which is either a single or double gun"
          ! "..."
        ]
      ]

    Section(@"Demo")
    SubSection("Let's build the game state.")

    Section(@"First instance")
    SubSection("So it begins...")
    VerticalStack
      [
        TextBlock @"Having the types, we give the initial instance of the program state."
        TextBlock @"Usually this is a ``zero'' instance, with mostly safe values."
        Pause
        TextBlock @"The application still does nothing, but at least everything should compile and run."
      ]

    Section(@"Demo")
    SubSection("First instance of the game state.")

    Section(@"Visualisation")
    SubSection("Only believe what you see.")
    VerticalStack
      [
        TextBlock @"To be able to debug our application, we need to visualise the state."
        TextBlock @"We add a print/show routine to our data. If it is graphical it is better, but text is also fine."
      ]

    Section(@"Demo")
    SubSection("Visualisation in text and graphics.")

    Section(@"Traversal and update")
    SubSection("Make it move")
    VerticalStack
      [
        TextBlock @"We now traverse and update the game state."
        TextBlock @"At every frame/main iteration we will, in turn:"

        ItemsBlock
          [
            ! @"Read input events in a buffer"
            ! @"Go through every element of the game state"
            ! @"Compute its \textit{next value} (by also looking at the input)"
            ! @"Build the next state from all the \textit{next values}"
          ]
      ]

    SubSection("Main loop")
    TextBlock @"As many times as needed, we perform this update routine, visualise the state, and repeat."

    Section(@"Demo")
    SubSection("Traversal and update, and main loop.")

    Section(@"Try it out!")
    SubSection("Your turn.")
    TextBlock @"Use F\# or Haskell, the principles remain the same."
  ]
