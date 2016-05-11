module FsActorApp.Actors

open FsMyActor.OperationContracts
open Microsoft.ServiceFabric.Actors.Runtime
open System.Threading.Tasks
open System.Runtime.Serialization
open FsActorApp.ActorEventSource

[<DataContract>]
[<CLIMutable>]
type ImmutableFsMyActorState = 
    { [<DataMember>]
      Count : int }

module FsMyActorActions = 
    let updateCount state = { state with Count = state.Count + 1 }

open FsMyActorActions

type FsMyActor() = 
    inherit Actor()
    let emptyTask() = Task.FromResult() :> Task

    interface IFsMyActor with
        
        member this.SetCountAsync count = 
          upcast this.StateManager.AddOrUpdateStateAsync("count", count, fun k v ->
            if count > v then count else v)
        
        member this.GetCountAsync() = this.StateManager.GetStateAsync<int>("count")