import { useCallback, useEffect } from 'react';
import { useImmer } from 'use-immer';

import { MerchandiseReadModel } from '~/types';

import { LOCALSTORE_CART_KEY } from './contants';
import initCart from './initCart';
import { CartContext } from './types';

type UseCartContext = () => CartContext;

const useCartContext: UseCartContext = () => {
  const [state, setState] = useImmer(initCart);

  const change = useCallback(
    (merchandise: MerchandiseReadModel, amount: number) => {
      const { id } = merchandise;

      setState((draft) => {
        if (amount > 0) {
          draft[id] = {
            amount,
            merchandise,
            updated: new Date(),
          };
        } else if (draft[id]) {
          delete draft[id];
        }
      });
    },
    [setState],
  );

  const increase = useCallback(
    (merchandise: MerchandiseReadModel, amount = 1) => {
      const { id } = merchandise;

      setState((draft) => {
        draft[id] = {
          amount: (draft[id]?.amount ?? 0) + amount,
          merchandise,
          updated: new Date(),
        };
      });
    },
    [setState],
  );

  const decrease = useCallback(
    (merchandise: MerchandiseReadModel, amount = 1) => {
      const { id } = merchandise;

      setState((draft) => {
        if (!draft[id]) {
          return;
        }

        if (draft[id].amount > amount) {
          draft[id] = {
            amount: draft[id].amount - amount,
            merchandise,
            updated: new Date(),
          };
        } else {
          delete draft[id];
        }
      });
    },
    [setState],
  );

  useEffect(() => window.localStorage.setItem(LOCALSTORE_CART_KEY, JSON.stringify(state)), [state]);

  return [state, { change, decrease, increase }];
};

export default useCartContext;
