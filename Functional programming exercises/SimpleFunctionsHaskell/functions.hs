troll_function n m x = 
  if n * x == m then 
    "Tro" 
  else if m * x == n then 
    "lolo"
  else if n * m == x then
    "Trololo"
  else
    ""
    
troll_function_mk2 n m x tro lolo trololo _default =
  if n * x == m then 
    tro
  else if m * x == n then 
    lolo
  else if n * m == x then
    trololo
  else
    _default
    
crazy n m =
  if m `mod` n == 0 then
    n * 10 + m
  else if n > 0 && m < 0 then
    n * m
  else
    n + m
    
leap_year year =
  if year `mod` 4 == 0 then
    if year `mod` 100 == 0 then
      not (year `mod` 400 /= 0)
    else
      True
  else
    False
    
prisoners_dilemma answer1 answer2 =
  case (answer1,answer2) of
    (True,True)     -> 2
    (True,False)    -> 0
    (False,True)    -> 3
    (False,False)   -> 1
    
ocd situation event =
  case situation of
    "at toilet" -> 
      case event of
        "toilet seat up"    -> "put down toilet seat and reverse roll"
        "toilet seat down" -> "lift up seat"
        _ -> "get crazy and kill himself"
    "walking" -> 
      case event of
        "pavement has tiles" -> "jump from tile to tile"
        "grey pavement" -> "walk backward"
        _ -> "walk normally"
    "at restaurant" ->
      case event of
        "table 42 empty" -> "seat down"
        "table 42 busy"-> "insult the waiter and leave"
        _ -> "get crazy and kill himself"
    _ -> "get crazy and kill himself"

main = do
  -- exercise 1
  putStr("Exercise 1")
  print (troll_function 8 24 3)
  print (troll_function 24 8 3)
  print (troll_function 2 3 6)
  print (troll_function 23 8 3)
  
  -- exercise 2
  putStr("\nExercise 2\n")
  print (troll_function_mk2 8 24 3 "Haters" "gonna hate" "Haters gonna hate" "You fail again")
  print (troll_function_mk2 24 8 3 "Haters" "gonna hate" "Haters gonna hate" "You fail again")
  print (troll_function_mk2 2 3 6 "Haters" "gonna hate" "Haters gonna hate" "You fail again")
  print (troll_function_mk2 23 8 3 "Haters" "gonna hate" "Haters gonna hate" "You fail again")
  
  -- exercise 3
  putStr("\nExercise 3\n")
  print (crazy 3 24)
  print (crazy 3 (- 5))
  print (crazy 8 11)
  
  -- exercise 4
  putStr("\nExercise 4\n")
  print (leap_year 2016)
  print (leap_year 1985)
  
  -- exercise 5
  putStr("\nExercise 5\n")
  print (prisoners_dilemma True False)
  
  -- exercise 6
  print (ocd "at toilet" "toilet seat up")
  print (ocd "at toilet" "toilet seat down")
  print (ocd "walking" "pavement has tiles")
  print (ocd "walking" "grey pavement")
  print (ocd "walking" "It's sunny")
  print (ocd "at restaurant" "table 42 empty")
  print (ocd "at restaurant" "table 42 busy")
  print (ocd "at toilet" "kill yourself")
  