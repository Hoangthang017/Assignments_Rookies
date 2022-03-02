import React, { useState, SyntheticEvent } from 'react';
import { Redirect } from 'react-router-dom'
import 'bootstrap/dist/css/bootstrap.css';

function LoginFrom() {
    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');
    const [remember, setRemember] = useState(false);
    const [redirect, setRedirect] = useState(false);
    const returnUrl = "/";

    const submit = async (e: SyntheticEvent) => {
        console.log({ userName, password, remember });

        e.preventDefault();

        await fetch('http://localhost:5001/api/authenticate', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: 'include',
            body: JSON.stringify({
                "username":"thangnh1394",
                "password":"Admin@123",
                returnUrl
            })
        });

        setRedirect(true);
    }

    if (redirect) {
        return <Redirect to="/" />;
    }

    return (
        <form className='w-25 mx-5'>
            <div className="mb-3">
                <label htmlFor="input-userNameId"
                    className="form-label"
                    value={userName}
                    onChange={e => setUserName(e.target.value)}>
                    Tài khoản
                </label>
                <input className="form-control" id="input-userNameId" placeholder="alice" />
            </div>
            <div className="mb-3">
                <label htmlFor="input-passwordId"
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                    className="form-label">
                    Mật khẩu
                </label>
                <input type="password" className="form-control" id="input-passwordId" placeholder="Pass123$" />
            </div>
            <div className="mb-3 form-check">
                <input type="checkbox"
                    className="form-check-input"
                    id="input-RememberId" />
                <label className="form-check-label" htmlFor="input-RememberId">Nhớ mật khẩu</label>
            </div>
            <button type="submit" className="btn btn-dark" onClick={submit}>Đăng nhập</button>
        </form>
    );
}

export default LoginFrom;