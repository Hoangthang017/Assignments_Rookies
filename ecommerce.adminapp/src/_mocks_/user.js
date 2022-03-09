import { faker } from '@faker-js/faker';
import { sample } from 'lodash';
// utils
import { mockImgAvatar } from '../utils/mockImages';

// ----------------------------------------------------------------------

const users = [...Array(24)].map((_, index) => ({
  id: faker.datatype.uuid(),
  avatarUrl: "https://serving.photos.photobox.com/1651493216a133738ce610dd288ef961463a44cafe827227e79ed1b5a635475d2b92c23e.jpg",
  name: faker.name.findName(),
  userName: "admin",
  password: faker.name.findName(),
  // company: faker.company.companyName(),
  // isVerified: faker.datatype.boolean(),
  // status: sample(['active', 'banned']),
  role: sample([
    'Leader',
    'Hr Manager',
    'UI Designer',
    'UX Designer',
    'UI/UX Designer',
    'Project Manager',
    'Backend Developer',
    'Full Stack Designer',
    'Front End Developer',
    'Full Stack Developer'
  ])
}));

export default users;
