import { Box } from '@mui/material';
import { GridRenderCellParams } from '@mui/x-data-grid';
import { FC } from 'react';

import { FileReadModel } from '~/types';

const RenderLogoCell: FC<GridRenderCellParams<FileReadModel | undefined>> = ({ value }) => {
  const path = value?.path;

  if (!path) {
    return null;
  }

  return (
    <Box
      borderRadius="50%"
      component="img"
      height={64}
      src={path}
      sx={{ backgroundImage: `url(${path})`, backgroundSize: 'cover' }}
      width={64}
    />
  );
};

export { RenderLogoCell };
