import { Navigate, Outlet } from 'react-router';
import { useAuth } from './context/AuthContext';
import Dashboard from './pages/Dashboard';
import LoginPage from './pages/LoginPage';

function ProtectedLayout() {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />;
}

const routes = [
  { path: '/login', element: <LoginPage /> },
  {
    element: <ProtectedLayout />,
    children: [{ path: '/', element: <Dashboard /> }],
  },
];

export default routes;
