import React from 'react';
import CreateSongForm from '../CreateSongForm';
import { LogList } from '../LogList';
import { Welcome } from '../Welcome';

const Home = ({username, token, list, sendSignalRMessage}) => {
  return (
    <div>
      <Welcome username={username} />      
      
      <CreateSongForm token={token} sendSignalRMessage={sendSignalRMessage}/>

      <LogList list={list} />
    </div>
  );
};

export default Home;