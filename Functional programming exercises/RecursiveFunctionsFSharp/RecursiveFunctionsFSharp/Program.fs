//Note: To convert a string to list of characters and from list of characters to string use the following functions:
//let toCharList (s: string) : char list = s.ToCharArray() |> List.ofArray
//let toString (cl : char list) : string = List.fold (fun s c -> s + (string c)) "" cl

//Velociraptors were very intelligent animals and, according to extremely reliable scientific sources, they were able to communicate verbally and
//to perform simple human-like tasks, such as opening doors. At this purpose, since as computer scientists we fear uncertainty, we want to predict how much time we would
//survive if we are barricaded in a room of a building with a velociraptor roaming around. Opening a door initially takes T seconds for a velociraptor. Every time it opens a new door
//the required time is halved. Write a function that estimates your survival time if your are stuck in a room (you cannot change room) and the velociraptor is D doors
//far from you. The function takes as input the initial time T required to open a door and the number of doors to open and returns the seconds you have left to leave. Remember: velociraptors 
//do not wear out and they do not know fear.

//In the 40th millenium the fleets of the Emperor of mankind will travel across the galaxy to cleanse the Imperium from aliens and heretics threatening the humans. Reaching a point in the galaxy
//requires a ship to travel at warp speed. Unfortunately, if the endpoint of the course is affected by a warp storm, the ship will be damaged. Although expendable, the Imperium prefers not to throw 
//away the lives of guardsmen so each captain must know if the course he plotted is valid. At this purpose we model the galaxy as a list of sectors. Each sector is defined by a triple containg the X 
//and Y coordinates (integer) and the amount of damage the ship takes when jumping there (example: (0,5,30) means that if a ship jumps to sector with X = 0 and Y = 5 it takes 30 damage). 
//You can think about this as a matrix orginized by rows. Each sector contains the amount of damage the ship takes when jumping there. 
//If the value is positive than it means there is a warp storm. A ship is destroyed when the damage reaches 100 or more.
//A course takes as input a list of pairs containing the coordinates of each course point. A course is valid if the ship can cross all the sectors in the course without being destroyed. Write a function
//takes as input a galaxy, a ship course, and checks if the course is valid.

//Caesar's chipher is the first cryptographic algorithm documented in history. Given a text and an integer n called key, you shift ahead every letter of the text by n positions. If you reach the end of 
//the alphabet you go back from the beginning. For example the letter 'c' encoded with key = 3 becomes 'f', the letter 'y' encoded with key = 5 becomes the letter 'd'. Write the functions to encode
//and decode a given text with this algorithm. Decoding a text encoded with Caesar's chypher only requires to apply the encoding by shifting BACK of the same amount the letters.
//For example if you encoded the text with key = 5 then decoding just requires to call the same function with key = -5 and the encoded text as input. 
//For simplicity only encode lower case letters, but if you want you can also make a version which encodes capital letters.
//Punctuations and special characters are never encoded.

//Vigenere's cipher is an improvement of Caesar's chypher. The idea is to use variable shifting for each character. This chypher uses a password instead of a single key.
//The password is used to create an encryption table by placing it under the text and repeating it until the text ends. For example to encode the text "hello world!" encoded with the password "cat" we build the following
//encryption table:
//text:       hello world!
//password:   catcatcatcat
//Each character in the text is shifted by the position in the alphabet of the corresponding letter of the password in the encryption table. In the example above h is shifted by 2 positions becoming a j.
//e stays the same because a has position 0 in the alphabet, and l becomes e because it is shifted by 19 positions. The decoding is done by shifting back with the password. Write a function that econdes
//and decodes a text using the Vigenere's cipher.


//Given a string as input write a function that splits it into substrings by splitting it at every whitespace. For example
//splitSpaces "Hello World!" would return ["Hello"; "World!"]. 

//Generalize the previous function to accept as input a specific character separator for example
//splitBy "Hello World!" 'o' would return ["Hell"; " w"; "rld!"]

//David is trying to improve his writing skills but he often does the mistake of using the word "besides"
//instead of "furthermore". Since this problem is impacting his career, he is asking you to make a text checker
//that replaces every occurence of the word "Besides" with "However". Implement a function which takes as input
//a text in the format of a string and returns a text where every word besides is replaced by however. Remember that
//words are always separated by whitespaces. Assume that the letters are never capitalized and there are no punctuation characters. 
//Generalize the previous function to accept as input the word to be replaced and the word to replace.

let toCharList (s: string) : char list = s.ToCharArray() |> List.ofArray
let toString (cl : char list) : string = List.fold (fun s c -> s + (string c)) "" cl

let rec velociraptor (openingTime : float) (doors : int) : float =
  match doors with
  | 0 -> 0.0
  | _ -> openingTime + (velociraptor (openingTime / 2.0) (doors - 1))

let validCourse (galaxy : (int * int * int) list) (course : (int * int) list) : bool =
  let rec findSectorDamage galaxy sector =
    let sx,sy = sector
    match galaxy with
    | [] -> failwith "Not existing sector"
    | (gx,gy,damage) :: sectors ->
        if gx = sx && gy = sy then damage else findSectorDamage sectors sector
  let rec checkCourse galaxy course takenDamage =
    match course with
    | [] -> true
    | sector :: sectors ->
        let damage = (findSectorDamage galaxy sector) + takenDamage
        if damage >= 100 then false else checkCourse galaxy sectors damage
  checkCourse galaxy course 0
     


let splitSpaces text =
  let rec splitCharList text split acc =
    match text with
    | [] -> split @ [acc |> List.rev |> toString]
    | c :: cs when c = ' ' ->
        let acc = acc |> List.rev
        (splitCharList cs (split @ [acc |> toString]) []) 
    | c :: cs ->
        splitCharList cs split (c :: acc)
  splitCharList (toCharList text) [] []

let splitBy text separator =
  let rec splitCharList text split acc separator =
    match text with
    | [] -> split @ [acc |> List.rev |> toString]
    | c :: cs when c = separator ->
        let acc = acc |> List.rev
        splitCharList cs (split @ [acc |> toString]) [] separator
    | c :: cs ->
        splitCharList cs split (c :: acc) separator
  splitCharList (toCharList text) [] [] separator

let textCorrection text =
  let splitText = (splitBy text ' ')
  let rec replace text =
    match text with
    | [] -> []
    | word :: words ->
        if word = "besides" then 
          "furthermore" :: (replace words) 
        else 
          word :: (replace words)
  let rec combineText text =
    match text with
    | [] -> ""
    | word :: words -> 
        word + " " + (combineText words)
  replace splitText |> combineText

let textCorrectionGeneric text word replacer =
  let splitText = (splitBy text ' ')
  let rec replace text word replacer =
    match text with
    | [] -> []
    | w :: words ->
        if w = word then 
          replacer :: (replace words word replacer) 
        else 
          w :: (replace words word replacer)
  let rec combineText text =
    match text with
    | [] -> ""
    | word :: words -> 
        word + " " + (combineText words)
  replace splitText word replacer |> combineText

let (>>) (c : char) (n : int) : char = ((((((int c) - 97 + n) % 26) + 26) % 26) + 97) |> char

let rec encodeCaesar text n =
  match text with
  | [] -> []
  | c :: cs ->
      if (int c >= 97 && int c <= 122) then
        (c >> n) :: (encodeCaesar cs n)
      else
        c :: (encodeCaesar cs n)

let rec encodeVigenere text password encode =
  match text, password with
  | [], _ -> []
  | c :: cs, p :: ps ->
      if (int c >= 97 && int c <= 122) then
        let n =
          let position = ((int p) - 97) % 26
          if encode then position else -position
        (c >> n) :: (encodeVigenere cs (ps @ [p]) encode)
      else
        c :: (encodeVigenere cs (ps @ [p]) encode)
  | _ -> failwith "uh?!"


[<EntryPoint>]
let main argv =
  let milkyWay =
    [
      (0,0,0)
      (0,1,10)
      (0,2,0)
      (1,0,50)
      (1,1,0)
      (1,2,0)
      (2,0,10)
      (2,1,0)
      (2,2,70)
    ]
  let course =
    [
      (0,0)
      (0,1)
      (2,0)
      (1,0)
    ]
  let text = "Arma virumque cano, Troiae qui primus ab oris Italiam, fato profugus, Laviniaque venit litora." |> toCharList
  let text1 = "hello world!" |> toCharList
  let password = "cat" |> toCharList
  let encodedText = encodeVigenere text password true |> toString
  let decodedText = encodeVigenere (encodedText |> toCharList) password false |> toString
  printfn "%A \n%A" encodedText decodedText
  0 // return an integer exit code
