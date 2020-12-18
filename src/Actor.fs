module Actor

open Animation
open ActorDomain
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content

type MyVector2 = {X: float32; Y: float32}

let CreateActor (content: ContentManager) actorType bodyType size (TextureName textureName) position =
    let texture = content.Load textureName
    let animation =
        match actorType with
        | Player(s) -> Animated(CreateAnimation texture 100)
        | Enemy(s) -> Animated(CreateAnimation texture 100)
        | Obstacle -> NotAnimated(texture)
        | Background -> NotAnimated(texture)
    {
        Type = actorType;
        Position = position;
        Size = size;
        Animation = animation;
        Body = bodyType;
    }

let GetActors (content: ContentManager) =
    let width = 72.f
    let createPlayer =
        CreateActor content (Player(Idle)) (Dynamic(Vector2.Zero))
            (Size (Vector2(72.f, 97.f))) (TextureName "Player/p1_front")
    let createObstacles =
        CreateActor content Obstacle Static (Size (Vector2(72.f, 72.f))) (TextureName "Tiles/grassMid")
    let createBackGround = CreateActor content Background Static (Size (Vector2(256.f, 256.f)))
    let createObstaclesGrassMid = createObstacles
    let createActors (f, pos) = f (Position (Vector2(pos.X, pos.Y)))
    lazy (
        [
            (createPlayer, {X=10.f; Y=28.f});
            (createObstacles, {X=width * 0.f; Y=512.f - width});
            (createObstacles, {X=width * 1.f; Y=512.f - width});
            (createObstacles, {X=width * 2.f; Y=512.f - width});
            (createObstacles, {X=width * 3.f; Y=512.f - width});
            (createObstacles, {X=width * 4.f; Y=512.f - width});
            (createObstacles, {X=width * 5.f; Y=512.f - width});
            (createObstacles, {X=width * 6.f; Y=512.f - width});
            (createObstacles, {X=width * 7.f; Y=512.f - width});
            (createObstacles, {X=width * 8.f; Y=512.f - width});
            (createObstacles, {X=width * 9.f; Y=512.f - width});
            //(createBackGround, Vector2(  0.f, 0.f), "bg");
            //(createBackGround, Vector2(256.f, 0.f), "bg");
            //(createBackGround, Vector2(512.f, 0.f), "bg");
            //(createBackGround, Vector2(  0.f, 256.f), "bg_castle");
            //(createBackGround, Vector2(256.f, 256.f), "bg_castle");
            //(createBackGround, Vector2(512.f, 256.f), "bg_castle");
         ]
        |> List.map createActors
    )

let UpdateActorAnimation (gameTime: GameTime) (actor: Actor) =
    let animation =
        match actor.Animation with
        | Animated(animation) -> Animated(UpdateAnimation gameTime animation)
        | NotAnimated(size) -> NotAnimated(size)
    { actor with Animation = animation }

let DrawActor (spriteBatch: SpriteBatch) (actor: Actor) =
    match actor.Animation with
    | Animated (animation) ->
        do DrawAnimation spriteBatch animation actor.Position
    | NotAnimated (texture) ->
        do DrawTexture spriteBatch texture actor.Position
    ()
