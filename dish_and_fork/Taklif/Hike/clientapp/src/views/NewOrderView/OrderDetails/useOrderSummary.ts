import { useMemo } from 'react';

import { CartItem } from '~/types';

type OrderSummary = {
  count: number;
  sum: number;
};

type UseOrderSummary = (items: CartItem[]) => OrderSummary;

const useOrderSummary: UseOrderSummary = (items) => {
  return useMemo(
    () =>
      items.reduce(
        (acc, { amount, merchandise }) => {
          acc.count += amount;
          acc.sum += amount * merchandise.price;

          return acc;
        },
        { count: 0, sum: 0 },
      ),
    [items],
  );
};

export { useOrderSummary };
export type { OrderSummary, UseOrderSummary };
