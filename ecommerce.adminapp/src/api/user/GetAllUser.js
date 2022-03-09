import { faker } from '@faker-js/faker';
import { sample } from 'lodash';
import axios from '../axios';

async function GetAllUser() {
  try {
    const response = await axios.get('api/Users', { headers: { Authorization:  sessionStorage.getItem("token")} })

    let users = response.data;

    return users;
  }
  catch(err) {
    console.log(err);
  }
}



// function GetAllUser() {
//     const users = [...Array(24)].map((_, index) => ({
//         id: faker.datatype.uuid(),
//         avatarUrl: "https://serving.photos.photobox.com/1651493216a133738ce610dd288ef961463a44cafe827227e79ed1b5a635475d2b92c23e.jpg",
//         name: faker.name.findName(),
//         userName: "admin",
//         email: faker.name.findName(),
//         // company: faker.company.companyName(),
//         // isVerified: faker.datatype.boolean(),
//         // status: sample(['active', 'banned']),
//         dateOfBirth: "00/00/00",
//         phone: "10123",
//         role: sample([
//           'Leader',
//           'Hr Manager',
//           'UI Designer',
//           'UX Designer',
//           'UI/UX Designer',
//           'Project Manager',
//           'Backend Developer',
//           'Full Stack Designer',
//           'Front End Developer',
//           'Full Stack Developer'
//         ])
//       }));
//     return users;
// }

export default GetAllUser;