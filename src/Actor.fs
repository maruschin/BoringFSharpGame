module Actor

open Animation
open ActorDomain
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content

let GetActors (content:ContentManager) =
    let CreateActorWithContent = CreateActor content
    lazy (
        [
            ("Player/p1_front", Player(Nothing), Vector2(10.f,28.f), Vector2(72.f,97.f), Dynamic(Vector2.Zero) );
            ("Tiles/grassMid", Obstacle, Vector2(32.f              , 256.f), Vector2(72.f, 72.f), Static);
            ("Tiles/grassMid", Obstacle, Vector2(32.f + 70.f       , 256.f), Vector2(72.f, 72.f), Static);
            ("Tiles/grassMid", Obstacle, Vector2(32.f +  2.f * 70.f, 256.f), Vector2(72.f, 72.f), Static);
            ("Tiles/grassMid", Obstacle, Vector2(32.f +  3.f * 70.f, 256.f), Vector2(72.f, 72.f), Static);
            ("Tiles/grassMid", Obstacle, Vector2(32.f +  4.f * 70.f, 256.f), Vector2(72.f, 72.f), Static);
            ("Tiles/grassMid", Obstacle, Vector2(32.f +  5.f * 70.f, 256.f), Vector2(72.f, 72.f), Static);
            ("Tiles/grassMid", Obstacle, Vector2(32.f +  6.f * 70.f, 256.f), Vector2(72.f, 72.f), Static);
            ("Tiles/grassMid", Obstacle, Vector2(32.f +  7.f * 70.f, 256.f), Vector2(72.f, 72.f), Static);
            ("Tiles/grassMid", Obstacle, Vector2(32.f +  8.f * 70.f, 256.f), Vector2(72.f, 72.f), Static);
            ("Tiles/grassMid", Obstacle, Vector2(32.f +  9.f * 70.f, 256.f), Vector2(72.f, 72.f), Static);
            ("bg", BackGround, Vector2(  0.f, 0.f), Vector2(128.f,128.f), Static);
            ("bg", BackGround, Vector2(256.f, 0.f), Vector2(128.f,128.f), Static);
            ("bg", BackGround, Vector2(512.f, 0.f), Vector2(128.f,128.f), Static);
            ("bg_castle", BackGround, Vector2(  0.f, 256.f), Vector2(128.f,128.f), Static);
            ("bg_castle", BackGround, Vector2(256.f, 256.f), Vector2(128.f,128.f), Static);
            ("bg_castle", BackGround, Vector2(512.f, 256.f), Vector2(128.f,128.f), Static);
         ]
        |> List.map CreateActorWithContent
    )


let UpdateActorAnimation gameTime (actor:WorldActor) =
    let animation =
        match actor.Animation with
        | Animated(animation) -> Animated(UpdateAnimation gameTime animation)
        | NotAnimated(size) -> NotAnimated(size)
    { actor with Animation = animation }

let DrawActor (sb:SpriteBatch) actor =
    let position = actor.Position
    match actor.Animation with
    | Animated (animation) ->
        do DrawAnimation sb animation actor.Position
    | NotAnimated (texture) ->
        do DrawTexture sb texture actor.Position
    ()
