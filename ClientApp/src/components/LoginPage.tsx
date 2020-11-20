import React, { useState } from 'react';
import { useLocation } from 'wouter';

type LoginPageProps = {
  handleLogin: (username: string, password: string) => Promise<boolean>;
}

export default function LoginPage(props: LoginPageProps) {
  let [location, setLocation] = useLocation();
  let [userName, setUserName] = useState('');
  let [password, setPassword] = useState('');

  const submitForm = () => {
    console.log(" submit: logging in");
    const isSuccess = props.handleLogin(userName, password);
    if (isSuccess) {
      setLocation('/')
    } else {
      alert("Login unsuccessful");
    }
  }

  return (
    <div>
      <div>
        <label htmlFor="userNameInput">User name:</label>
        <input
          type="text"
          value={userName}
          name="userNameInput"
          onChange={e => setUserName(e.target.value)}
        />
      </div>

      <div>
        <label htmlFor="passwordInput">Password:</label>
        <input
          type="password"
          value={password}
          name="passwordInput"
          onChange={e => setPassword(e.target.value)}
        />
      </div>

      <button onClick={submitForm}>Login</button>
    </div>
  );
}