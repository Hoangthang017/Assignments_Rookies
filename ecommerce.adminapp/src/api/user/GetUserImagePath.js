import axios from "../axios"

async function GetUserImagePath(userId) {
    try {
        const user = await axios.get(`api/User/${userId}/avatar`,
        { headers: { Authorization: sessionStorage.getItem("token")} 
       })

       return user;
    }
    catch (err){
        console.log(err);
    }
    
}

export default GetUserImagePath;