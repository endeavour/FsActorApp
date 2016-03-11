module FsMyActor.OperationContracts

open System.Threading.Tasks
open Microsoft.ServiceFabric.Actors

[<Interface>]
type IFsMyActor = 
    inherit IActor
    abstract GetCountAsync : unit -> int Task
    abstract SetCountAsync : unit -> Task
