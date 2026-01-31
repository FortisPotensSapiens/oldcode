import AddIcon from '@mui/icons-material/Add';
import RemoveIcon from '@mui/icons-material/Remove';
import { Box } from '@mui/material';
import { Tooltip } from 'antd';
import { FC, useCallback, useMemo } from 'react';

import { useCartActions, useCartState } from '~/contexts';
import { MerchandiseReadModel } from '~/types';

import ChangeCountButton from './ChangeCountButton';

type CartItemChangeProps = { merchandise: MerchandiseReadModel };

const CartItemChange: FC<CartItemChangeProps> = ({ merchandise }) => {
  const cart = useCartState();
  const { decrease, increase } = useCartActions();
  const reachLimit = useMemo(() => {
    const itemOnCard = cart[merchandise.id];

    if (itemOnCard?.amount === merchandise.availableQuantity) {
      return true;
    }

    return false;
  }, [cart, merchandise.availableQuantity, merchandise.id]);
  const onIncreaseCallback = useCallback(() => {
    if (!reachLimit) {
      increase(merchandise);
    }
  }, [increase, merchandise, reachLimit]);

  return (
    <Box alignItems="center" display="flex" width={110}>
      <ChangeCountButton component={RemoveIcon} onClick={() => decrease(merchandise)} />

      <Box color="common.black" flexGrow={1} fontSize={16} textAlign="center">
        {cart[merchandise.id]?.amount ?? 0}
      </Box>

      <Tooltip zIndex={reachLimit ? 999 : -1} title={`В наличии: ${merchandise.availableQuantity}`}>
        <div>
          <ChangeCountButton disabled={reachLimit} component={AddIcon} onClick={onIncreaseCallback} />
        </div>
      </Tooltip>
    </Box>
  );
};

export { CartItemChange };
export type { CartItemChangeProps };
