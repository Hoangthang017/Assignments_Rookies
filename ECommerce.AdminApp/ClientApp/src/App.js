import React from 'react';
import Login from './components/login/Login'
import useToken from './components/login/useToken'
import Topbar from './components/topbar/Topbar';

const url = "https://serving.photos.photobox.com/1651493216a133738ce610dd288ef961463a44cafe827227e79ed1b5a635475d2b92c23e.jpg"
const title = "CHAPTER INFINITY"
const userName = "admin"
const userEmail = "admin@gmail.com"

function App() {
    const { token, setToken } = useToken();

    if (!token) {
        return <Login setToken={setToken} />
    }

    return (
        <div className="app">
            <Topbar
                setToken={setToken}
                url={url}
                title={title}
                userName={userName}
                userEmail={userEmail} />
        </div>
    );
}

export default App;