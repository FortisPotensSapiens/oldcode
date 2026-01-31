import { Box } from '@mui/system';
import { FC } from 'react';

import { MerchandiseUnitType } from '~/types';
import { getNormalizedServingSize } from '~/utils';

const SummServingSize: FC<{ size: number }> = ({ size }) => {
  return size ? (
    <Box display="flex" justifyContent="space-between" mb={3} style={{ fontSize: '80%', opacity: '0.8' }}>
      <span>Суммарный вес брутто</span>
      <span>{getNormalizedServingSize(size, MerchandiseUnitType.Kilograms)}</span>
    </Box>
  ) : (
    <></>
  );
};

export { SummServingSize };
