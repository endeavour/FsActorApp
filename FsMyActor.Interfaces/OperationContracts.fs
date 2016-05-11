module FsMyActor.OperationContracts

open System.Threading.Tasks
open Microsoft.ServiceFabric.Actors

type IFsMyActor = 
    inherit IActor
    abstract GetCountAsync : unit -> Task<int>
    abstract SetCountAsync : int -> Task
