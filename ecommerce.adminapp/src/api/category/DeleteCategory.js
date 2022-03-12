import axios from "../axios"

async function DeleteCategory(categoryIds) {
    try {
        const response = await axios.delete(`api/categories/${categoryIds}`, 
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

export default DeleteCategory;