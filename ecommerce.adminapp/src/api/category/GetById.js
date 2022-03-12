import axios from "../axios"

async function GetById(categoryId, languageId) {
    try {
        const user = await axios.get(`api/categories/${categoryId}/${languageId}`,
        { headers: { Authorization: sessionStorage.getItem("token")} 
       })

       return user;
    }
    catch (err){
        console.log(err);
    }
    
}

export default GetById;