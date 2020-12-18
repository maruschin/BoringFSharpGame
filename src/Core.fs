module Core

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Actor
open Physics
open Input


type BoringGame () as this =
    inherit Game()

    do this.Content.RootDirectory <- "Content"

    let mutable graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>

    do
        graphics.PreferredBackBufferWidth <- 256 * 3
        graphics.PreferredBackBufferHeight <- 256 * 2
    let mutable WorldObjects = GetActors this.Content

    override this.Initialize() =
        do spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        do base.Initialize()
        ()

    override this.LoadContent() =
        do WorldObjects.Force () |> ignore
        ()

    override this.Update (gameTime) =
        let addGravity = AddGravity gameTime
        let handleInput = HandleInput (Keyboard.GetState ())
        let updateActorAnimation = UpdateActorAnimation gameTime
        let current = WorldObjects.Value
        do WorldObjects <- lazy (
            current
            |> List.map handleInput
            |> List.map addGravity
            |> List.map AddFriction
            |> HandleCollisions
            |> List.map ResolveVelocities
            |> List.map updateActorAnimation
            )
        do WorldObjects.Force () |> ignore
        ()

    override this.Draw (gameTime) =
        do this.GraphicsDevice.Clear Color.CornflowerBlue
        let drawActor = DrawActor spriteBatch
        do spriteBatch.Begin (SpriteSortMode.FrontToBack)
        WorldObjects.Value |> List.iter drawActor
        do spriteBatch.End ()
        ()

