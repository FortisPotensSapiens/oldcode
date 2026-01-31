import { Button, Dialog, DialogActions, DialogTitle } from '@mui/material';
import { useEffect, useState } from 'react';

const ConfirmDeleteDialog = ({
  itemId,
  text,
  onClose,
  onDelete,
}: {
  itemId: string | undefined;
  text: string;
  onClose: () => void;
  onDelete: () => void;
}) => {
  const [open, setOpen] = useState(!!itemId);

  const handleCancel = () => {
    onClose();
  };

  const handleSuccess = () => {
    onDelete();
  };

  useEffect(() => {
    setOpen(!!itemId);
  }, [itemId]);

  return (
    <Dialog open={open}>
      <DialogTitle>{text}</DialogTitle>

      <DialogActions>
        <Button autoFocus onClick={handleCancel}>
          Отмена
        </Button>
        <Button onClick={handleSuccess}>Удалить</Button>
      </DialogActions>
    </Dialog>
  );
};

export default ConfirmDeleteDialog;
