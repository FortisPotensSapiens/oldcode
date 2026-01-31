import { useMemo } from 'react';

import { CartItem } from '~/contexts';

type UseSortItems = (items: CartItem[]) => CartItem[];

const useSortItems: UseSortItems = (items) =>
  useMemo(
    () =>
      items.sort((a, b) =>
        a.merchandise.title > b.merchandise.title ? 1 : a.merchandise.title < b.merchandise.title ? -1 : 0,
      ),
    [items],
  );

export default useSortItems;
