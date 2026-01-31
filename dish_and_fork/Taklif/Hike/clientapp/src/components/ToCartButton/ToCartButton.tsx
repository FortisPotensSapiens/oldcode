import { Button, styled } from '@mui/material';
import { FC } from 'react';

import { useCartState } from '~/contexts';
import { useDownSm } from '~/hooks';
import { MerchandiseReadModel } from '~/types';

import useClick from './useClick';

type ToCartButtonProps = { merchandise: MerchandiseReadModel };

const StyledButton = styled(Button)(({ theme }) => ({
  [theme.breakpoints.down('sm')]: {
    whiteSpace: 'nowrap',
    fontSize: '70%',
  },
}));

const ToCartButton: FC<ToCartButtonProps> = ({ merchandise }) => {
  const cart = useCartState();
  const isSmall = useDownSm();
  const click = useClick(merchandise);
  const inCart = cart[merchandise.id]?.amount > 0;

  return (
    <StyledButton
      color={inCart ? 'success' : 'primary'}
      fullWidth={isSmall}
      onClick={click}
      size="large"
      variant="contained"
    >
      {inCart ? 'В корзине' : 'В корзину'}
    </StyledButton>
  );
};

export type { ToCartButtonProps };
export { ToCartButton };
