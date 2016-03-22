module AsteroidGame

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

let r = new System.Random()

type List<'a> = 
  | Empty 
  | Node of 'a * List<'a>
let (<<) x xs = Node(x,xs)

let rec toFSharpList l =
  match l with
  | Empty -> []
  | Node(x,xs) -> x :: toFSharpList xs

let rec filter (p:'a->bool) (l:List<'a>) : List<'a> =
  match l with
  | Empty -> Empty
  | Node(x,xs) ->
    if p x then
      Node(x, filter p xs)
    else
      filter p xs

let rec map (f:'a->'b) (l:List<'a>) : List<'b> =
  match l with
  | Empty -> Empty
  | Node(x:'a,xs:List<'a>) -> 
    let y:'b = f x
    let ys:List<'b> = map f xs
    Node(y,ys)
 
let rec length (l:List<'a>) =
  match l with
  | Empty -> 0
  | Node(x,xs) -> 1 + length xs

type GunStatus = 
  | Cooldown of float32
  | Ready

type Projectile = 
  {
    Position : Vector2
  }

type Enemy = 
  {
    Position : Vector2
  }

type Ship = 
  {
    Position : Vector2
    Velocity : Vector2
  }

type GameState =
  {
    Gun         : GunStatus
    Enemies     : List<Enemy>
    Projectiles : List<Projectile>
    Ship        : Ship
  }

//type GamePhase = 
//  | StartMenu 
//  | Playing of GameState
//  | Dead of GameState

let initialState() = 
  {
    Gun         = GunStatus.Ready
    Enemies     = Empty
    Projectiles = Empty
    Ship = 
      {
        Position = Vector2(320.0f, 400.0f)
        Velocity = Vector2.Zero
      }
  }

let updateShip (ks:KeyboardState) (ms:MouseState) (dt:float32) (ship:Ship) =
  let speed = 1000.0f;
  let ship =
    if ks.IsKeyDown(Keys.Left) then
      { ship with Velocity = ship.Velocity - Vector2.UnitX * speed * dt }
    else
      ship
  let ship = 
    if ks.IsKeyDown(Keys.Right) then
      { ship with Velocity = ship.Velocity + Vector2.UnitX * speed * dt }
    else
      ship
  let ship =
    if ks.IsKeyDown(Keys.Down) then
      { ship with Velocity = ship.Velocity + Vector2.UnitY * speed * dt }
    else
      ship
  let ship = 
    if ks.IsKeyDown(Keys.Up) then
      { ship with Velocity = ship.Velocity - Vector2.UnitY * speed * dt }
    else
      ship
  { ship with Position = ship.Position + ship.Velocity * dt; 
              Velocity = ship.Velocity * 0.9f }

let randomEnemy() =
  {
    Enemy.Position = Vector2(float32(r.Next(0, 700)), 0.0f)
  }

let updateEnemy (dt:float32) (enemy:Enemy) : Enemy =
  {
    enemy with Position = enemy.Position + Vector2.UnitY * dt * 10.0f
  }

let updateEnemies (isNotHit:Enemy->bool) (dt:float32) (enemies:List<Enemy>) =
  let creationProbability() =
    let l = length enemies
    if l < 5 then
      100
    elif l < 15 then
      20
    else
      5
  let enemies = 
    if r.Next(0,100) < creationProbability() then
      randomEnemy() << enemies
    else
      enemies
  let enemies = map (updateEnemy dt) enemies
  let insideScreen (e:Enemy) : bool =
    e.Position.Y < 600.0f
  let enemies = filter insideScreen enemies
  filter isNotHit enemies

let updateProjectile (dt:float32) (projectile:Projectile) : Projectile =
  {
    projectile with Position = projectile.Position - Vector2.UnitY * dt * 100.0f
  }

let updateProjectiles (newProjectile:Unit->Projectile) (shouldCreateNow:Unit->bool) (dt:float32) (projectiles:List<Projectile>) =
  let projectiles = 
    if shouldCreateNow() then
      newProjectile() << projectiles
    else
      projectiles
  let projectiles = map (updateProjectile dt) projectiles
  let insideScreen (p:Projectile) : bool =
    p.Position.Y > -100.0f
  filter insideScreen projectiles

let updateState (ks:KeyboardState) (ms:MouseState) (dt:float32) (gameState:GameState) =
  let isShootingNow,newGun = 
    match gameState.Gun with
    | Ready ->
      if ks.IsKeyDown(Keys.Space) then
        (fun () -> true), Cooldown 0.2f
      else
        (fun () -> false), Ready
    | Cooldown t ->
      if t > 0.0f then
        (fun () -> false), Cooldown(t-dt)
      else
        (fun () -> false), Ready
  let createProjectileAtShip() =
    {
      Projectile.Position = gameState.Ship.Position
    }
  let rec isEnemyNotHit (projectiles:List<Projectile>) (e:Enemy) =
    match projectiles with
    | Empty -> true
    | Node(p,ps) ->
      if Vector2.Distance(p.Position, e.Position) < 120.0f then
        false
      else
        isEnemyNotHit ps e
  { 
    gameState with Ship         = updateShip ks ms dt gameState.Ship
                   Enemies      = updateEnemies (isEnemyNotHit gameState.Projectiles) dt gameState.Enemies
                   Projectiles  = updateProjectiles createProjectileAtShip isShootingNow dt gameState.Projectiles
                   Gun          = newGun
  }

type Drawable = 
  {
    Position : Vector2
    Image    : string
  }

let drawProjectile (projectile:Projectile) : Drawable =
  {
    Drawable.Position = projectile.Position
    Drawable.Image    = "laser.png"
  }

let drawEnemy (enemy:Enemy) : Drawable =
  {
    Drawable.Position = enemy.Position
    Drawable.Image    = "enemy.png"
  }

let drawState (gameState:GameState) : seq<Drawable> =
  let listOfDrawableProjectiles =
    map drawProjectile gameState.Projectiles |> toFSharpList
  let listOfDrawableEnemies =
    map drawEnemy gameState.Enemies |> toFSharpList
  [
    {
      Drawable.Position = gameState.Ship.Position
      Drawable.Image    = "spaceShip.png"
    }
  ] @ listOfDrawableEnemies @ listOfDrawableProjectiles
    |> Seq.ofList
