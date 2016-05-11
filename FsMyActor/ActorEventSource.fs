﻿module FsActorApp.ActorEventSource

open System.Diagnostics.Tracing
open System.Fabric
open System.Threading.Tasks
open Microsoft.ServiceFabric.Actors.Runtime

[<Literal>]
let MessageEventId = 1

[<Literal>]
let ActorMessageEventId = 2

[<Literal>]
let ActorHostInitializationFailedEventId = 3

[<Sealed>]
[<EventSource(Name = "MyCompany-FsActorApp-FsMyActor")>]
type internal ActorEventSource =
    inherit EventSource

    private new() = { inherit EventSource }

    static member Current : ActorEventSource = new ActorEventSource()

    static member ActorEventSource = Task.FromResult().Wait()

    [<NonEvent>]
    member this.Message (message : string, args : array<'T>) =
        if this.IsEnabled() then
            let finalMessage = System.String.Format(message, args)
            this.Message(finalMessage)

    [<Event(MessageEventId, Level = EventLevel.Informational, Message = "{0}")>]
    member this.Message (message : string) =
        if this.IsEnabled() then
            this.WriteEvent(MessageEventId, message)

    [<NonEvent>]
    member this.ActorMessage (actor : Actor, message, args : array<'T>) =
        if this.IsEnabled() then
            let finalMessage = System.String.Format(message, args)
            this.ActorMessage(
                actor.GetType().ToString(),
                actor.Id.ToString(),
                actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                actor.ActorService.Context.ServiceTypeName,
                actor.ActorService.Context.ServiceName.ToString(),
                actor.ActorService.Context.PartitionId,
                actor.ActorService.Context.ReplicaId,
                actor.ActorService.Context.NodeContext.NodeName,
                finalMessage)
    
    [<Event(ActorMessageEventId, Level = EventLevel.Informational, Message = "{9}")>]
    member private this.ActorMessage (actorType, actorId, applicationTypeName, applicationName, serviceTypeName, serviceName, partitionId, replicaOrInstanceId, nodeName, message) =
        this.WriteEvent(ActorMessageEventId, actorType,
            actorId,
            applicationTypeName,
            applicationName,
            serviceTypeName,
            serviceName,
            partitionId,
            replicaOrInstanceId,
            nodeName,
            message)

    [<Event(ActorHostInitializationFailedEventId, Level = EventLevel.Error, Message = "Actor host initialization failed", Keywords = EventKeywords.None)>]
    member this.ActorHostInitializationFailed (ex : string) =
        this.WriteEvent(ActorHostInitializationFailedEventId, ex)