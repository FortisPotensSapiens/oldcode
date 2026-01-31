import { useMemo } from 'react';

import { CartItem, useCartState } from '~/contexts';

type UseSellerGoods = () => CartItem[][];

const useSellerGoods: UseSellerGoods = () => {
  const cart = useCartState();

  return useMemo(() => {
    const sellers = Object.values(cart).reduce<Record<string, CartItem[]>>((acc, item) => {
      const { id } = item.merchandise.seller;
      acc[id] = acc[id] || [];
      acc[id].push(item);

      return acc;
    }, {});

    return Object.values(sellers)
      .filter((item) => item.length > 0)
      .sort(([a], [b]) => ((a?.merchandise.seller.title ?? '') > (b?.merchandise.seller.title ?? '') ? 1 : -1));
  }, [cart]);
};

export default useSellerGoods;
