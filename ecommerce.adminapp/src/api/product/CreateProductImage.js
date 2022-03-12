import axios from '../axios'

async function CreateProductImage(props) {
    var bodyFormData = new FormData();
    bodyFormData.append('productId', props.productId);
    bodyFormData.append('isDefault', props.isDefault);
    bodyFormData.append('caption', props.caption);
    bodyFormData.append('sortOrder', 1);
    bodyFormData.append('ImageFile', props.imageFile);


    try {
        const reponse = await axios.post(
          "api/images/product",
          bodyFormData,
          {
            headers: {
              "Content-Type": "multipart/form-data",
              Authorization:  sessionStorage.getItem("token")
            }
          }
        );
    
        return reponse.data;
      }
      catch(err) {
        console.log(err.response);
      }
}

export default CreateProductImage;