import axios from "../axios"

async function DeleteRangeCategory({categoryIds}) {
    try {
        const response = await axios.patch("api/Categories/deleteRange",
        categoryIds, 
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

export default DeleteRangeCategory;