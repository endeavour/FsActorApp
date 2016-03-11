module FsActorApp.Program

open System.Fabric
open System.Threading
open Microsoft.ServiceFabric.Actors
open FsActorApp.Actors
open FsActorApp.ActorEventSource

[<EntryPoint>]
let main argv = 
    try 
        use fabricRuntime = FabricRuntime.Create()
        fabricRuntime.RegisterActor<FsMyActor>()
        Thread.Sleep(Timeout.Infinite)
    with e -> 
        ActorEventSource.Current.ActorHostInitializationFailed(e.ToString())
        raise (e)
    0
