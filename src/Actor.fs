module Actor

open Animation
open ActorDomain
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content

let GetActors (content:ContentManager) =
    let createPlayer     = CreateActor content (Player(Idle)) (Dynamic(Vector2.Zero)) (Vector2(72.f,97.f))
    let createObstacles  = CreateActor content  Obstacle       Static                 (Vector2(72.f,72.f))
    let createBackGround = CreateActor content  Background     Static                 (Vector2(128.f,128.f))
    let createActors (f, pos, tex) = f pos tex
    lazy (
        [
            (createPlayer, Vector2(10.f,28.f), "Player/p1_front" );
            (createObstacles, Vector2(32.f              , 256.f), "Tiles/grassMid");
            (createObstacles, Vector2(32.f + 70.f       , 256.f), "Tiles/grassMid");
            (createObstacles, Vector2(32.f +  2.f * 70.f, 256.f), "Tiles/grassMid");
            (createObstacles, Vector2(32.f +  3.f * 70.f, 256.f), "Tiles/grassMid");
            (createObstacles, Vector2(32.f +  4.f * 70.f, 256.f), "Tiles/grassMid");
            (createObstacles, Vector2(32.f +  5.f * 70.f, 256.f), "Tiles/grassMid");
            (createObstacles, Vector2(32.f +  6.f * 70.f, 256.f), "Tiles/grassMid");
            (createObstacles, Vector2(32.f +  7.f * 70.f, 256.f), "Tiles/grassMid");
            (createObstacles, Vector2(32.f +  8.f * 70.f, 256.f), "Tiles/grassMid");
            (createObstacles, Vector2(32.f +  9.f * 70.f, 256.f), "Tiles/grassMid");
            (createBackGround, Vector2(  0.f, 0.f), "bg");
            (createBackGround, Vector2(256.f, 0.f), "bg");
            (createBackGround, Vector2(512.f, 0.f), "bg");
            (createBackGround, Vector2(  0.f, 256.f), "bg_castle");
            (createBackGround, Vector2(256.f, 256.f), "bg_castle");
            (createBackGround, Vector2(512.f, 256.f), "bg_castle");
         ]
        |> List.map createActors
    )


let UpdateActorAnimation gameTime (actor:Actor) =
    let animation =
        match actor.Animation with
        | Animated(animation) -> Animated(UpdateAnimation gameTime animation)
        | NotAnimated(size) -> NotAnimated(size)
    { actor with Animation = animation }

let DrawActor (sb:SpriteBatch) actor =
    match actor.Animation with
    | Animated (animation) ->
        do DrawAnimation sb animation actor.Position
    | NotAnimated (texture) ->
        do DrawTexture sb texture actor.Position
    ()
