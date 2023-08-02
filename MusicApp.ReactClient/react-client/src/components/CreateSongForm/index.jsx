import React, { useState } from 'react';

const CreateSongForm = (props) => {
  const [songTitle, setSongTitle] = useState('');

  const createSong = () => {
    fetch('http://localhost:5001/api/songs', {
        method: 'POST',
        body: JSON.stringify({ title: songTitle }),
        headers: { 'Authorization': `Bearer ${props.token}`, 'Content-Type': 'application/json'},
      })
      .then((response) => {
        if(response.status === 201){
          console.log('ok');
          props.sendSignalRMessage( songTitle );
        }
      });
    setSongTitle('');
  };

  return (
    <div className="d-flex justify-content-center m-5">
      <div>
      <label>Song title</label>
      <input type="text" className="form-control" placeholder='Song title' value={songTitle} onChange={(e) => setSongTitle(e.target.value)} /> 
      
      <button type="submit" className="btn btn-primary mt-2" onClick={createSong}>Create song</button>
      </div>
    </div>
  );
};

export default CreateSongForm;