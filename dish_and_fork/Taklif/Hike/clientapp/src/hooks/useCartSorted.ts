import { useEffect, useMemo, useRef } from 'react';

import { CartItem, useCartState } from '~/contexts';

type UseCartSorted = (filterFn?: (item: CartItem) => boolean) => CartItem[];

const useCartSorted: UseCartSorted = (filterFn) => {
  const state = useCartState();
  const ref = useRef(filterFn);

  useEffect(() => {
    ref.current = filterFn;
  }, [filterFn]);

  return useMemo(() => {
    const values = Object.values(state);
    const prepared = ref.current ? values.filter(ref.current) : values;

    return prepared.sort((a, b) => (a.merchandise.title < b.merchandise.title ? -1 : 1));
  }, [state]);
};

export { useCartSorted };
