import { Box } from '@mui/material';

import { Row } from './utils';
// eslint-disable-next-line
const RenderPhotoCell = ({ row }: { row: Row }) => {
  if (!row.photo) {
    return null;
  }

  return (
    <Box
      borderRadius={1}
      height={44}
      style={{ backgroundImage: `url(${row.photo})` }}
      sx={{ backgroundSize: 'cover' }}
      width={58}
    />
  );
};

export { RenderPhotoCell };
