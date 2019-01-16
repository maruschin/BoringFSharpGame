module Physics

open Microsoft.Xna.Framework
open ActorDomain


let AddGravity (gameTime:GameTime) actor =
    let ms = gameTime.ElapsedGameTime.TotalMilliseconds
    let g = ms * 0.005
    match actor.Body with
    | Dynamic(s) ->
        let d = Vector2(s.X, s.Y + (float32 g))
        { actor with Body = Dynamic(d); }
    | _ -> actor

let ResolveVelocities actor =
    match actor.Body with
    | Dynamic(s) ->
        { actor with Position = actor.Position + s }
    | _ -> actor

let IsActorStatic actor =
    match actor.Body with
    | Static -> true
    | _ -> false

let PartitionWorldObjects worldObjects =
    worldObjects
    |> List.partition IsActorStatic

let AddFriction actor =
    match actor.Body with
    | Dynamic(v) ->
        let newV = Vector2(v.X*0.95f, v.Y)
        { actor with Body = Dynamic(newV) }
    | _ -> actor


let HandleCollisions worldObjects =
    let stc, dyn = PartitionWorldObjects worldObjects

    let FindNewVelocity rect1 rect2 velocity =
        let inter = Rectangle.Intersect(rect1, rect2)
        let mutable (newVel: Vector2) = velocity
        if inter.Height > inter.Width then
                do newVel.X <- 0.f
        if inter.Width > inter.Height then
                do newVel.Y <- 0.f
        newVel
                    
    let FindOptimumCollision a b = 
        match a.Type, b.Type with
        | Player(h), Obstacle ->
            match a.Body, b.Body with
            | Dynamic(s), Static ->
                {a with Body = Dynamic((FindNewVelocity a.DesiredBounds b.CurrentBounds s)); Type = Player(Idle) }
            | _ -> a
        | _ -> a

    let rec FigureCollisions (actor:Actor) (sortedActors: Actor list) =
        match sortedActors with
        | [] -> actor
        | x::xs ->
            let a =
                if actor.DesiredBounds.Intersects x.DesiredBounds then
                    FindOptimumCollision actor x
                else
                    actor
            FigureCollisions a xs

    let rec FixCollisions (toFix:Actor list) (alreadyFixed:Actor list) =
        match toFix with
        | [] -> alreadyFixed
        | x::xs -> 
            let a = FigureCollisions x alreadyFixed
            FixCollisions xs (a::alreadyFixed)

    FixCollisions dyn stc
