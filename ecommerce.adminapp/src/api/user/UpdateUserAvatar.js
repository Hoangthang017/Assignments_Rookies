import axios from '../axios'

async function UpdateUserAvatar(props) {
    var bodyFormData = new FormData();
    bodyFormData.append('userId', props.userId);
    bodyFormData.append('caption', props.caption);
    bodyFormData.append('sortOrder', 1);
    bodyFormData.append('ImageFile', props.imageFile);


    try {
        const reponse = await axios.put(
          "api/user/avatar",
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
        return err.response;
      }
}

export default UpdateUserAvatar;