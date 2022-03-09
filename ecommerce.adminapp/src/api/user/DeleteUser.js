import axios from "../axios"

async function DeleteUser(userId) {
    try {
        const response = await axios.delete(`api/Users/${userId}`, 
        { 
            headers: { Authorization:  sessionStorage.getItem("token")} 
        })

        if (response.status === 200)
            return true;
      }
      catch(err) {
        console.log(err);
      }
}

export default DeleteUser