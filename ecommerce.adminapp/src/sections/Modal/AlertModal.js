import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { Button, Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, Slide } from '@mui/material';

const Transition = React.forwardRef(function Transition(props, ref) {
  return <Slide direction="up" ref={ref} {...props} />;
});

AlertModal.propTypes = {
    isOpen: PropTypes.bool,
    setIsOpen: PropTypes.func,
    title: PropTypes.string,
    message: PropTypes.string,
    setAgreeAction: PropTypes.func
};

function AlertModal({isOpen, setIsOpen, title, message,setAgreeAction }) {
  const handleClose = () => {
    setIsOpen(false);
  };

  const handleAgree = () => {
    setIsOpen(false);
    setAgreeAction();
  };

  return (
    <div>
      <Dialog
        open={isOpen ?? false}
        TransitionComponent={Transition}
        keepMounted
        onClose={handleClose}
        aria-describedby="alert-dialog-slide-description"
      >
        <DialogTitle>{title}</DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-slide-description">
               {message}
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Disagree</Button>
          <Button type='submit' onClick={handleAgree}>Agree</Button>
        </DialogActions>
      </Dialog>
    </div>
  );
}

export default AlertModal;
