import PropTypes from 'prop-types';
// material
import { styled } from '@mui/material/styles';
import {
  Toolbar,
  Tooltip,
  IconButton,
  Typography,
  OutlinedInput,
  InputAdornment,
  Stack
} from '@mui/material';
// component
import Iconify from '../../../components/Iconify';
import DeleteRangeUser from 'src/api/user/DeleteRangeUser';
import GetAllName from 'src/api/category/GetAllName';
import DeleteRangeCategory from 'src/api/category/DeleteRangeCategory';
import { useState } from 'react';
import UnstyledSelectObjectValues from 'src/sections/product/Dropdown';
import DeleteProduct from 'src/api/product/DeleteProduct';
import DeleteRangeProduct from 'src/api/product/DeleteRangeProduct';
import AlertModal from 'src/sections/Modal/AlertModal';

// ----------------------------------------------------------------------

const RootStyle = styled(Toolbar)(({ theme }) => ({
  height: 96,
  display: 'flex',
  justifyContent: 'space-between',
  padding: theme.spacing(0, 1, 0, 3)
}));

const SearchStyle = styled(OutlinedInput)(({ theme }) => ({
  width: 240,
  transition: theme.transitions.create(['box-shadow', 'width'], {
    easing: theme.transitions.easing.easeInOut,
    duration: theme.transitions.duration.shorter
  }),
  '&.Mui-focused': { width: 320, boxShadow: theme.customShadows.z8 },
  '& fieldset': {
    borderWidth: `1px !important`,
    borderColor: `${theme.palette.grey[500_32]} !important`
  }
}));

// ----------------------------------------------------------------------

UserListToolbar.propTypes = {
  selected: PropTypes.array,
  numSelected: PropTypes.number,
  filterName: PropTypes.string,
  onFilterName: PropTypes.func,
  setIdRemoveUser: PropTypes.func,
  setSelected: PropTypes.func,
  setCategoryId: PropTypes.func
};

export default function UserListToolbar({
  setSelected,
  selected,
  numSelected,
  filterName,
  onFilterName,
  setIdRemoveRow,
  type,
  setCategoryId
}) {
  async function removeRange() {
    var result = null;
    if (type === 'user') {
      result = await DeleteRangeUser({ userIds: selected });
    } else if (type === 'category') {
      result = await DeleteRangeCategory({ categoryIds: selected });
    } else if (type === 'product') {
      result = await DeleteRangeProduct({ productIds: selected });
    }

    if (result) {
      setIdRemoveRow(true);
      setSelected([]);
    }
  }

  const [isOpenModal, setIsOpenModal] = useState(false);
  return (
    <>
      <RootStyle
        sx={{
          ...(numSelected > 0 && {
            color: 'primary.main',
            bgcolor: 'primary.lighter'
          })
        }}
      >
        {numSelected > 0 ? (
          <Typography component="div" variant="subtitle1">
            {numSelected} selected
          </Typography>
        ) : (
          <SearchStyle
            value={filterName}
            onChange={onFilterName}
            placeholder={`Search ${type} ...`}
            startAdornment={
              <InputAdornment position="start">
                <Iconify icon="eva:search-fill" sx={{ color: 'text.disabled' }} />
              </InputAdornment>
            }
          />
        )}

        {type === 'product' && numSelected === 0 && (
          <>
            <Stack direction="row" justifyContent="center" alignItems="center">
              <Typography component="div" variant="subtitle1" sx={{ mr: '5px' }}>
                Category:
              </Typography>
              <UnstyledSelectObjectValues setCategoryId={setCategoryId} />
            </Stack>
          </>
        )}

        {numSelected > 0 ? (
          <Tooltip title="Delete">
            <IconButton onClick={() => setIsOpenModal(true)}>
              <Iconify icon="eva:trash-2-fill" />
            </IconButton>
          </Tooltip>
        ) : (
          <Tooltip title="Filter list">
            <IconButton>
              <Iconify icon="ic:round-filter-list" />
            </IconButton>
          </Tooltip>
        )}
      </RootStyle>
      <AlertModal
        isOpen={isOpenModal}
        setIsOpen={setIsOpenModal}
        title={'Delete Confirm'}
        message={'Are you sure want to delete your row'}
        setAgreeAction={removeRange}
      ></AlertModal>
    </>
  );
}
