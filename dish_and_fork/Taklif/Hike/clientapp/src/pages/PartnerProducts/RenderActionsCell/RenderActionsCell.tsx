import { Edit } from '@mui/icons-material';
import { Box, Button, Grid } from '@mui/material';
import { SyntheticEvent } from 'react';

import { Row } from '../utils';
import { useActions } from './useActions';

const RenderActionsCell = ({ row, onDelete }: { row: Row; onDelete: (rowId: string) => void }) => {
  const { edit } = useActions(row.id);

  const onDeleteCallback = (e: SyntheticEvent) => {
    e.stopPropagation();

    onDelete(row.id);
  };

  return (
    <Box ml={-1}>
      <Grid container spacing={2}>
        <Grid item xs={12}>
          <Button fullWidth onClick={edit} startIcon={<Edit />} variant="contained">
            Редактировать
          </Button>
        </Grid>
        <Grid item xs={12}>
          <Button color="error" fullWidth onClick={onDeleteCallback} variant="contained">
            Удалить
          </Button>
        </Grid>
      </Grid>
    </Box>
  );
};

export { RenderActionsCell };
