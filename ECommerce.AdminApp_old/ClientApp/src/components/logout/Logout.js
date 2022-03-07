export default function Logout({ setToken }) {
    function handleLogout() {
        setToken({});
        sessionStorage.clear();

        console.log(setToken);
    }
    return (
        <button onClick={handleLogout}>Logout</button>
    );
};