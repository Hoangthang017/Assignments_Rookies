import React, { useState, useEffect, useContext } from 'react';
import { Redirect } from 'react-router-dom'
import 'bootstrap/dist/css/bootstrap.css';
import { User } from 'oidc-client';
import AuthContext from '../context/AuthProvider';
import axios from '../api/axios';

const LOGIN_URL = 'account/login';

function LoginFrom() {
    const { setAuth } = useContext(AuthContext)

    const [username, setUserName] = useState('');
    const [password, setPassword] = useState('');
    const [success, setSuccess] = useState(false);
    //const [remember, setRemember] = useState(false);

    const submit = async (e) => {
        e.preventDefault();

        try {
            const respone = await axios.post(LOGIN_URL);

            console.log(respone)
            //axios.get(LOGIN_URL,
            //    {
            //        params: {
            //            client_id: 'admin',
            //            client_secret: 'secret',
            //            scope: 'openid',
            //            username: 'thangnh1394',
            //            password: 'Admin@123',
            //            nonce: 'bearer',
            //            redirect_uri: 'https://localhost:44401/signin-oidc',
            //            response_type: 'token',
            //        }
            //    })
            //    .then(function (reponse) {
            //        console.log(reponse)
            //    })
        }
        catch {
        }
    }

    return (
        <form className='w-25 mx-5' onSubmit={submit}>
            <div className="mb-3">
                <label htmlFor="input-userNameId"
                    className="form-label">
                    Tài khoản
                </label>
                <input
                    value={username}
                    onChange={e => setUserName(e.target.value)}
                    className="form-control"
                    id="input-userNameId"
                    placeholder="alice" />
            </div>
            <div className="mb-3">
                <label htmlFor="input-passwordId"
                    className="form-label">
                    Mật khẩu
                </label>
                <input type="password"
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                    className="form-control"
                    id="input-passwordId"
                    placeholder="Pass123$" />
            </div>
            <div className="mb-3 form-check">
                <input type="checkbox"
                    className="form-check-input"
                    id="input-RememberId" />
                <label className="form-check-label" htmlFor="input-RememberId">Nhớ mật khẩu</label>
            </div>
            <button type="submit" className="btn btn-dark">Đăng nhập</button>
        </form>
    );
}

export default LoginFrom;