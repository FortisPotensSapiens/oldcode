import { Box, MenuItem } from '@mui/material';
import { FC } from 'react';
import { useFormContext } from 'react-hook-form';

import { MerchandiseUnitType } from '~/types';
import { getUnitText } from '~/utils';

import { StyledFormTextField } from './StyledFormTextField';
import { useProps } from './useProps';

type ServingUnitProps = {
  disabled?: boolean;
};

const items = [MerchandiseUnitType.Kilograms, MerchandiseUnitType.Liters, MerchandiseUnitType.Pieces];

const ServingUnit: FC<ServingUnitProps> = ({ disabled }) => {
  const props = useProps();
  const { watch } = useFormContext();
  const watchUnitType = watch('unitType');

  return (
    <Box display="flex">
      <StyledFormTextField
        disabled={disabled}
        label="Размер порции"
        linked="unitType"
        name="servingSize"
        noBorder="right"
        type="number"
        valueAsNumber={watchUnitType === MerchandiseUnitType.Pieces}
        rules={{
          required: true,
        }}
        {...props}
      />

      <StyledFormTextField
        disabled={disabled}
        linked="servingSize"
        name="unitType"
        noBorder="left"
        rules={{ required: true }}
        select
        width={70}
        {...props}
      >
        {items.map((item) => (
          <MenuItem key={item} value={item}>
            {getUnitText(item)}
          </MenuItem>
        ))}
      </StyledFormTextField>
    </Box>
  );
};

export { ServingUnit };
export type { ServingUnitProps };
