module FsActorApp.Actors

open FsMyActor.OperationContracts
open Microsoft.ServiceFabric.Actors
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
    inherit StatefulActor<ImmutableFsMyActorState>()
    let emptyTask() = Task.FromResult() :> Task

    interface IFsMyActor with
        
        member this.SetCountAsync() = 
            this.State <- this.State |> updateCount
            ActorEventSource.Current.ActorMessage(this, "Updating Count", [||])
            this.SaveStateAsync()
        
        member this.GetCountAsync() = this.State.Count |> Task.FromResult