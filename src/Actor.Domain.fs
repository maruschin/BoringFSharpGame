module ActorDomain

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content
open Animation

type Direction =
    | Forward
    | Backward

type ActionType =
    | Idle
    | Moving of Direction
    | Jumping

type AnimationType =
    | Animated of Animation
    | NotAnimated of Texture2D

type ActorType =
    | Player of ActionType
    | Enemy of ActionType
    | Obstacle
    | Background

type BodyType =
    | Static
    | Dynamic of Vector2

type Size = Size of Vector2

type Position = Position of Vector2

let getBounds (position:Vector2) (size:Vector2) =
    Rectangle((int position.X),
              (int position.Y),
              (int size.X),
              (int size.Y))

type Actor =
    {
        Type : ActorType;
        Position : Vector2;
        Size : Vector2;
        Animation : AnimationType;
        Body : BodyType;
    }

    member this.CurrentBounds =
        getBounds this.Position this.Size

    member this.DesiredBounds =
        match this.Body with
        | Static -> getBounds this.Position this.Size
        | Dynamic(s) -> getBounds (this.Position + s) this.Size

let CreateActor (content:ContentManager) (actorType:ActorType) (bodyType:BodyType) (size:Vector2) (position:Vector2) (textureName) =
    let tex = content.Load textureName
    let animation =
        match actorType with
        | Player(s) -> Animated(CreateAnimation tex 100)
        | Enemy(s) -> Animated(CreateAnimation tex 100)
        | Obstacle -> NotAnimated(tex)
        | Background -> NotAnimated(tex)

    {
        Type = actorType;
        Position = position;
        Size = size;
        Animation = animation;
        Body = bodyType;
    }
