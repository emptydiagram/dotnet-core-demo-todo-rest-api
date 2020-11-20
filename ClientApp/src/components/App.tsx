import React, { useEffect } from 'react';
import { Link, Route, Switch, useLocation } from 'wouter';
import ky from 'ky';

import TodoListApp from './TodoListApp';
import LoginPage from './LoginPage';

function App() {
  let [location, setLocation] = useLocation();
  useEffect(() => {
    // detect if the user is logged in
    // if not, redirect to login page
    // else: fetch todo list and display
    (async () => {
      let resp;
      try {
        resp = await ky.get('https://localhost:5001/api/v1/Account/Login', {});
      } catch (error) {
        if (error instanceof ky.HTTPError) {
          console.log("error response = ", error.response)
          if (error.response.status === 401) {
            // not logged in, redirect to login
            setLocation('/login');
          } else {
            console.log("Unanticipated HTTP error response: ", error.response);
          }
        } else {
          console.log("Unknown error occurred: ", error);
        }
      }

      if (resp != null && resp.status >= 200 && resp.status <= 299) {
        // logged in, get list and display it
        console.log(" :: response = ", resp, resp.status, resp.statusText);

        let todoResp = await ky.get('https://localhost:5001/api/v1/TodoItems', {});
        console.log(" :: todo response = ", todoResp, todoResp.status, todoResp.statusText);
        setLocation('/todo');
      }
    })();
  }, []);

  const doLogin = async (username: string, password: string) => {
    let resp;
    try {
      resp = await ky.post('https://localhost:5001/api/v1/Account/Login', {
        json: { UserName: username, Password: password }
      });
    } catch (error) {
      if (error instanceof ky.HTTPError) {
        return false;
      } else {
        console.log("Unknown error occurred: ", error);
      }

    }

    return resp != null && resp.status >= 200 && resp.status <= 299;
  };

  return (
    <div>
      <Link href="/login">login</Link>
      <Link href="/todo">todo</Link>

      <Switch>
        <Route path="/login">
          <LoginPage
            handleLogin={doLogin}
          />
        </Route>
        <Route path="/todo">
          <TodoListApp />
        </Route>
      </Switch>
    </div>
  );
}

export default App;
