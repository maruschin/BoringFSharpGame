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

    let mutable graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>

    do
        graphics.PreferredBackBufferWidth <- 256 * 3
        graphics.PreferredBackBufferHeight <- 256 * 2
    let mutable WorldObjects = 
        let CreateActorWithContent = CreateActor this.Content
        lazy (
            [
                ("player", Player(Nothing), Vector2(10.f,28.f), Vector2(32.f,32.f), Dynamic(Vector2.Zero) );
                ("obstacle", Obstacle, Vector2(10.f,60.f), Vector2(32.f,32.f), Static);
                ("animtest", Obstacle, Vector2(42.f, 60.f), Vector2(32.f,32.f), Static);
                ("bg", BackGround, Vector2(  0.f, 0.f), Vector2(128.f,128.f), Static);
                ("bg", BackGround, Vector2(256.f, 0.f), Vector2(128.f,128.f), Static);
                ("bg", BackGround, Vector2(512.f, 0.f), Vector2(128.f,128.f), Static);
                ("bg_castle", BackGround, Vector2(  0.f, 256.f), Vector2(128.f,128.f), Static);
                ("bg_castle", BackGround, Vector2(256.f, 256.f), Vector2(128.f,128.f), Static);
                ("bg_castle", BackGround, Vector2(512.f, 256.f), Vector2(128.f,128.f), Static);
             ]
            |> List.map CreateActorWithContent
        )

    let DrawActor (sb:SpriteBatch) actor =
        let position = actor.Position
        match actor.Animation with
        | Animated (animation) ->
            do DrawAnimation sb animation actor.Position
        | NotAnimated (texture) ->
            do DrawTexture sb texture actor.Position
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
        do spriteBatch.Begin (SpriteSortMode.FrontToBack)
        WorldObjects.Value |> List.iter DrawActor'
        do spriteBatch.End ()
        ()

