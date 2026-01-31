import { useMemo } from 'react';

import { useCartState } from '~/contexts';
import { cartCount } from '~/utils/cartCount';

type UseCartCount = () => number;

const useCartCount: UseCartCount = () => {
  const cart = useCartState();

  return useMemo(() => cartCount(Object.values(cart)), [cart]);
};

export { useCartCount };
