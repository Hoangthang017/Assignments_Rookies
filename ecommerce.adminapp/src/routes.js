import { Navigate, useRoutes } from 'react-router-dom';
// layouts
import DashboardLayout from './layouts/dashboard';
import LogoOnlyLayout from './layouts/LogoOnlyLayout';
//
import Login from './pages/Login';
import Register from './pages/Register';
import DashboardApp from './pages/DashboardApp';
import Product from './pages/Product';
import Blog from './pages/Blog';
import User from './pages/User';
import NotFound from './pages/Page404';
import CreateUser from './sections/user/CreateUser';
import Category from './pages/Category';
import CreateOrUpdateCategory from './sections/category/CreateOrUpdateCategory';
import CreateOrUpdateProduct from './sections/product/CreateOrUpdateProduct'

// ----------------------------------------------------------------------

export default function Router() {
  return useRoutes([
    {
      path: '/dashboard',
      element: <DashboardLayout />,
      children: [
        { path: '', element: <DashboardApp /> },
        { path: 'user', element: <Navigate to="/user" /> },
        { path: 'category', element: <Navigate to="/category" /> },
        { path: 'products', element: <Navigate to="/product" /> },
        { path: 'blog', element: <Blog /> }
      ]
    },
    {
      path: '/user',
      element: <DashboardLayout />,
      children: [
        { path: '', element: <User /> },
        { path: 'create', element: <CreateUser /> },
        { path: 'edit/:id', element: <CreateUser />}
      ]
    },
    {
      path: '/product',
      element: <DashboardLayout />,
      children: [
        { path: '', element: <Product/> },
        { path: 'create', element: <CreateOrUpdateProduct /> },
        { path: 'edit/:id', element: <CreateOrUpdateProduct /> },
      ]
    },
    {
      path: '/category',
      element: <DashboardLayout />,
      children: [
        { path: '', element: <Category /> },
        { path: 'create', element: <CreateOrUpdateCategory /> },
        { path: 'edit/:id', element: <CreateOrUpdateCategory /> }
      ]
    },
    {
      path: '/',
      element: <LogoOnlyLayout />,
      children: [
        { path: '/', element: <Navigate to="/dashboard" /> },
        { path: 'login', element: <Login /> },
        { path: 'register', element: <Register /> },
        { path: '404', element: <NotFound /> },
        { path: '*', element: <Navigate to="/404" /> }
      ]
    },
    { path: '*', element: <Navigate to="/404" replace /> }
  ]);
}
