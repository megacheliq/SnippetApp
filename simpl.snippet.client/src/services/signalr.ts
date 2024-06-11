import * as signalR from "@microsoft/signalr";
import { MessageObject } from "@/abstract/shareTypes";

const URL = import.meta.env.VITE_API_BASE_URL + 'code';

export function createConnection(): signalR.HubConnection {
  return new signalR.HubConnectionBuilder()
    .withUrl(URL)
    .build();
}

export function startConnection(connection: signalR.HubConnection): Promise<void> {
  return connection.start();
}

export function stopConnection(connection: signalR.HubConnection): Promise<void> {
  return connection.stop();
}

export function addReceiveHandler(connection: signalR.HubConnection, handler: (message: string) => void): void {
  connection.on("Receive", handler);
}

export function addReceiveSelectionHandler(connection: signalR.HubConnection, handler: (messageJson: MessageObject) => void): void {
  connection.on("ReceiveSelection", handler);
}

export function joinGroup(connection: signalR.HubConnection, sessionId: string): Promise<void> {
  return connection.invoke("JoinGroup", sessionId);
}

export function leaveGroup(connection: signalR.HubConnection, sessionId: string): Promise<void> {
  return connection.invoke("LeaveGroup", sessionId);
}

export function sendMessage(connection: signalR.HubConnection, sessionId: string, message: string): Promise<void> {
  return connection.invoke("SendToGroup", sessionId, message);
}

export function sendSelection(connection: signalR.HubConnection, sessionId: string, username: string, messageObject: MessageObject): Promise<void> {
  return connection.invoke("SendSelectionToGroup", sessionId, username, messageObject);
}