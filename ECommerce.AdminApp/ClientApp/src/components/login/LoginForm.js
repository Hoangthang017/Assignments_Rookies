import React, { useState, useEffect, useContext } from 'react';
import { Redirect } from 'react-router-dom'
import 'bootstrap/dist/css/bootstrap.css';
import { User } from 'oidc-client';
import axios from '../api/axios';

const LOGIN_URL = 'api/Users/authenticate';

async function loginUser(credentials) {
    const reponse = await axios.post(LOGIN_URL,
        JSON.stringify(credentials),
        {
            headers: {
                'Content-Type': 'application/json'
            },
        })

    return reponse.data;
}

function LoginFrom({ setToken }) {
    const [username, setUserName] = useState('');
    const [password, setPassword] = useState('');

    const handleSubmit = async e => {
        e.preventDefault();
        try {
            const token = await loginUser({
                ClientId: 'react-admin',
                ClientSecret: 'D013F030-0177-4F0D-AECA-1206D0608408',
                Scope: "openid profile swaggerApi",
                UserName: username,
                Password: password,
            })
            console.log(token);
            setToken(token);
        }
        catch {
        }
    }

    return (
        <form className='w-25 mx-5' onSubmit={handleSubmit}>
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