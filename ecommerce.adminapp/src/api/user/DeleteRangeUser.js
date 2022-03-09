import axios from "../axios"

async function DeleteRangeUser({userIds}) {
    console.log({})
    try {
        const response = await axios.patch("api/Users/deleteMulti",
        userIds, 
        { 
            'Content-Type': 'application/json',
            headers: { Authorization:  sessionStorage.getItem("token")} 
        })

        if (response.status === 200)
            return true;
    }
    catch(err) {
        console.log(err);
    }
}

export default DeleteRangeUser