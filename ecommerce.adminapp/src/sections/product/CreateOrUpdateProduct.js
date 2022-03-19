import React, { useEffect } from 'react';
import Page from '../../components/Page';
import * as Yup from 'yup';
import { useState } from 'react';
import { Link as RouterLink, useNavigate, useParams } from 'react-router-dom';
import { useFormik, Form, FormikProvider, yupToFormErrors } from 'formik';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
// material
import {
  FormControlLabel,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Stack,
  TextField,
  Alert,
  Container,
  Typography,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Switch,
  InputAdornment
} from '@mui/material';
import { LoadingButton } from '@mui/lab';
// component
import { styled } from '@mui/material/styles';
import UpdateUserAvatar from 'src/api/user/UpdateUserAvatar';
import UnstyledSelectObjectValues from './Dropdown';
import ImageProductTable from './ImageProductTable';
import CreateProduct from 'src/api/product/CreateProduct';
import GetProductById from 'src/api/product/GetProductById';
import UpdateProduct from 'src/api/product/UpdateProduct';
import UpdateProductPrice from 'src/api/product/UpdateProductPrice';
import UpdateProductQuantity from 'src/api/product/UpdateProductQuantity';
import AlertModal from '../Modal/AlertModal';

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

const handleChange = (event) => {
  setLanguageId(event.target.value);
};

function CreateOrUpdateProduct() {
  // states
  const params = useParams();
  const navigate = useNavigate();
  const [updateSuccess, setUpdateSuccess] = useState(false);
  const [createSuccess, setCreateSuccess] = useState(false);
  const [initalUser, setInitalUser] = useState({
    name: '',
    description: '',
    details: '',
    seoTitle: '',
    seoAlias: '',
    seoDescription: '',
    stock: 0
  });
  const [createDate, setCreateDate] = useState(new Date(new Date()).toLocaleDateString('en-US'));
  const [updateDate, setupdateDate] = useState(new Date(new Date()).toLocaleDateString('en-US'));
  const [category, setCategory] = useState({ id: 0, name: 'Select Category' });
  const [viewCount, setViewCount] = useState(0);
  const [originalPrice, setOriginalPrice] = useState(0);
  const [price, setPrice] = useState(0);
  const [stock, setStock] = useState(0);
  const [languageId, setLanguageId] = useState('en-US');
  const [isShowOnHome, setIsShowOnHome] = useState(true);

  // modal
  const [isOpenConfirmModal, setIsOpenConfirmModal] = useState(false);

  // check changed
  const [priceChanged, setPriceChanged] = useState(false);
  const [stockChanged, setStockChanged] = useState(false);

  // validation
  const RegisterSchema = Yup.object().shape({
    name: Yup.string()
      .required('Name is required')
      .max(200, 'Name has max length is 200 characters'),
    description: Yup.string()
      .required('Description is required')
      .max(200, 'Description has max length is 200 characters'),
    details: Yup.string()
      .required('Detail is required')
      .max(500, 'Detail has max length is 200 characters'),
    seoTitle: Yup.string()
      .required('Seo Title is required')
      .max(200, 'Seo title has max length is 200 characters'),
    seoAlias: Yup.string()
      .required('Seo Alias is required')
      .max(200, 'Sep alias has max length is 200 characters'),
    seoDescription: Yup.string()
      .required('Seo description is required')
      .max(200, 'Seo description has max length is 200 characters')
  });
  const [isValidCategoryId, setIsValidCategoryId] = useState(true);

  // call api
  useEffect(async () => {
    if (params.id) {
      const response = await GetProductById(params.id, 'en-US');

      if (response) {
        setInitalUser(response);
        setCreateDate(response.createdDate);
        setupdateDate(response.updatedDate);
        setCategory({
          id: response.categoryId,
          name: response.categoryName
        });
        setViewCount(response.viewCount);
        setOriginalPrice(response.originalPrice);
        setPrice(response.price);
        setStock(response.stock);
        setLanguageId(response.languageId);
        setIsShowOnHome(response.isShowOnHome);
      }
    }
  }, []);

  // use formik to validate from
  const formik = useFormik({
    initialValues: initalUser,
    validationSchema: RegisterSchema,
    enableReinitialize: true,
    // function handle submit
    onSubmit: async () => {
      // base request
      var baseRequest = {
        name: values.name,
        description: values.description,
        details: values.details,
        seoTitle: values.seoTitle,
        seoAlias: values.seoAlias,
        seoDescription: values.seoDescription,
        isShowOnHome
      };

      if (category.id === null || category.id === 0) {
        setIsValidCategoryId(false);
      } else {
        setIsValidCategoryId(true);

        // update
        if (params.id) {
          let result = true;
          result && (await UpdateProduct(params.id, languageId, baseRequest));

          if (priceChanged) {
            result && (await UpdateProductPrice(params.id, price));
          }

          if (stockChanged) {
            result && (await UpdateProductQuantity(params.id, stock));
          }

          if (result) {
            setUpdateSuccess(true);
          } else {
            console.log('Fail to update');
          }
        }
        // create
        else {
          const response = await CreateProduct({
            ...baseRequest,
            categoryId: category.id,
            viewCount,
            languageId,
            originalPrice,
            price,
            stock
          });

          if (response) {
            setCreateSuccess(true);
            navigate(`/product/edit/${response.id}`, { replace: true });
            window.scrollTo(0, document.body.scrollHeight);
          } else {
            console.log('Faild to create product');
          }
        }
      }
    }
  });

  // initial formik
  const { errors, touched, values, handleSubmit, isSubmitting, getFieldProps } = formik;

  // upload image
  const [updateAvatarSuccess, setUpdateAvatarSuccess] = useState(false);
  function UploadHandler(e) {
    var file = e.target.files[0];
    setUserAvatar([file]);
    var reader = new FileReader();
    var url = reader.readAsDataURL(file);

    reader.onloadend = function (e) {
      setSourceImage(reader.result);
    }.bind(this);
  }

  // save image
  async function SaveImageHandler() {
    if (params.id) {
      const responseUpdateImage = await UpdateUserAvatar({
        userId: params.id,
        caption: userAvatar[0].name,
        imageFile: userAvatar[0]
      });
      if (responseUpdateImage === 200) {
        setUserAvatar([]);
        setUpdateAvatarSuccess(true);
      }
    }
  }

  function changePriceHandler(e) {
    setPrice(e.target.value);
    setPriceChanged(true);
  }

  function changeStockHandler(e) {
    setStock(e.target.value);
    setStockChanged(true);
  }

  return (
    <Page title={`${params.id ? 'Edit Product' : 'New Product'} | Mystic Green`}>
      <Container>
        <AlertModal></AlertModal>
        <Typography variant="h4" gutterBottom>
          {params.id ? 'Edit Product' : 'New Product'}
        </Typography>
        {/* basic information */}
        <Accordion>
          <AccordionSummary
            expandIcon={<ExpandMoreIcon />}
            aria-controls="panel1a-content"
            id="panel1a-header"
          >
            <Typography>Basic Information</Typography>
          </AccordionSummary>
          <AccordionDetails>
            <FormikProvider value={formik}>
              <Form autoComplete="off" noValidate onSubmit={handleSubmit}>
                <TextField
                  multiline={true}
                  sx={{ mt: '0.5rem', mb: '0.5rem' }}
                  fullWidth
                  label="Name"
                  {...getFieldProps('name')}
                  error={Boolean(touched.name && errors.name)}
                  helperText={touched.name && errors.name}
                />
                <TextField
                  multiline={true}
                  sx={{ mt: '0.5rem', mb: '0.5rem' }}
                  fullWidth
                  label="Description"
                  {...getFieldProps('description')}
                  error={Boolean(touched.description && errors.description)}
                  helperText={touched.description && errors.description}
                />
                <TextField
                  sx={{ mt: '0.5rem', mb: '0.5rem' }}
                  multiline={true}
                  fullWidth
                  label="Details"
                  {...getFieldProps('details')}
                  error={Boolean(touched.details && errors.details)}
                  helperText={touched.details && errors.details}
                />

                <Stack direction="row" justifyItems="center">
                  {params.id ? (
                    <TextField
                      multiline={true}
                      disabled
                      sx={{ mt: '0.5rem', mb: '0.5rem', width: '40%' }}
                      fullWidth
                      label="Category"
                      value={category.name}
                    />
                  ) : (
                    <UnstyledSelectObjectValues
                      setCategoryParent={setCategory}
                      initialCategory={category}
                    />
                  )}
                  {!isValidCategoryId ?? (
                    <Alert severity="danger" sx={{ mt: '1rem', mb: '1rem' }}>
                      Please choose a category for product
                    </Alert>
                  )}

                  {/* language  */}
                  <FormControl
                    sx={{ mt: '0.5rem', mb: '0.5rem', ml: '3rem', minWidth: 120, width: '30%' }}
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
                      <MenuItem value={'en-US'}>English</MenuItem>
                    </Select>
                  </FormControl>

                  <FormControlLabel
                    sx={{ ml: '2rem' }}
                    control={
                      <Android12Switch
                        checked={isShowOnHome}
                        onChange={(e) => setIsShowOnHome(e.target.checked)}
                      />
                    }
                    label="Show On Home"
                  />
                </Stack>
              </Form>
            </FormikProvider>
          </AccordionDetails>
        </Accordion>

        {/* SEO information */}
        <Accordion>
          <AccordionSummary
            expandIcon={<ExpandMoreIcon />}
            aria-controls="panel1a-content"
            id="panel1a-header"
          >
            <Typography>SEO Information</Typography>
          </AccordionSummary>
          <AccordionDetails>
            <FormikProvider value={formik}>
              <Form autoComplete="off" noValidate onSubmit={handleSubmit}>
                <TextField
                  sx={{ mt: '0.5rem', mb: '0.5rem' }}
                  fullWidth
                  multiline={true}
                  label="Seo Title"
                  {...getFieldProps('seoTitle')}
                  error={Boolean(touched.seoTitle && errors.seoTitle)}
                  helperText={touched.seoTitle && errors.seoTitle}
                />
                <TextField
                  sx={{ mt: '0.5rem', mb: '0.5rem' }}
                  fullWidth
                  label="Seo Alias"
                  multiline={true}
                  {...getFieldProps('seoAlias')}
                  error={Boolean(touched.seoAlias && errors.seoAlias)}
                  helperText={touched.seoAlias && errors.seoAlias}
                />
                <TextField
                  sx={{ mt: '0.5rem', mb: '0.5rem' }}
                  fullWidth
                  label="Seo Description"
                  multiline={true}
                  {...getFieldProps('seoDescription')}
                  error={Boolean(touched.seoDescription && errors.seoDescription)}
                  helperText={touched.seoDescription && errors.seoDescription}
                />
              </Form>
            </FormikProvider>
          </AccordionDetails>
        </Accordion>

        {/* Date */}
        <Accordion>
          <AccordionSummary
            expandIcon={<ExpandMoreIcon />}
            aria-controls="panel1a-content"
            id="panel1a-header"
          >
            <Typography>Date</Typography>
          </AccordionSummary>
          <AccordionDetails>
            <TextField
              disabled
              sx={{ mt: '0.5rem', mb: '0.5rem' }}
              fullWidth
              label="Created Date"
              value={createDate}
            />
            <TextField
              disabled
              sx={{ mt: '0.5rem', mb: '0.5rem' }}
              fullWidth
              label="Update Date"
              value={updateDate}
            />
          </AccordionDetails>
        </Accordion>

        {/* Statitics */}
        <Accordion>
          <AccordionSummary
            expandIcon={<ExpandMoreIcon />}
            aria-controls="panel1a-content"
            id="panel1a-header"
          >
            <Typography>Statitics</Typography>
          </AccordionSummary>
          <AccordionDetails>
            <FormikProvider value={formik}>
              <Form autoComplete="off" noValidate onSubmit={handleSubmit}>
                <TextField
                  type="number"
                  disabled={!!params.id}
                  sx={{ mt: '0.5rem', mb: '0.5rem' }}
                  fullWidth
                  required
                  error={!Number.isInteger(parseInt(stock))}
                  label="Original Price"
                  InputProps={{
                    startAdornment: <InputAdornment position="start">$</InputAdornment>
                  }}
                  value={originalPrice}
                  onChange={(e) => setOriginalPrice(e.target.value)}
                />
                <TextField
                  type="number"
                  sx={{ mt: '0.5rem', mb: '0.5rem' }}
                  fullWidth
                  required
                  label="Price"
                  error={!Number.isInteger(parseInt(stock))}
                  InputProps={{
                    startAdornment: <InputAdornment position="start">$</InputAdornment>
                  }}
                  value={price}
                  onChange={changePriceHandler}
                />

                <TextField
                  type="number"
                  sx={{ mt: '0.5rem', mb: '0.5rem' }}
                  fullWidth
                  required
                  label="Stock"
                  error={!Number.isInteger(parseFloat(stock))}
                  helperText="Quantity must have interger type"
                  InputProps={{
                    startAdornment: <InputAdornment position="start">kg</InputAdornment>
                  }}
                  value={stock}
                  onChange={changeStockHandler}
                />

                <TextField
                  disabled
                  sx={{ mt: '0.5rem', mb: '0.5rem' }}
                  fullWidth
                  label="View Count"
                  value={viewCount}
                />
              </Form>
            </FormikProvider>
          </AccordionDetails>
        </Accordion>

        {createSuccess && (
          <Alert severity="success" sx={{ mt: '1rem', mb: '1rem' }}>
            Create Successfully!!! Add product Image Here!!!
          </Alert>
        )}
        {/* images */}
        {params.id && (
          <Accordion>
            <AccordionSummary
              expandIcon={<ExpandMoreIcon />}
              aria-controls="panel1a-content"
              id="panel1a-header"
            >
              <Typography>Images</Typography>
            </AccordionSummary>
            <AccordionDetails>
              <ImageProductTable productId={params.id} />
            </AccordionDetails>
          </Accordion>
        )}

        {updateSuccess && (
          <Alert severity="success" sx={{ mb: '3rem' }}>
            Success!!!
          </Alert>
        )}
        {/* register button */}
        <FormikProvider value={formik}>
          {/* <Form autoComplete="off" noValidate onSubmit={handleSubmit}> */}
          <Form autoComplete="off" noValidate>
            {!params.id ? (
              <LoadingButton
                sx={{ mt: '1.5rem' }}
                fullWidth
                size="large"
                type="submit"
                variant="contained"
                loading={isSubmitting}
              >
                Create
              </LoadingButton>
            ) : (
              <>
                <LoadingButton
                  sx={{ mt: '1.5rem' }}
                  fullWidth
                  size="large"
                  variant="contained"
                  loading={isSubmitting}
                  onClick={() => setIsOpenConfirmModal(true)}
                >
                  Update
                </LoadingButton>
                <AlertModal
                  isOpen={isOpenConfirmModal}
                  setIsOpen={setIsOpenConfirmModal}
                  title={'Update Confirm'}
                  message={'Are you sure want to update new information'}
                  setAgreeAction={handleSubmit}
                ></AlertModal>
              </>
            )}
          </Form>
        </FormikProvider>
      </Container>
    </Page>
  );
}

export default CreateOrUpdateProduct;
