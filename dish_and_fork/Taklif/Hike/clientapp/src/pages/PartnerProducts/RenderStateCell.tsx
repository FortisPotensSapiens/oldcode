import { Box, FormControlLabel, Switch } from '@mui/material';
import { SyntheticEvent } from 'react';

import { MerchandisesState } from '~/types';

import { MAP_STATUS_COLOR, MAP_STATUS_NAME, Row } from './utils';

const RenderStateCell = ({
  row,
  onTogglePublish,
}: {
  row: Row;
  onTogglePublish: (rowId: string, state: boolean) => void;
}) => {
  const onTogglePublishCallback = (e: SyntheticEvent, checked: boolean) => {
    e.stopPropagation();

    onTogglePublish(row.id, checked);
  };

  return (
    <Box color={MAP_STATUS_COLOR[row.state]}>
      <FormControlLabel
        onChange={onTogglePublishCallback}
        control={<Switch defaultChecked={row.state === MerchandisesState.Published} />}
        label={MAP_STATUS_NAME[row.state]}
      />
    </Box>
  );
};

export { RenderStateCell };
