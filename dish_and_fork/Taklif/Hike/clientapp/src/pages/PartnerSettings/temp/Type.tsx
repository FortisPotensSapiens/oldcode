import { Box, Button, ButtonGroup } from '@mui/material';
import { forwardRef, MouseEventHandler } from 'react';

import { PartnerType } from '~/types';

type TypeProps = {
  name: string;
  value: PartnerType;
  onChange: (value: string | null) => void;
};

const titles: Record<PartnerType, string> = {
  [PartnerType.Company]: 'Компания',
  [PartnerType.IndividualEntrepreneur]: 'ИП',
  [PartnerType.SelfEmployed]: 'Самозанятый',
};

const items = [PartnerType.SelfEmployed, PartnerType.IndividualEntrepreneur, PartnerType.Company];

const Type = forwardRef<HTMLInputElement, TypeProps>(function Type({ onChange, ...props }, ref) {
  const clickHandler: MouseEventHandler<HTMLButtonElement> = (event) =>
    onChange(event.currentTarget.getAttribute('data-value'));

  return (
    <>
      <input ref={ref} type="hidden" {...props} />

      <ButtonGroup disableElevation size="large" variant="outlined">
        {items.map((key) => (
          <Box
            key={key}
            borderColor={({ palette }) => (props.value === key ? undefined : palette.divider)}
            component={Button}
            data-value={key}
            fontWeight="400"
            onClick={clickHandler}
            textTransform="none"
            variant={props.value === key ? 'contained' : undefined}
          >
            {titles[key]}
          </Box>
        ))}
      </ButtonGroup>
    </>
  );
});

export { Type };
export type { TypeProps };
