import HomePage from './pages/Home';
import LoginPage from './pages/Login';
import { createBrowserRouter, createRoutesFromElements, Outlet, Route, RouterProvider } from 'react-router-dom';
import './App.css';
import Registration from './pages/Registration';

function App() {

  const router = createBrowserRouter(createRoutesFromElements(
    <Route path="/" element={ <Layout /> }>
      <Route path='/' element={ <HomePage /> } />
      <Route path="/login" element={ <LoginPage /> } />
      <Route path="/registration" element={ <Registration /> } />
    </Route>
  ));

  return <RouterProvider router={router} fallbackElement={<p>Loading...</p>} />;
}

function Layout() {
  return (
  <>
    <h1>Identity App</h1>
    <nav className="navigation-panel">
      <a className="nav-link" href="/">Home</a>
      <a className="nav-link" href="/login">SignIn</a>
      <a className="nav-link" href="/registration">SignUp</a>
    </nav>
    <Outlet />
  </>
  );
}

export default App;
