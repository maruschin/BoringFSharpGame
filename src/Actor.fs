module Actor

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content
open Animation

type BodyType =
    | Static
    | Dynamic of Vector2

type PlayerState =
    | Nothing
    | Jumping

type ActorType =
    | Player of PlayerState
    | Obstacle
    | BackGround

type AnimationType =
    | Animated of Animation
    | NotAnimated of Texture2D

type WorldActor =
    {
        ActorType : ActorType;
        Position : Vector2;
        Size : Vector2;
        Animation : AnimationType;
        BodyType : BodyType
    }
    member this.CurrentBounds =
        Rectangle((int this.Position.X),
                  (int this.Position.Y),
                  (int this.Size.X),
                  (int this.Size.Y))
    
    member this.DesiredBounds =
        let desiredPos =
            match this.BodyType with
            | Dynamic(s) -> this.Position + s
            | Static -> this.Position
        Rectangle((int desiredPos.X),
                  (int desiredPos.Y),
                  (int this.Size.X),
                  (int this.Size.Y))

let CreateActor (content:ContentManager) (textureName, actorType, position, size, bodyType) =
    let tex = content.Load textureName
    let animation =
        match actorType with
        | Player(s) -> Animated(CreateAnimation tex 100)
        | Obstacle -> Animated(CreateAnimation tex 100)
        | BackGround -> NotAnimated ( tex )
    {
        ActorType = actorType;
        Position = position;
        Size = size;
        Animation = animation;
        BodyType = bodyType;
    }

let UpdateActorAnimation gameTime (actor:WorldActor) =
    let animation =
        match actor.Animation with
        | Animated(animation) -> Animated(UpdateAnimation gameTime animation)
        | NotAnimated(size) -> NotAnimated(size)
    { actor with Animation = animation }
