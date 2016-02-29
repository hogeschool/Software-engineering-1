import Data.Char

velociraptor :: Double -> Integer -> Double
velociraptor openingTime 0 = 0.0
velociraptor openingTime doors = openingTime + (velociraptor (openingTime / 2.0) (doors - 1))

validCourse :: [(Integer,Integer,Integer)] -> [(Integer,Integer)] -> Bool
validCourse galaxy course =
  let 
    findSectorDamage ((gx,gy,damage):sectors) sector@(sx,sy) =
      if gx == sx && gy == sy then damage else findSectorDamage sectors sector
    checkCourse galaxy [] takenDamage = True
    checkCourse galaxy (sector:sectors) takenDamage =
      let
        damage = (findSectorDamage galaxy sector) + takenDamage
      in
        if damage >= 100 then False else checkCourse galaxy sectors damage
  in 
    checkCourse galaxy course 0

(|>) x f = f x    
(>>>) :: Char -> Integer -> Char
(>>>) c shift = 
  let
    n = fromIntegral shift
  in
    ((((((ord c) - 97 + n) `mod` 26) + 26) `mod` 26) + 97) |> chr

encodeCaesar :: [Char] -> Integer -> [Char]
encodeCaesar [] n = []
encodeCaesar (c:cs) n =
  if ord c >= 97 && ord c <= 122 then
    (c >>> n):(encodeCaesar cs n)
  else
    c:(encodeCaesar cs n)
    
encodeVigenere :: [Char] -> [Char] -> Bool -> [Char]
encodeVigenere [] _ _ = []
encodeVigenere (c:cs) (p:ps) encode = do
  if ord c >= 97 && ord c <= 122 then
    let 
      n =
        let 
          position = ((ord p) - 97) `mod` 26
        in
          if encode then position |> fromIntegral else (- position) |> fromIntegral
    in
      (c >>> n):(encodeVigenere cs (ps ++ [p]) encode)
  else
    c:(encodeVigenere cs (ps ++ [p]) encode)

splitSpaces :: [Char] -> [[Char]]    
splitSpaces text =
  let
    splitCharList [] split acc = split ++ [acc |> reverse]
    splitCharList (' ':cs) split acc = splitCharList cs (split ++ [acc |> reverse]) []
    splitCharList (c:cs) split acc = splitCharList cs split (c:acc)
  in
    splitCharList text [] []
    
splitBy text separator =
  let
    splitCharList [] split acc separator = split ++ [acc |> reverse]
    splitCharList (c:cs) split acc separator 
      | c == separator = splitCharList cs (split ++ [acc |> reverse]) [] separator
      | otherwise = splitCharList cs split (c:acc) separator
  in
    splitCharList text [] [] separator
    
textCorrection text =
  let
    splitText = splitBy text ' '
    replace [] = []
    replace (word:words) =
      if word == "besides" then
        "furthermore":(replace words)
      else
        word:(replace words)
    combineText [] = ""
    combineText (word:words) =
      word ++ " " ++ (combineText words)
  in
    replace splitText |> combineText
    
textCorrectionGeneric text word replacer =
  let
    splitText = splitBy text ' '
    replace [] word replacer = []
    replace (w:words) word replacer =
      if w == word then
        replacer:(replace words word replacer)
      else
        w:(replace words word replacer)
    combineText [] = ""
    combineText (word:words) =
      word ++ " " ++ (combineText words)
  in
    replace splitText word replacer |> combineText

main = do
  -- exercise 1
  putStr("Velociraptor\n")
  print (velociraptor 30.0 10)
  
  -- exercise 2
  putStr("\nCourse\n")
  let milkyWay = [(0,0,0),(0,1,10),(0,2,0),(1,0,50),(1,1,0),(1,2,0),(2,0,10),(2,1,0),(2,2,70)]
  let course = [(0,0),(1,0),(2,0),(2,2)]
  print (validCourse milkyWay course)
  
  -- exercise 3
  putStr("\nCaesar's cipher\n")
  let text = "Arma virumque cano, Troiae qui primus ab oris Italiam, fato profugus, Laviniaque venit litora."
  let encodedText = encodeCaesar text 5
  let decodedText = encodeCaesar encodedText (- 5)
  print (encodedText)
  print(decodedText)
  
  -- exercise 4
  putStr("\nVigenere's cipher\n")
  let encodedTextV = encodeVigenere text "cat" True
  let decodedTextV = encodeVigenere encodedTextV "cat" False
  print(encodedTextV)
  print(decodedTextV)
  
  -- exercise 5
  putStr("\nSplit with spaces\n")
  print (splitSpaces "Hello World!")
  
  -- exercise 6
  putStr("\nSplit by\n")
  print (splitBy "Hello World!" 'o')
  
  -- exercise 7
  putStr("\nText correction with 'besides'\n")
  print (textCorrection "we will show that functional programming languages are the best. besides we will prove that f# is the finest among all functional languages.")
  print (textCorrection "yo dawg, i heard you like replacing words, so i put a besides in a besides so you can replace while you replace")
  
  -- exercise 8
  putStr("\nText correction generic\n")
  print (textCorrectionGeneric 
          "we will show that functional programming languages are the best. besides we will prove that f# is the finest among all functional languages."
          "functional"
          "imperative")