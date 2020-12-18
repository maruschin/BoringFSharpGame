module ActorDomain

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open AnimationDomain

type MyVector2 = {X: float32; Y: float32}

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

type TextureName = TextureName of string

let getBounds (Position position) (Size size) =
    Rectangle((int position.X),
              (int position.Y),
              (int size.X),
              (int size.Y))

let getNewPosition (Position position) (speed: Vector2) =
    Position (position + speed)

type Actor =
    {
        Type : ActorType;
        Position : Position;
        Size : Size;
        Animation : AnimationType;
        Body : BodyType;
    }

    member this.CurrentBounds =
        getBounds this.Position this.Size

    member this.DesiredBounds =
        match this.Body with
        | Static -> getBounds this.Position this.Size
        | Dynamic s -> getBounds (getNewPosition this.Position s) this.Size