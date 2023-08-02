import React, { useState } from 'react';
import Login from './components/Login';
import Home from './components/Home';
import useSignalRClient from './hooks/useSignalRClient';

const App = () => {
  const [username, setUsername] = useState('');
  const [token, setToken] = useState('');
  const [isInLogin, setIsInLogin] = useState(false);
  const { list, connection } = useSignalRClient()

  const updateUser = (username) => {
    console.log(username);
    setUsername(username);
  }

  const updateToken = (token) => {
    console.log(token);
    setToken(token)
  }

  const updateIsInLogin = (value) => {
    console.log(value);
    setIsInLogin(value)
  }
    
  const sendSignalRMessage = (song) => {
    connection?.invoke('Send', username, song);
  }

  return (
    <div>
      <div className="d-flex justify-content-end">
        <button onClick={() => {setIsInLogin(!isInLogin);}} className="btn btn-primary m-3">
          {isInLogin === false ? 'Log in' : 'Home' }
        </button>
      </div>

      {isInLogin === false 
      ? 
      <Home username={username} token={token} list={list} sendSignalRMessage={sendSignalRMessage}/>
      : 
      <Login setToken={updateToken} setUsername={updateUser} setIsInLogin={updateIsInLogin}/>}
    </div>
  );
};

export default App;