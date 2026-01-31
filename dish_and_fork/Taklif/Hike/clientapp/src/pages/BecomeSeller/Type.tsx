import { Box, ToggleButton, ToggleButtonGroup } from '@mui/material';
import { forwardRef, MouseEvent } from 'react';

import { PartnerType } from '~/types';
import { getPartnerTypeText } from '~/utils';

type TypeProps = {
  disabled?: boolean;
  name: string;
  value: PartnerType;
  onChange: (value: string | null) => void;
};

const items = [PartnerType.SelfEmployed, PartnerType.IndividualEntrepreneur, PartnerType.Company];

const Type = forwardRef<HTMLInputElement, TypeProps>(function Type({ disabled, name, onChange, value }, ref) {
  const clickHandler = (event: MouseEvent<HTMLElement>, payload: PartnerType) => onChange(payload);

  return (
    <>
      <input ref={ref} name={name} type="hidden" value={value} />

      <ToggleButtonGroup
        color="primary"
        disabled={disabled}
        exclusive
        onChange={clickHandler}
        size="medium"
        value={value}
      >
        {items.map((key) => (
          <Box
            key={key}
            color="text.primary"
            component={ToggleButton}
            disabled={disabled}
            fontSize="1rem"
            fontWeight={400}
            sx={{
              '&.Mui-disabled': {
                color: 'text.disabled',
              },

              '&.Mui-selected': {
                backgroundColor: 'primary.main',
                color: 'primary.contrastText',
              },

              '&.Mui-selected:hover': {
                backgroundColor: 'primary.main',
                color: 'primary.contrastText',
              },

              // eslint-disable-next-line sort-keys-fix/sort-keys-fix
              '&.Mui-selected.Mui-disabled': {
                backgroundColor: 'text.disabled',
                color: 'text.secondary',
              },

              '&.Mui-selected.Mui-disabled:hover': {
                backgroundColor: 'text.disabled',
                color: 'text.secondary',
              },
            }}
            textTransform="none"
            value={key}
          >
            {getPartnerTypeText(key)}
          </Box>
        ))}
      </ToggleButtonGroup>
    </>
  );
});

export { Type };
export type { TypeProps };
