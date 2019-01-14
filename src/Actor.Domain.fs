module ActorDomain

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content
open Animation

type Direction =
	| Forward
	| Backward

type Action =
	| Idle
	| Moving of Direction
	| jumping
	| JumpingTo of Direction

type Animation =
	| Animated of Animation
	| Static of Texture2D

type ActorType =
	| Player of Action
	| Enemy of Action
	| Obstacle
	| BackGround

type BodyType =
	| Static
	| Dynamic

type Size = Size of Vector2

type Position = Position of Vector2

let getBounds (position:Position) (size:Size) =
	Rectangle((int position.X),
	          (int position.Y),
	          (int size.X),
	          (int size.Y))

type Actor =
	{
		Type : ActorType;
		Position : Position;
		Size : Size;
		Animation : Animation;
	}

	member this.CurrentBounds =
		getBounds this.Position this.Size

	member this.DesiredBounds =
		match this.BodyType with
		| Static -> getBound this.Position this.Size
		| Dynamic(s) -> getBound this.Position + s this.Size

let Create Actor (content:ContentManager) (actorType:ActorType) (bodyType:BodyType) (size:Size) (position:Position) (textureName) =
	let tex = content.Load textureName
	let animation =
		match actorType with
		| Player(s)
		| Enemy(s) -> Animated(CreateAnimation tex 100)
		| Obstacle
		| Background -> NotAnimated(tex)

	{
		ActorType = actorType;
		Position = position;
		Size = size;
		Animation = animation;
		BodyType = bodyType;
	}
