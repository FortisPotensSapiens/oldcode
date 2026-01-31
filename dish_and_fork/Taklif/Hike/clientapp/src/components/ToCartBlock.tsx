import { Box } from '@mui/material';
import { FC } from 'react';

import { useCartState } from '~/contexts';
import { MerchandiseReadModel } from '~/types';

import { CartItemChange } from './CartItemChange';
import { ToCartButton } from './ToCartButton';

type ToCartBlockProps = { merchandise: MerchandiseReadModel };

const ToCartBlock: FC<ToCartBlockProps> = ({ merchandise }) => {
  const cart = useCartState();

  return cart[merchandise.id]?.amount > 0 ? (
    <Box display="flex" height={42}>
      <CartItemChange merchandise={merchandise} />
    </Box>
  ) : (
    <ToCartButton merchandise={merchandise} />
  );
};

export { ToCartBlock };
export type { ToCartBlockProps };
