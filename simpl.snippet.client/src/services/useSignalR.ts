import { useEffect, useState } from 'react';
import { createConnection, startConnection, stopConnection, addReceiveHandler, sendMessage, joinGroup, leaveGroup, sendSelection, addReceiveSelectionHandler } from '@/services/signalr';
import { MessageObject } from '@/abstract/shareTypes';
import { useSnippetContext } from '@/contexts/SnippetContext';

const useSignalR = (sessionId: string) => {
    const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
    const [codeValue, setCodeValue] = useState("");
    const [messageObject, setMessageObject] = useState<MessageObject>();
    const { setCurrentCode } = useSnippetContext();
    
    const handleConnection = async (newConnection: signalR.HubConnection) => {
        try {
            await startConnection(newConnection);
            console.log("SignalR connected");
            setConnection(newConnection);

            addReceiveHandler(newConnection, (message: string) => {
                setCurrentCode(message || "");
                setCodeValue(message);
            });

            addReceiveSelectionHandler(newConnection, (messageJson: MessageObject) => {
                setMessageObject(messageJson);
            });

            if (sessionId) {
                await joinGroup(newConnection, sessionId);
            }
        } catch (error) {
            console.error("SignalR connection error: ", error);
        }
    };

    useEffect(() => {
        const newConnection = createConnection();

        handleConnection(newConnection);

        const handleUnload = () => {
            if (newConnection && sessionId) {
                leaveGroup(newConnection, sessionId)
                    .then(() => {
                        console.log("Left group");
                    })
                    .catch(err => console.error("Error leaving group: ", err));
            }
        };

        window.addEventListener("beforeunload", handleUnload);

        return () => {
            window.removeEventListener("beforeunload", handleUnload);
            if (newConnection) {
                stopConnection(newConnection);
            }
        };
    }, [sessionId]);

    const sendMessageToGroup = (message: string) => {
        if (connection && sessionId) {          
            setCurrentCode(message || ""); 
            sendMessage(connection, sessionId, message || "")
                .catch(err => console.error("Error sending message: ", err));
        }
    };

    const sendSelectionToGroup = (username: string, messageObject: MessageObject) => {
        if (connection && sessionId) {           
            sendSelection(connection, sessionId, username, messageObject)
                .catch(err => console.error("Error sending selection: ", err))
        }
    };

    return { codeValue, messageObject, sendMessageToGroup, sendSelectionToGroup };
};

export default useSignalR;