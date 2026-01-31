import { useMemo } from 'react';

import { CartItem } from '~/contexts';

type UseCartSum = (cart: CartItem[]) => number;

const useCartSum: UseCartSum = (cart) =>
  useMemo(
    () => Object.values(cart).reduce((acc, { amount, merchandise }) => acc + amount * merchandise.price, 0),
    [cart],
  );

export { useCartSum };
