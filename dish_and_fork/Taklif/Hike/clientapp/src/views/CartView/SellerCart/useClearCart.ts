import { useCallback, useEffect, useRef } from 'react';

import { CartItem, useCartActions } from '~/contexts';

type UseClearCart = (items: CartItem[]) => () => void;

const useClearCart: UseClearCart = (items) => {
  const { change } = useCartActions();
  const ref = useRef(items);

  useEffect(() => {
    ref.current = items;
  }, [items]);

  return useCallback(() => {
    ref.current.forEach(({ merchandise }) => {
      change(merchandise, 0);
    });
  }, [change]);
};

export default useClearCart;
