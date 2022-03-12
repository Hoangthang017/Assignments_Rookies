import axios from '../axios'

async function UpdateProductImage(props) {
    var bodyFormData = new FormData();
    bodyFormData.append('isDefault', props.isDefault);
    bodyFormData.append('caption', props.caption);
    bodyFormData.append('sortOrder', 1);


    try {
        const reponse = await axios.put(
          `api/Images/product/${props.productId}/${props.imageId}`,
          bodyFormData,
          {
            headers: {
              "Content-Type": "multipart/form-data",
              Authorization:  sessionStorage.getItem("token")
            }
          }
        );
    
        return reponse.status;
      }
      catch(err) {
        console.log(err.response);
      }
}

export default UpdateProductImage;