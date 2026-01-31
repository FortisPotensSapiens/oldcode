import styled from '@emotion/styled';
import { Box, IconButton, SvgIcon } from '@mui/material';
import { FC } from 'react';

import usePreventedClick from './usePreventedClick';

type ChangeCountButtonProps = {
  component: typeof SvgIcon;
  onClick: () => void;
  disabled?: boolean;
};

const StyledButton = styled.button`
  all: unset;
  cursor: default;
`;

const ChangeCountButton: FC<ChangeCountButtonProps> = ({ component, onClick, disabled = false }) => {
  const click = usePreventedClick(onClick);
  const rootClick = usePreventedClick();

  return (
    <StyledButton type="button" onClick={rootClick}>
      <Box component={IconButton} onClick={click} p={0} size="small" disabled={disabled}>
        <Box border={1} borderRadius="50%" p={1.25} sx={{ opacity: 0.23 }}>
          <SvgIcon component={component} display="block" fontSize="small" sx={{ display: 'block' }} />
        </Box>
      </Box>
    </StyledButton>
  );
};

export type { ChangeCountButtonProps };
export default ChangeCountButton;
