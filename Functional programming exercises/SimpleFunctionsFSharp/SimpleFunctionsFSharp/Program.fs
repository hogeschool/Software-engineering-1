//1. Define a function that takes two integers n, m and another integer x and do the following: if the product of n and x is m return the string
//"Tro", if the product of m and x is n return "lolo", and if the product of n and m is x return "Trololo". In all other cases just return an emtpy string.

//2. Define a new version of exercise 1 where the user can specify the messages to output.

//3. The crazy operation takes two one-digit integer numbers n and m and does the following: if n divides m then just return an integer number containing the digits nm.
//For example crazy 4 8 returns 48. If n is positive and m negative return their product. In all the other cases return the sum of the two numbers. Implement this
//extremely useful operation.

//4. A year is a "leap year" if it is divisible by 4, with the exception of century years (those divisible by 100) that are not divisible by 400. Write a function that
//takes as input a year and returns true if it is a leap year.

//5. The prisoner's dilemma is a historical game analyzed in game theory: two prisoners are asked to confess their crimes. Depending on their answers they must serve in
// prison in the following ways:
// a) If both betray each other they both serve 2 years.
// b) If one of the two confesses and the other stays silent, the one who confessed will be set free and the other will serve 3 years.
// c) If both stay stays silent, both of them will serve 1 year in prison.
// Write a function that takes as input whether each prisoner confessed or not, and output the years to serve for one of the prisoners depending on the answer of the other one.

//6. An obsessive compulsive person is a mentally sick person who does some routine rituals when some specific situation presents itself. Bob has an obsessive compulsive desease
// which makes him react in the following ways:
// a) When he is going to the toilet he does the following: if the toilet seat is up then he puts it down and reverse the toilet paper roll. Otherwise he lifts up the toilet seat
// but he does NOT reverse the toilet paper roll.
// b) When he is walking on the street, if the pavement has tiles then he jumps to avoid tile intersections, if the pavement is grey but without tiles he walks backward, otherwise he
// walks normally.
// c) When he is going to the restaurant, if table 42 is empty he seats there, if it is busy he insults the waiter and leaves.
// Write a function that takes as input a string depicting a situation (for example "at toilet"), and a state (for example "toilet seat up") and outputs Bob's reaction as a string
// i.e. "insult waiter". If you input an invalid combination of situation-event then Bob becomes crazy and kills himself.


let exercise1 n m x =
  if n * x = m then
    "Tro"
  elif m * x = n then
    "lolo"
  elif n * m = x then
    "Trololo"
  else
    ""

let exercise2 n m x tro lolo trololo _default : string =
  if n * x = m then
    tro
  elif m * x = n then
    lolo
  elif n * m = x then
    trololo
  else
    _default

let exercise3 n m =
  if m % n = 0 then
    n * 10 + m
  elif n > 0 && m < 0 then
    n * m
  else
    n + m

let exercise4 year =
  if year % 4 = 0 then
    if year % 100 = 0 then
        not (year % 400 <> 0) 
    else
      true
  else
    false

let exercise5 answer1 answer2 =
  match answer1, answer2 with
  | true, true -> 2
  | true, false -> 0
  | false, true -> 3
  | false, false -> 1

let exercise6 situation event =
  match situation with
  | "at toilet" ->
      match event with
      | "toilet seat up" -> "put down toilet seat and reverse roll"
      | "toilet seat down" -> "lift up seat"
      | _ -> "get crazy and kill himself"
  | "walking" ->
      match event with
      | "pavement has tiles" -> "jump from tile to tile"
      | "grey pavement" -> "walk backward"
      | _ -> "walk normally"
  | "at restaurant" ->
      match event with
      | "table 42 empty" -> "seat down"
      | "table 42 busy"-> "insult the waiter and leave"
      | _ -> "get crazy and kill himself"
  | _ -> "get crazy and kill himself"    







[<EntryPoint>]
let main argv = 
    printfn "%A" (exercise6 "at toilet" "toilet seat down")
    0 // return an integer exit code
