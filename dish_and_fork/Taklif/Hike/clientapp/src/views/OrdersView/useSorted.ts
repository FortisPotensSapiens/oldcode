import { useMemo } from 'react';

import { OrderReadModel } from '~/types';

type UseSorted = (items?: OrderReadModel[] | null) => OrderReadModel[];

const useSorted: UseSorted = (items) => {
  return useMemo(() => {
    if (!items) {
      return [];
    }

    return items
      .sort((a, b) => {
        if (a.paymentDate !== b.paymentDate) {
          return a.paymentDate && a.paymentDate > (b.paymentDate ?? '') ? 1 : -1;
        }

        if (a.deliveredDate !== b.deliveredDate) {
          return a.deliveredDate && a.deliveredDate > (b.deliveredDate ?? '') ? 1 : -1;
        }

        return a.created > b.created ? 1 : -1;
      })
      .reverse();
  }, [items]);
};

export { useSorted };
