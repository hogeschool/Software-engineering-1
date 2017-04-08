

let fizzbuzz (n : int) (m : int) =
  if (n % 4 = 0) && (m % 4 = 0) then
    "FizzBuzz"
  elif n % 4 = 0 then
    "Fizz"
  elif m % 4 = 0 then
    "Buzz"
  else
    ""

let rec zip (list1 : 'a list) (list2 : 'b list) : (('a * 'b) list)=
  match list1,list2 with
  | [],[] -> []
  | _,[]
  | [],_ -> failwith "One of the list is longer than the other"
  | x :: xs,y :: ys -> (x,y) :: (zip xs ys)

let unzip (l : List<'a * 'b>) : (List<'a>) * (List<'b>) =
  let inv1,inv2 =
    l |>
    List.fold (fun (left,right) (x,y) -> x :: left,y :: right) ([],[])
  List.rev inv1,List.rev inv2

let unzipSlow (l : List<'a * 'b>) : (List<'a>) * (List<'b>) =
  l |>
  List.fold (fun (left,right) (x,y) -> left @ [x],right @ [y]) ([],[])
  
let randomList (r : System.Random) =
  let maxLength = r.Next(10000)
  [for x in [1.. maxLength] do
          yield r.Next(100),r.Next(100)]

[<EntryPoint>]
let main argv =
  let r = System.Random()
  let list = randomList r
  let fast = unzip list
  let slow = unzipSlow list
  printfn "%A" (unzip [(1,4);(2,5);(3,6)])
  0
