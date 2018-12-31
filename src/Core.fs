module Core

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Actor
open Animation
open Physics
open Input


type BoringGame () as this =
    inherit Game()

    do this.Content.RootDirectory <- "Content"

    let mutable Graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>

    let mutable WorldObjects = 
        let CreateActorWithContent = CreateActor this.Content
        lazy (
            [("player", Player(Nothing), Vector2(10.f,28.f), Vector2(32.f,32.f), false);
             ("obstacle", Obstacle, Vector2(10.f,60.f), Vector2(32.f,32.f), true);
             ("animtest", Obstacle, Vector2(42.f, 60.f), Vector2(32.f,32.f), true);] 
            |> List.map CreateActorWithContent
        )

    let DrawActor (sb:SpriteBatch) actor =
        if actor.Animation.IsSome then
            do DrawAnimation sb actor.Animation.Value actor.Position
        ()

    override this.Initialize() =
        do spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        do base.Initialize()
        ()

    override this.LoadContent() =
        do WorldObjects.Force () |> ignore
        ()

    override this.Update (gameTime) =
        let AddGravity' = AddGravity gameTime
        let HandleInput' = HandleInput (Keyboard.GetState ())
        let UpdateActorAnimation' = UpdateActorAnimation gameTime
        let current = WorldObjects.Value
        do WorldObjects <- lazy (
            current
            |> List.map HandleInput'
            |> List.map AddGravity'
            |> List.map AddFriction
            |> HandleCollisions
            |> List.map ResolveVelocities
            |> List.map UpdateActorAnimation'
            )
        do WorldObjects.Force () |> ignore
        ()

    override this.Draw (gameTime) =
        do this.GraphicsDevice.Clear Color.CornflowerBlue
        let DrawActor' = DrawActor spriteBatch
        do spriteBatch.Begin ()
        WorldObjects.Value |> List.iter DrawActor'
        do spriteBatch.End ()
        ()

