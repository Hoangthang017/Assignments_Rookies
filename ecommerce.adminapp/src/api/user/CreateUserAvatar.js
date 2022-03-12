import axios from '../axios'

async function CreateUserAvatar(props) {
    var bodyFormData = new FormData();
    bodyFormData.append('userId', props.userId);
    bodyFormData.append('caption', props.caption);
    bodyFormData.append('sortOrder', 1);
    bodyFormData.append('ImageFile', props.imageFile);


    try {
        const reponse = await axios.post(
          "api/images/user",
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
        return err.response;
      }
}

export default CreateUserAvatar;