import React, { useState } from 'react';

const Login = (props) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const handleSubmit = (event) => {
      event.preventDefault();
  
      fetch('http://localhost:5000/api/identity/login', {
        method: 'POST',
        body: JSON.stringify({ username: username, password: password }),
        headers: { 'Content-Type': 'application/json' },
      })
        .then(response => {
          if(!response.ok) return;

          response.text().then(token => props.setToken(token))          
          props.setUsername(username);
          props.setIsInLogin(false);
        })
      
      setUsername('');
      setPassword('');
    };
  
    return (
      <div className="d-flex justify-content-center mt-5">
        <div>
          <h2>Log in</h2>
          <form onSubmit={handleSubmit}>
            <div className="form-outline mb-4 mt-4">
              <label className="form-label">Username</label>
              <input type="text" className="form-control" value={username} onChange={(e) => setUsername(e.target.value)} required/>
            </div>

            <div className="form-outline mb-4">
              <label className="form-label">Password</label>
              <input type="password" className="form-control" value={password} onChange={(e) => setPassword(e.target.value)} required/>
            </div>

            <div className="d-flex justify-content-end">
              <button type="submit" className="btn btn-primary btn-block mb-4 align-center">
                Sign in
              </button>
            </div>
          </form>
        </div>
      </div>
    );
  };
  
  export default Login;