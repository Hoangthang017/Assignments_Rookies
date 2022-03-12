import React, { useEffect } from 'react';
import Page from '../../components/Page';
import * as Yup from 'yup';
import { useState } from 'react';
import { Link as RouterLink, useNavigate, useParams } from 'react-router-dom';
import { useFormik, Form, FormikProvider, yupToFormErrors } from 'formik';
import AdapterDateFns from '@mui/lab/AdapterDateFns';
import LocalizationProvider from '@mui/lab/LocalizationProvider';
// material
import { styled } from '@mui/material/styles';
import {
  Link,
  Stack,
  TextField,
  MenuItem,
  Select,
  FormControlLabel,
  Alert,
  Container,
  Typography,
  Card,
  Switch,
  FormHelperText,
  FormControl,
  InputLabel,
  TextareaAutosize
} from '@mui/material';
import { LoadingButton } from '@mui/lab';
import CreateUserApi from '../../api/user/CreateUserApi';
// component
import Iconify from '../../components/Iconify';
import DatePicker from '@mui/lab/DatePicker';
import { values } from 'lodash';
import GetUserById from 'src/api/user/GetUserById';
import UpdateUser from 'src/api/user/UpdateUser';
import CreateUserAvatar from 'src/api/user/CreateUserAvatar';
import UpdateUserAvatar from 'src/api/user/UpdateUserAvatar';
import GetById from 'src/api/category/GetById';
import UpdateCategory from 'src/api/category/UpdateCategory';
import CreateCategory from 'src/api/category/CreateCategory';
import UpdateShowOnHome from 'src/api/category/UpdateShowOnHome';
import UpdateStatus from 'src/api/category/UpdateStatus';
//-----------------------------------------------------------------------------------------------
const Android12Switch = styled(Switch)(({ theme }) => ({
  padding: 8,
  '& .MuiSwitch-track': {
    borderRadius: 22 / 2,
    '&:before, &:after': {
      content: '""',
      position: 'absolute',
      top: '50%',
      transform: 'translateY(-50%)',
      width: 16,
      height: 16
    },
    '&:before': {
      backgroundImage: `url('data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" height="16" width="16" viewBox="0 0 24 24"><path fill="${encodeURIComponent(
        theme.palette.getContrastText(theme.palette.primary.main)
      )}" d="M21,7L9,19L3.5,13.5L4.91,12.09L9,16.17L19.59,5.59L21,7Z"/></svg>')`,
      left: 12
    },
    '&:after': {
      backgroundImage: `url('data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" height="16" width="16" viewBox="0 0 24 24"><path fill="${encodeURIComponent(
        theme.palette.getContrastText(theme.palette.primary.main)
      )}" d="M19,13H5V11H19V13Z" /></svg>')`,
      right: 12
    }
  },
  '& .MuiSwitch-thumb': {
    boxShadow: 'none',
    width: 16,
    height: 16,
    margin: 2
  }
}));
//------------------------------------------------------------------------------------------------
function CreateOrUpdateCategory() {
  // states
  const params = useParams();
  const navigate = useNavigate();
  const [updateSuccess, setUpdateSuccess] = useState(false);
  const [initalElement, setInitalElement] = useState({
    name: '',
    seoTitle: '',
    seoAlias: '',
    seoDescription: ''
  });
  const [languageId, setLanguageId] = useState('en-us');
  const [isShowOnHome, setIsShowOnHome] = useState(true);
  const [showOnHomeChange, setShowOnHomeChange] = useState(false);
  const [status, setStatus] = useState(1);
  const [statusChange, setStatusChange] = useState(false);

  const handleChange = (event) => {
    setLanguageId(event.target.value);
  };

  function handleShowHomeChange(event) {
    var value = event.target.checked;
    setIsShowOnHome(value);
    setShowOnHomeChange(true);
  }

  function handleStatusChange(event) {
    var value = 0;
    if (event.target.checked) value = 1;
    setStatus(value);
    setStatusChange(true);
  }

  // validation
  const RegisterSchema = Yup.object().shape({
    name: Yup.string().required('Name is required'),
    seoTitle: Yup.string().required('Title is required'),
    seoAlias: Yup.string().required('Alias is required'),
    seoDescription: Yup.string().required('Description is required')
  });

  // call api
  useEffect(async () => {
    if (params.id) {
      const response = await GetById(params.id, languageId);

      if (response.data) {
        setInitalElement(response.data);
        setIsShowOnHome(response.data.isShowOnHome);
        setStatus(response.data.status);
      }
    }
  }, []);

  // use formik to validate from
  const formik = useFormik({
    initialValues: initalElement,
    validationSchema: RegisterSchema,
    enableReinitialize: true,
    // function handle submit
    onSubmit: async () => {
      // base request
      var baseRequest = {
        name: values.name,
        seoTitle: values.seoTitle,
        seoAlias: values.seoAlias,
        seoDescription: values.seoDescription
      };
      // update
      if (params.id) {
        const responseUpdate = await UpdateCategory(params.id, languageId, baseRequest);

        if (showOnHomeChange) {
          const responeShowOnHome = await UpdateShowOnHome(params.id, isShowOnHome);
          setShowOnHomeChange(false);
        }

        if (statusChange) {
          const responeStatus = await UpdateStatus(params.id, status);
          setStatusChange(false);
        }

        setUpdateSuccess(true);
      }
      // create
      else {
        const response = await CreateCategory({
          ...baseRequest,
          isShowOnHome,
          status,
          languageId,
          sortOrder: 1,
          parentId: 0
        });

        if (response) {
          navigate('/category', { replace: true });
        } else {
          console.log('Faild to create new category');
        }
      }
    }
  });

  // initial formik
  const { errors, touched, values, handleSubmit, isSubmitting, getFieldProps } = formik;

  return (
    <Page title={`${params.id ? 'Edit Category' : 'New Category'} | Minimal-UI`}>
      <Container>
        <Typography variant="h4" gutterBottom>
          {params.id ? 'Edit Category' : 'New Category'}
        </Typography>
        <Stack direction="row" justifyContent="center" sx={{ mt: '3rem' }}>
          {/* information */}
          <FormikProvider value={formik}>
            <Form autoComplete="off" noValidate onSubmit={handleSubmit}>
              <Stack spacing={3}>
                {/* information */}
                <Stack direction="row" spacing={3}>
                  {/* basic information */}
                  <Stack>
                    <Card spacing={3} sx={{ p: '3rem' }}>
                      {updateSuccess && (
                        <Alert severity="success" sx={{ mb: '3rem' }}>
                          Success!!!
                        </Alert>
                      )}
                      <TextField
                        fullWidth
                        label="Name"
                        {...getFieldProps('name')}
                        error={Boolean(touched.name && errors.name)}
                        helperText={touched.name && errors.name}
                      />

                      <TextField
                        sx={{ mt: '1.5rem' }}
                        fullWidth
                        label="Title"
                        {...getFieldProps('seoTitle')}
                        error={Boolean(touched.seoTitle && errors.seoTitle)}
                        helperText={touched.seoTitle && errors.seoTitle}
                      />

                      <TextField
                        fullWidth
                        sx={{ mt: '1.5rem', mb: '1.5rem' }}
                        label="Alias"
                        {...getFieldProps('seoAlias')}
                        error={Boolean(touched.seoAlias && errors.seoAlias)}
                        helperText={touched.seoAlias && errors.seoAlias}
                      />

                      <TextField
                        fullWidth
                        label="Description"
                        {...getFieldProps('seoDescription')}
                        error={Boolean(touched.seoDescription && errors.seoDescription)}
                        helperText={touched.seoDescription && errors.seoDescription}
                      />
                    </Card>
                  </Stack>

                  {/* account information */}
                  <Stack sx={{ width: '25%' }}>
                    <Card sx={{ p: '1.5rem' }}>
                      <Stack direction={{ xs: 'column', sm: 'column' }} sx={{ ml: '1rem', mr: 0 }}>
                        <FormControlLabel
                          control={
                            <Android12Switch
                              checked={isShowOnHome}
                              onChange={handleShowHomeChange}
                            />
                          }
                          label="Show On Home"
                        />

                        <FormControlLabel
                          sx={{ mt: '1rem', mb: '1rem' }}
                          control={
                            <Android12Switch checked={status === 1} onChange={handleStatusChange} />
                          }
                          label="Active"
                        />
                      </Stack>

                      {/* language  */}
                      <FormControl
                        sx={{ m: 1, minWidth: 120, mt: 0, width: '100%' }}
                        disabled={!!params.id}
                      >
                        <InputLabel id="demo-simple-select-helper-label">Language</InputLabel>
                        <Select
                          labelId="demo-simple-select-helper-label"
                          id="demo-simple-select-helper"
                          value={languageId}
                          label="Language"
                          onChange={handleChange}
                        >
                          <MenuItem value={'en-us'}>English</MenuItem>
                          <MenuItem value={'vi-VN'}>Vietnamese</MenuItem>
                        </Select>
                      </FormControl>
                    </Card>
                  </Stack>
                </Stack>

                {/* register button */}
                <LoadingButton
                  fullWidth
                  size="large"
                  type="submit"
                  variant="contained"
                  loading={isSubmitting}
                >
                  {params.id ? 'Update' : 'Register'}
                </LoadingButton>
              </Stack>
            </Form>
          </FormikProvider>
        </Stack>
      </Container>
    </Page>
  );
}

export default CreateOrUpdateCategory;
