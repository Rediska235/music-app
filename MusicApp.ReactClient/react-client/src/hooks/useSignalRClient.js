import { useState, useEffect } from 'react';
import * as signalR from '@microsoft/signalr';

const useSignalRClient = () => {
  const [connection, setConnection] = useState(null);
  const [list, setList] = useState([]);
  
  const addMessage = (msg) => setList(prev => [...prev, msg]);

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://songservice.default.svc.cluster.local:5001/song', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);

    newConnection.on('ReceiveMessage', (msg) => {
      addMessage(msg);
    });

    newConnection.start();

    return () => newConnection.stop();
  }, []);

  return { list, connection }
};

export default useSignalRClient;