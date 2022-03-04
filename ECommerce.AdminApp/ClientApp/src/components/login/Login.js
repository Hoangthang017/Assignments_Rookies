import 'bootstrap/dist/css/bootstrap.css';

import LoginFrom from './LoginForm'

function Login({ setToken }) {
    return (
        <div className='container-fluid vh-100'>
            <div className="row h-100">
                <div className="col-4 bg-dark text-white">
                    <h1>EBook Shope Management</h1>
                    <h2>Login Page</h2>
                    <p>Login from here for access management</p>
                </div>
                <div className="col-8 d-flex align-items-center">
                    <LoginFrom setToken={setToken}></LoginFrom>
                </div>
            </div>
        </div>
    );
}
export default Login;