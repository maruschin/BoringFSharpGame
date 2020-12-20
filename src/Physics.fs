module Physics

open Microsoft.Xna.Framework
open ActorDomain


let AddGravity (gameTime: GameTime) actor =
    let ms =
        gameTime.ElapsedGameTime.TotalMilliseconds

    let g = float32 (ms * 0.005)
    match actor.Body with
    | Dynamic (velocity) ->
        let newVelocity = { velocity with Y = (velocity.Y + g) }
        { actor with
              Body = Dynamic(newVelocity) }
    | _ -> actor

let ResolveVelocities actor =
    match actor.Body with
    | Dynamic (velocity) ->
        let newLocation =
            Location.FromVector2
                (actor.Bound.Location.ToVector2
                 + velocity.ToVector2)

        { actor with
              Bound =
                  { actor.Bound with
                        Location = newLocation } }
    | _ -> actor

let IsActorStatic actor =
    match actor.Body with
    | Static -> true
    | _ -> false

let PartitionWorldObjects worldObjects =
    worldObjects |> List.partition IsActorStatic

let AddFriction actor =
    match actor.Body with
    | Dynamic (velocity) ->
        let newVelocity =
            { velocity with
                  X = (velocity.X * 0.95f) }

        { actor with
              Body = Dynamic(newVelocity) }
    | _ -> actor


let HandleCollisions worldObjects =
    let stc, dyn = PartitionWorldObjects worldObjects

    let findNewVelocity dynamicBound staticBound velocity =
        let inter =
            Rectangle.Intersect(dynamicBound, staticBound)

        match (inter.Height > inter.Width), (inter.Width > inter.Height) with
        | true,  true  -> { Velocity.X = 0.f; Y = 0.f }
        | true,  false -> { velocity with X = 0.f }
        | false, true  -> { velocity with Y = 0.f }
        | false, false -> velocity

    let findOptimumCollision actor1 actor2 =
        match actor1.Type, actor2.Type with
        | Player (h), Obstacle ->
            match actor1.Body, actor2.Body with
            | Dynamic (velocity), Static ->
                { actor1 with
                      Body = Dynamic((findNewVelocity actor1.DesiredBounds actor2.CurrentBounds velocity))
                      Type = Player(Idle) }
            | _ -> actor1
        | _ -> actor1

    let rec figureCollisions (actor: Actor) (sortedActors: Actor list) =
        match sortedActors with
        | [] -> actor
        | x :: xs ->
            let a =
                if actor.DesiredBounds.Intersects x.DesiredBounds
                then findOptimumCollision actor x
                else actor

            figureCollisions a xs

    let rec fixCollisions (toFix: Actor list) (alreadyFixed: Actor list) =
        match toFix with
        | [] -> alreadyFixed
        | x :: xs ->
            let a = figureCollisions x alreadyFixed
            fixCollisions xs (a :: alreadyFixed)

    fixCollisions dyn stc
