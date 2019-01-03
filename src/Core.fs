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
        do spriteBatch.Begin (SpriteSortMode.FrontToBack)
        WorldObjects.Value |> List.iter DrawActor'
        do spriteBatch.End ()
        ()

