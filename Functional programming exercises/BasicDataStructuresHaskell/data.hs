module Main where

import Data.Time.Clock
import Data.Time.LocalTime
import Data.Time.Format
import ScriptLanguage
import System.IO
import System.Console.ANSI

(|>) x f = f x

data GameState = GameState
  { x :: Integer  
  , y :: Double  
  , quit :: Bool  
  } deriving (Show)

data ScriptManager = ScriptManager
  { stateScripts :: [Script GameState]
  }
  
update :: ScriptManager -> Double -> GameState -> (GameState,ScriptManager)
update manager dt state =
  let
    (updatedScripts,result) = scheduler (stateScripts manager) [] state dt
  in
    (result,ScriptManager {stateScripts = updatedScripts})
  

gameLoop before state manager frameRate steps = do
  now <- getCurrentTime
  let dt = ((utctDayTime now) - (utctDayTime before)) |> realToFrac
  print(dt >= 1.0 / frameRate)
  if dt >= 1.0 / frameRate && steps > 0 then do
    let (updatedState,updatedManager) = update manager dt state
    putStr("Updating\n")
    print(updatedState)
    hFlush stdout
    gameLoop now updatedState updatedManager frameRate (steps - 1)
  else do
    gameLoop before state manager frameRate (steps - 1)
    
script1 :: (Script GameState)
script1 =
      Wait 0.5 >>> (
        Call (\state -> state {x = (x state) + 1}) >>> (
          Wait 2.0 >>> (
            Call(\state -> state {x = (x state) + 3}))))

main = do
  now <- getCurrentTime
  let state = GameState {x = 0,y = 0.0,quit = False}
  let scripts = ScriptManager {stateScripts = [script1]}
  gameLoop now state scripts 60.0 2000