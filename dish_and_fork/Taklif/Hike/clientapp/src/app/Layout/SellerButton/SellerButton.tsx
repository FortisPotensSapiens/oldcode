import { Box, Button } from '@mui/material';
import { FC } from 'react';

import { ReactComponent as Cook } from '~/assets/icons/cook.svg';
import { useCanBeSeller } from '~/hooks';
import { getBecomeSellerPath } from '~/routing';

import { StyledLink } from './StyledLink';

const SellerButton: FC = () => {
  const canBeSeller = useCanBeSeller();

  if (!canBeSeller) {
    return null;
  }

  return (
    <Button
      color="primary"
      component={StyledLink}
      size="large"
      startIcon={<Box component={Cook} position="relative" />}
      to={getBecomeSellerPath()}
      variant="outlined"
    >
      <Box position="relative" top={2}>
        Стать продавцом
      </Box>
    </Button>
  );
};

export { SellerButton };
