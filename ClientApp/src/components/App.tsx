import React from 'react';
import { Link, Route, Switch } from 'wouter';

import TodoListApp from './TodoListApp';

function App() {
  return (
    <div>
      <Link href="/login">login</Link>
      <Link href="/todo">todo</Link>

      <Switch>
        <Route path="/login">
          <p>TODO: login stuff</p>
        </Route>
        <Route path="/todo">
          <TodoListApp />
        </Route>
      </Switch>
    </div>
  );
}

export default App;
