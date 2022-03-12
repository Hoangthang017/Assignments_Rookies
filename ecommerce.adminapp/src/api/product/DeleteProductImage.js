import axios from "../axios"

async function DeteleProductImage({imageId}) {
    try {
        const response = await axios.delete(`api/images/${imageId}`, 
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

export default DeteleProductImage