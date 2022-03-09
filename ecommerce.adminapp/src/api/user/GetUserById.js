import axios from "../axios"

async function GetUserById(userId) {
    try {
        const user = await axios.get(`api/Users/${userId}`,
        { headers: { Authorization: sessionStorage.getItem("token")} 
       })

       return user;
    }
    catch (err){
        console.log(err);
    }
    
}

export default GetUserById