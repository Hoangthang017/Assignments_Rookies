import axios from "../axios";

async function LoginUser(credentials) {
  try {
    const reponse = await axios.post(
      "api/Users/authenticate",
      JSON.stringify({
        ClientId: 'react-admin',
        ClientSecret: 'D013F030-0177-4F0D-AECA-1206D0608408',
        Scope: 'openid profile swaggerApi',
        ...credentials
      }),
      {
        headers: {
          'Content-Type': 'application/json'
        }
      }
    );

    return reponse.data;
  }
  catch(err) {
    console.log(err);
  }
}

export default LoginUser;
