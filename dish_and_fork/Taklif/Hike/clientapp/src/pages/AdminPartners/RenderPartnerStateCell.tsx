import { Chip } from '@mui/material';
import { GridValueGetterParams } from '@mui/x-data-grid';
import { FC } from 'react';

import { PartnerState } from '~/types';

const RenderPartnerStateCell: FC<GridValueGetterParams<PartnerState>> = ({ value }) => {
  if (value === PartnerState.Confirmed) {
    return <Chip color="success" label="Подтвержден" />;
  }

  if (value === PartnerState.Created) {
    return <Chip color="warning" label="Ожидает" />;
  }

  if (value === PartnerState.Blocked) {
    return <Chip color="error" label="Заблокирован" />;
  }

  return <></>;
};

export { RenderPartnerStateCell };
