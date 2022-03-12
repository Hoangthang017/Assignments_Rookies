import { useEffect, useRef, useState } from 'react';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
// material
import { Menu, MenuItem, IconButton, ListItemIcon, ListItemText } from '@mui/material';
// component
import Iconify from '../../components/Iconify';
import { func } from 'prop-types';
import DeteleProductImage from '../../api/product/DeleteProductImage';

// ----------------------------------------------------------------------

export default function ProductImageMoreMenu({
  id,
  setIdRemoveRow,
  IMAGE_LIST,
  setSourceImage,
  setNewCaption,
  setIsNewDefault,
  setExpendUpload,
  setImageId
}) {
  const navigate = useNavigate();
  const ref = useRef(null);
  const [isOpen, setIsOpen] = useState(false);

  async function handleRemove() {
    var result = await DeteleProductImage({imageId:id})
    if (result) {
        setIdRemoveRow(true)
        setIsOpen(false);
        setExpendUpload(false);
        setImageId(0);
    };
  }

  function ShowEditHandler(){
      const currentImage =IMAGE_LIST.filter(i => i.id === id)[0];
      if (currentImage){
        setSourceImage(currentImage.imagePath);
        setNewCaption(currentImage.caption);
        setIsNewDefault(currentImage.isDefault);
        setExpendUpload(true);
        setImageId(id);
        setIsOpen(false);
        window.scrollTo({ left: 0, top: document.body.scrollHeight, behavior: "smooth" })
      }
  }
  return (
    <>
      <IconButton ref={ref} onClick={() => setIsOpen(true)}>
        <Iconify icon="eva:more-vertical-fill" width={20} height={20} />
      </IconButton>

      <Menu
        open={isOpen}
        anchorEl={ref.current}
        onClose={() => setIsOpen(false)}
        PaperProps={{
          sx: { width: 200, maxWidth: '100%' }
        }}
        anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
        transformOrigin={{ vertical: 'top', horizontal: 'right' }}
      >
        <MenuItem sx={{ color: 'text.secondary' }} onClick={handleRemove}>
          <ListItemIcon>
            <Iconify icon="eva:trash-2-outline" width={24} height={24} />
          </ListItemIcon>
          <ListItemText primary="Delete" primaryTypographyProps={{ variant: 'body2' }} />
        </MenuItem>

        <MenuItem
          onClick={ShowEditHandler}
          sx={{ color: 'text.secondary' }}
        >
          <ListItemIcon>
            <Iconify icon="eva:edit-fill" width={24} height={24} />
          </ListItemIcon>
          <ListItemText primary="Edit" primaryTypographyProps={{ variant: 'body2' }} />
        </MenuItem>
      </Menu>
    </>
  );
}
